using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ReportActivityCommand : SlashCommand
{
    ActivityReport _report;
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

        _report = new ActivityReport();
        await _report.SetResponse(((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value).Username + "#" + ((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value).Discriminator, command.User.Username + "#" + command.User.Discriminator);

        await command.RespondWithModalAsync(_modal.Build());
    }

    public async Task HandleAccept(SocketModal command)
    {
        await _report.SetResponse(short.Parse(command.Data.Components.Where(x => x.CustomId == "activity").First().Value), short.Parse(command.Data.Components.Where(x => x.CustomId == "productivity").First().Value), short.Parse(command.Data.Components.Where(x => x.CustomId == "vibe").First().Value));
        await _report.PushToDatabase();
        await command.RespondAsync("Database Updated", ephemeral: Ephemeral);

    }
}
