﻿using Discord.WebSocket;
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
        WorkTimeTracker _tracker = new WorkTimeTracker();
        await base.HandleCommand(command);

        await _tracker.SetPerson(command.User.Username + "#" + command.User.Discriminator);

        await _tracker.PushStartToDatabase();

        await command.FollowupAsync("Timer Started at" + DateTime.Now.ToShortTimeString());
        await command.DeleteOriginalResponseAsync();
    }
}