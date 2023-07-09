using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class AddTaskCommand : SlashCommand
{
    Task2 _task;
    public AddTaskCommand()
    {
        _name = "add-task";
        _description = "Adds a task to the database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "title",
                "The title of the task",
                true,
                ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "description",
                "The detailed description of the task",
                true,
                ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "team",
                "The team this task is for (only 1)",
                false,
                ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "people",
                "The names of the people this is for (comma separated)",
                false,
                ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "resources",
                "Links to resources available to make this (comma separated)",
                false,
                ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "output",
                "The output location for this task (link or description)",
                false,
                ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "due",
                "The due date of this task (dd/mm/yyyy)",
                false,
                ApplicationCommandOptionType.String)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);

        string[] separator = new string[1];
        separator[0] = ", ";
        string thing;
        string[] peeps = new string[20];
        string title;
        string description;
        string team;
        string output;
        string due;
        try
        {
            thing = command.Data.Options.Where(x => x.Name == "resources").First().Value.ToString();
        }
        catch 
        {
            thing = null;
        }
        try
        {
            peeps = command.Data.Options.Where(x => x.Name == "people").First().Value.ToString().Split(separator, StringSplitOptions.None);
        }
        catch 
        {
            peeps = null;
        }
        if (command.Data.Options.Where(x => x.Name == "title") != null)
        {
            title = command.Data.Options.Where(x => x.Name == "title").First().Value.ToString();
        }
        else
        {
            title = null;
        }
        if (command.Data.Options.Where(x => x.Name == "description") != null)
        {
            description = command.Data.Options.Where(x => x.Name == "description").First().Value.ToString();
        }
        else
        {
            description = null;
        }        
        try
        {
            team = command.Data.Options.Where(x => x.Name == "team").First().Value.ToString();
        }
        catch
        {
            team = null;
        }
        try
        {
            output = command.Data.Options.Where(x => x.Name == "output").First().Value.ToString();
        }
        catch
        {
            output = null;
        }
        try
        {
            due = command.Data.Options.Where(x => x.Name == "due").First().Value.ToString();
        }
        catch
        {
            due = null;
        }

        _task = new Task2();
        _task.setTitle(title);
        _task.setDescription(description);
        _task.setDue(due);
        _task.setCompleted(false);
        _task.setTeam(await new DatabaseTeamController().useName(team));
        _task.setResources(thing);
        _task.setOutput(output);
        foreach (string person in peeps)
        {
            _task.addAssignee(await new DatabasePersonController().useName(person));
        }
        var blah = new TaskEmbed();
        blah.SetupEmbed(_task);
        embed.Add(blah);

        _buttons = new List<TButton>()
        {
            new AcceptButton(HandleAccept, _name),
            new CancelButton(HandleCancel, _name),
        };

        await command.FollowupAsync(embed: embed[0].Build(), ephemeral: Ephemeral, components: GetButtons());
        await command.DeleteOriginalResponseAsync();
    }

    public async Task HandleAccept(SocketMessageComponent command)
    {
        await new DatabaseTaskController().PushToDatabase(_task);
        await command.RespondAsync("Database Updated", ephemeral: Ephemeral);
    }
    public async Task HandleCancel(SocketMessageComponent command)
    {
        _task = null;
        await command.RespondAsync("Datebase Update Cancelled", ephemeral: Ephemeral);
    }
}
