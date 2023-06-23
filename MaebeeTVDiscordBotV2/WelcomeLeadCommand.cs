using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WelcomeLeadCommand : SlashCommand
{
    public WelcomeLeadCommand()
    {
        _name = "initiate-lead-hello";
        _description = "how you doin";
        _ephemeral = false;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await command.RespondAsync("Getting even funkier");

        embed = new List<TEmbed>
        {
            new WelcomeLeadEmbed()
        };

        await embed.Last().SetupEmbed();
        await command.FollowupAsync(embed: embed.Last().Build(), ephemeral: _ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
