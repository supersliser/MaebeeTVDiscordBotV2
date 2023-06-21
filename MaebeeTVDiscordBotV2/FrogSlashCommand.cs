using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FrogSlashCommand : SlashCommand
{
    public FrogSlashCommand()
    {
        _name = "war-crimes";
        _description = "do it, make your dreams come true";
        _ephemeral = false;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);
        await command.FollowupAsync("https://tenor.com/view/friend-frog-rain-sad-gif-24396867", ephemeral: _ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
