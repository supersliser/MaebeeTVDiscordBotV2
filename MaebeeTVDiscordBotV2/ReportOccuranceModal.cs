using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ReportOccuranceModal : TModal
{
    public ReportOccuranceModal(Func<SocketModal, Task> onAccept, string CommandName)
    {
        _title = "Occurance Report";
        _id = CommandName + "_OccuranceModal";
        _onAccept = onAccept;
        _inputs = new List<Discord.TextInputBuilder>()
        {
            new Discord.TextInputBuilder()
            {
                Label = "Description",
                CustomId = "description",
                Style = Discord.TextInputStyle.Paragraph,
                Placeholder = "Description",
                MinLength = 0,
                Required = true,
            }
        };
    }
}
