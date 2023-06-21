using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

class DisplayPersonCommand : SlashCommand
{
    Person _person;
    public DisplayPersonCommand()
    {
        _name = "display-person";
        _description = "Displays the details of a person";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "discord",
                "The discord of the person",
                true,
                Discord.ApplicationCommandOptionType.User)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);
        _person = new Person();
        await _person.GetFromDatabase((SocketUser)command.Data.Options.Where(x => x.Name == "discord").First().Value);

        var temp = new UserEmbed();
        await temp.SetupEmbed(_person, (short)await _person.GetActivity(), (short)await _person.GetProductivity(), (short)await _person.GetVibe());
        embed.Add(temp);

        await command.FollowupAsync(embed: embed.First().Build(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
