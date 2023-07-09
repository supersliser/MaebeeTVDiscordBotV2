using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DisplayTasksCommand : SlashCommand
{
    public DisplayTasksCommand()
    {
        _name = "display-tasks";
        _description = "Displays all tasks available for you in the Database";
        _ephemeral = true;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);
        var tasks = (await new DatabasePersonController().useDiscord(command.User.Username + "#" + command.User.Discriminator)).getTasks();

        for (int i = 1; i < (tasks.Count / 2); i++)
        {
            var tempEmbe = new TaskEmbed();
            tempEmbe.SetupEmbed(tasks.GetRange(i, 2), false);
            embed.Add(tempEmbe);
        }
        var tempEmbed = new TaskEmbed();
        tempEmbed.SetupEmbed(tasks.GetRange(((tasks.Count / 2) * 2), tasks.Count % 2), false);
        embed.Add(tempEmbed);

        var embedOutput = new List<Embed>();
        foreach (var embed in embed)
        {
            embedOutput.Add(embed.Build());
        }

        await command.FollowupAsync(embeds: embedOutput.ToArray(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
