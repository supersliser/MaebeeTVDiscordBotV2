using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ReportActivityCommand : SlashCommand
{
    ActivityReport2 _report;
    public ReportActivityCommand()
    {
        _name = "report-activity";
        _description = "Adds an activity report to the database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "person",
                "The person who you would like to report",
                true,
                Discord.ApplicationCommandOptionType.User)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        _modal = new ReportActivityModal(HandleAccept, _name);

        _report = new ActivityReport2();
        _report.setReported(await new DatabasePersonController().useDiscord((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value));
        _report.setReporter(await new DatabasePersonController().useDiscord(command.User));

        await command.RespondWithModalAsync(_modal.Build());
    }

    public async Task HandleAccept(SocketModal command)
    {
        _report.setActivity(short.Parse(command.Data.Components.Where(x => x.CustomId == "activity").First().Value.ToString()));
        _report.setProductivity(short.Parse(command.Data.Components.Where(x => x.CustomId == "productivity").First().Value.ToString()));
        _report.setVibe(short.Parse(command.Data.Components.Where(x => x.CustomId == "vibe").First().Value.ToString()));
        await new DatabaseActivityReportController().PushToDatabase(_report);
        await command.RespondAsync("Database Updated", ephemeral: Ephemeral);
    }
}
