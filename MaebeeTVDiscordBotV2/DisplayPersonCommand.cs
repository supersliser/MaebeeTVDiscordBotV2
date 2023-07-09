using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

class DisplayPersonCommand : SlashCommand
{
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
        Person2 _person = await new DatabasePersonController().useDiscord((SocketUser)command.Data.Options.Where(x => x.Name == "discord").First().Value);

        var temp = new PersonEmbed();
        temp.SetupEmbed(_person, _person.getActivity(), _person.getProductivity(), _person.getVibe());
        embed.Add(temp);

        await command.FollowupAsync(embed: embed.First().Build(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
