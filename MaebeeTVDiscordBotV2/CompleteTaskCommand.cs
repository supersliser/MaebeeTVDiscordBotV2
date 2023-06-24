using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CompleteTaskCommand : SlashCommand
{
    public CompleteTaskCommand()
    {
        _name = "complete-task";
        _description = "Sets a task to be completed";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "id",
                "The ID of the task you wish to accept",
                true,
                Discord.ApplicationCommandOptionType.Number)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);

        var task = new STask();
        await task.GetFromDatabase(long.Parse(command.Data.Options.Where(x => x.Name == "id").Last().Value.ToString()));
        await task.CompleteTask();

        await command.FollowupAsync("update successful", ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
