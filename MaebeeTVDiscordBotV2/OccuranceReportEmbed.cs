using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class OccuranceReportEmbed : TEmbed
{
    protected List<OccuranceReport> _reports;

    public override async Task SetupEmbed(List<OccuranceReport> reports)
    {
        _reports = reports;
        _title = "Occurance reports";
        _description = "These are the occurance reports that meed the given criteria";
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
                Name = "Description",
                Value = report.Description,
                IsInline = false
            });
        }
    }
}
