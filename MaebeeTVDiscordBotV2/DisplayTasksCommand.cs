using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DisplayTasksCommand : SlashCommand
{
    List<STask> _tasks;
    public DisplayTasksCommand()
    {
        _name = "display-tasks";
        _description = "Displays all tasks available for you in the Database";
        _ephemeral = true;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);
        var temp = await new STask().GetTasksForPerson(command.User.Username + "#" + command.User.Discriminator);
        _tasks = new List<STask>();
        foreach (var task in temp)
        {
            _tasks.Add(new STask());
            _tasks.Last().SetTask(task);
        }

        for (int i = 0; i < (_tasks.Count / 2); i++)
        {
            embed.Add(new TaskEmbed());
            await embed.Last().SetupEmbed(_tasks.GetRange(i, 2), false);
        }
        if (_tasks.Count % 2 == 1)
        {
            embed.Add(new TaskEmbed());
            await embed.Last().SetupEmbed(_tasks.GetRange(((_tasks.Count / 2) * 2), _tasks.Count % 2), false);
        }

        var embedOutput = new List<Embed>();
        foreach (var embed in embed)
        {
            embedOutput.Add(embed.Build());
        }

        await command.FollowupAsync(embeds: embedOutput.ToArray(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
