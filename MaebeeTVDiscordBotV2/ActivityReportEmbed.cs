using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ActivityReportEmbed : TEmbed
{

    public void SetupEmbed(List<ActivityReport2> reports)
    {
        _title = "Activity reports";
        _description = "These are the activity reports that meet the given criteria";
        _fields = new List<Discord.EmbedFieldBuilder>();
        foreach (var report in reports)
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
                Name = "Activity",
                Value = report.getActivity(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Productivity",
                Value = report.getProductivity(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Vibe",
                Value = report.getVibe(),
                IsInline = true
            });
        }
    }
}
