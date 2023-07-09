using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

class StartTimerCommand : SlashCommand
{
    public StartTimerCommand()
    {
        _name = "start-work-timer";
        _description = "Starts a timer to log work hours";
        _ephemeral = true;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        WorkTimeTracker2 _tracker = new WorkTimeTracker2();
        await base.HandleCommand(command);

        _tracker.setPerson(await new DatabasePersonController().useDiscord(command.User.Username + "#" + command.User.Discriminator));

        if (_tracker.getStart() != _tracker.getEnd())
        {
            await new DatabaseWorkTimeTrackerController().PushToDatabase(_tracker);

            await command.FollowupAsync("Timer Started at " + DateTime.Now.ToShortTimeString(), ephemeral: Ephemeral);
        }
        else
        {
            await command.FollowupAsync("Timer already active, please cancel current timer before starting a new one", ephemeral: Ephemeral);
        }
        await command.DeleteOriginalResponseAsync();

    }
}
