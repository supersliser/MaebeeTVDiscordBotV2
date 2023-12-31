﻿using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

class AddTeamCommand : SlashCommand
{
    Team2 _team;
    public AddTeamCommand()
    {
        _name = "add-team";
        _description = "Adds a team to the Database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "name",
                "The name of the team",
                true,
                Discord.ApplicationCommandOptionType.String)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);
        _team = new Team2();

        _team.setName(command.Data.Options.Where(x => x.Name == "name").First().Value.ToString());
        var temp = new TeamEmbed();
        temp.SetupEmbed(_team);

        _buttons = new List<TButton>()
        {
            new AcceptButton(HandleAccept, _name),
            new CancelButton(HandleCancel, _name)
        };

        await command.FollowupAsync(embed: embed[0].Build(), ephemeral: Ephemeral, components: GetButtons());
        await command.DeleteOriginalResponseAsync();
    }

    public async Task HandleAccept(SocketMessageComponent command)
    {
        await new DatabaseTeamController().PushToDatabase(_team);
        await command.RespondAsync("Database Updated", ephemeral: Ephemeral);
    }
    public async Task HandleCancel(SocketMessageComponent command)
    {
        _team = null;
        await command.RespondAsync("Datebase Update Cancelled", ephemeral: Ephemeral);

    }
}
