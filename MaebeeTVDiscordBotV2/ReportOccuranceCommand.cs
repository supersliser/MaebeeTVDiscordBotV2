using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ReportOccuranceCommand : SlashCommand
{
    OccuranceReport2 _report;
    public ReportOccuranceCommand()
    {
        _name = "report-occurance";
        _description = "Adds an occurance report to the database";
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
        _modal = new ReportOccuranceModal(HandleAccept, _name);

        _report = new OccuranceReport2();
        _report.setReported(await new DatabasePersonController().useDiscord((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value));
        _report.setReporter(await new DatabasePersonController().useDiscord(command.User));

        await command.RespondWithModalAsync(_modal.Build());
    }

    public async Task HandleAccept(SocketModal command)
    {
        _report.setDescription(command.Data.Components.Where(x => x.CustomId == "description").First().Value);
        await new DatabaseOccuranceReportController().PushToDatabase(_report);
        await command.RespondAsync("Database Updated", ephemeral: Ephemeral);
    }
}
