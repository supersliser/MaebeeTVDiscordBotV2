using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

class DisplayTeamsCommand : SlashCommand
{
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
        var teams = await new DatabaseTeamController().all();

        for (int i = 0; i <= (teams.Count / 8); i++)
        {
            var tempEmbe = new TeamEmbed();
            tempEmbe.SetupEmbed(teams.GetRange(i, 8));
            embed.Add(tempEmbe);
        };
        var tempEmbed = new TeamEmbed();
        tempEmbed.SetupEmbed(teams.GetRange(((teams.Count / 8) * 8), teams.Count % 8));
        embed.Add(tempEmbed);

        var embedOutput = new List<Embed>();
        foreach(var embed in embed)
        {
            embedOutput.Add(embed.Build());
        }
        
        await command.FollowupAsync(embeds: embedOutput.ToArray(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
