using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class OccuranceReportEmbed : TEmbed
{
    protected List<OccuranceReport2> _reports;

    public void SetupEmbed(List<OccuranceReport2> reports)
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
                Value = report.getTimestamp(),
                IsInline = false
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Reporter",
                Value = report.getReporter().getName(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Reported",
                Value = report.getReported().getName(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Description",
                Value = report.getDescription(),
                IsInline = false
            });
        }
    }
}
