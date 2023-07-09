using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WorkHoursEmbed : TEmbed
{

    public void SetupEmbed(List<WorkAmountForMonth> workAmounts, string personName)
    {
        _title = "Work Amount for " + personName;
        _description = "These are the work hours for this person per month";
        _fields = new List<Discord.EmbedFieldBuilder>();
        var temp = new Stack<WorkAmountForMonth>();
        foreach (var workAmount in workAmounts)
        {
            temp.Push(workAmount);
        }
        foreach (var item in temp)
        {
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = item.monthYear.Month + "/" + item.monthYear.Year,
                Value = item.hours,
                IsInline = false
            });
        }
    }
}
