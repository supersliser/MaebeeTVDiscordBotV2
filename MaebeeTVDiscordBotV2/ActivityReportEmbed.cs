using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ActivityReportEmbed : TEmbed
{
    protected List<ActivityReport> _reports;

    public override async Task SetupEmbed(List<ActivityReport> reports)
    {
        _reports = reports;
        _title = "Activity reports";
        _description = "These are the activity reports that meet the given criteria";
        _fields = new List<Discord.EmbedFieldBuilder>();
        foreach (var report in _reports)
        {
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Timestamp",
                Value = report.Timestamp,
                IsInline = false
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Reporter",
                Value = report.GetReporter().PersonName,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Reported",
                Value = report.GetReported().PersonName,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Activity",
                Value = report.Activity,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Productivity",
                Value = report.Productivity,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Vibe",
                Value = report.Vibe,
                IsInline = true
            });
        }
    }
}
