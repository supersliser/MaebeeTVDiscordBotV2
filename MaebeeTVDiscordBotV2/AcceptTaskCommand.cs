using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

class AcceptTaskCommand : SlashCommand
{
    public AcceptTaskCommand()
    {
        _name = "accept-task";
        _description = "Adds a task to your list of currently in progress";
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

        var person = new Person2((await new DatabasePersonController().useDiscord(command.User)).getSupabase());
        var task = await new DatabaseTaskController().useID(long.Parse(command.Data.Options.Where(x => x.Name == "id").Last().Value.ToString()));
        task.addAssignee(person);

        await command.FollowupAsync("update successful", ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}