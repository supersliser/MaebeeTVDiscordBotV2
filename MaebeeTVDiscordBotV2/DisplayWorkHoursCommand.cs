using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DisplayWorkHoursCommand : SlashCommand
{
    public DisplayWorkHoursCommand()
    {
        _name = "display-work-hours";
        _description = "Displays all work hours that have been logged to the database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "person",
                "the discord of the person whos hours you would like to see",
                true,
                Discord.ApplicationCommandOptionType.User)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);

        var thing = await new DatabaseWorkTimeTrackerController().useBeforeStart(DateTime.Now);
        var output = new WorkTimeTracker2();
        output.setPerson(await new DatabasePersonController().useDiscord((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value));
        embed = new List<TEmbed>();
        var outputItems = new List<WorkAmountForMonth>();
        WorkAmountForMonth ignore;
        ignore.hours = 0;
        ignore.monthYear = DateTime.MinValue;
        for (int i = 0; i < thing.Count; i++)
        {
            if (thing[i].getStart() < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
            {
                if (thing[i].getStart().Month > ignore.monthYear.Month)
                {
                    outputItems.Add(ignore);
                    ignore.hours = 0;
                    ignore.monthYear = new DateTime(thing[i].getStart().Year, thing[i].getStart().Month, 1);
                }
                if (thing[i].getStart().Day < thing[i].getEnd().Day)
                {
                    ignore.hours += ((thing[i].getEnd().Hour - thing[i].getStart().Hour) - 12) * -1;
                }
                else
                {
                    ignore.hours += thing[i].getEnd().Hour - thing[i].getStart().Hour;
                }
            }
        }
        outputItems.Add(ignore);

        for (int i = 1; i <= (outputItems.Count / 25); i++)
        {
            var embedItem = new WorkHoursEmbed();
            embedItem.SetupEmbed(outputItems.GetRange(i, 25), output.getPerson().getName());
            embed.Add(embedItem);
        };
        var thingout = new WorkHoursEmbed();
        thingout.SetupEmbed(outputItems.GetRange(((outputItems.Count / 25) * 25), outputItems.Count % 25), output.getPerson().getName());
        embed.Add(thingout);

        var embedOutput = new List<Embed>();
        foreach (var embed in embed)
        {
            embedOutput.Add(embed.Build());
        }

        await command.FollowupAsync(embeds: embedOutput.ToArray(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
