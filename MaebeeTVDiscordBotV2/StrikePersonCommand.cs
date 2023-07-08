using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StrikePersonCommand : SlashCommand
{
    ActivityReport _report;
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
        Person person = new Person();
        await person.GetFromDatabase((SocketUser)command.Data.Options.Where(x => x.Name == "person").First().Value);
        person.AddStrike();
        await person.PushToDatabase();
        await command.FollowupAsync("Strike Added");
        await command.DeleteOriginalResponseAsync();
    }
}
