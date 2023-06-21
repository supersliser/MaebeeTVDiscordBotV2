using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DisplayTeamsCommand : SlashCommand
{
    List<Team> _teams;
    public DisplayTeamsCommand()
    {
        _name = "display-teams";
        _description = "Displays all teams in the Database";
        _ephemeral = false;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);
        var temp = await new Team().GetAllFromDatabase();
        _teams = new List<Team>();
        foreach (var team in temp)
        {
            _teams.Add(new Team());
            _teams.Last().SetTeam(team);
        }

        for (int i = 0; i <= (_teams.Count / 8); i++)
        {
            embed.Add(new TeamEmbed());
            await embed.Last().SetupEmbed(_teams.GetRange(i, 8));
        };
        await embed.Last().SetupEmbed(_teams.GetRange(((_teams.Count / 8) * 8), _teams.Count % 8));

        var embedOutput = new List<Embed>();
        foreach(var embed in embed)
        {
            embedOutput.Add(embed.Build());
        }
        
        await command.FollowupAsync(embeds: embedOutput.ToArray(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
