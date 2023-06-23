using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StopTimerCommand : SlashCommand
{
    public StopTimerCommand()
    {
        _name = "stop-work-timer";
        _description = "Stops the timer to log work hours";
        _ephemeral = true;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        WorkTimeTracker _tracker = new WorkTimeTracker();
        await base.HandleCommand(command);

        await _tracker.SetPerson(command.User.Username + "#" + command.User.Discriminator);

        await _tracker.PushEndToDatabase();

        await command.FollowupAsync("Timer Stopped at " + DateTime.Now.ToShortTimeString(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
