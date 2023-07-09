using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StrikePersonCommand : SlashCommand
{
    ActivityReport2 _report;
    public StrikePersonCommand()
    {
        _name = "strike-person";
        _description = "gives a person a strike";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "person",
                "The person who you would like to report",
                true,
                Discord.ApplicationCommandOptionType.User)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);
        Person2 person = await new DatabasePersonController().useDiscord((SocketUser)command.Data.Options.Where(x => x.Name == "person").First().Value);
        person.incrementStrikeCount();
        await new DatabasePersonController().PushToDatabase(person);
        await command.FollowupAsync("Strike Added", ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
