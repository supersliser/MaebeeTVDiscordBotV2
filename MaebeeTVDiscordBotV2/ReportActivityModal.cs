using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ReportActivityModal : TModal
{
    public ReportActivityModal(Func<SocketModal, Task> onAccept, string CommandName)
    {
        _title = "Activity Report";
        _id = CommandName + "_ActivityModal";
        _onAccept = onAccept;
        _inputs = new List<Discord.TextInputBuilder>()
        {
            new Discord.TextInputBuilder()
            {
                Label = "Activity",
                CustomId = "activity",
                Style = Discord.TextInputStyle.Short,
                Placeholder = "1-10",
                MinLength = 1,
                MaxLength = 2,
                Required = true,
            },
            new Discord.TextInputBuilder()
            {
                Label = "Productivity",
                CustomId = "productivity",
                Style = Discord.TextInputStyle.Short,
                Placeholder = "1-10",
                MinLength = 1,
                MaxLength = 2,
                Required = true,
            },
            new Discord.TextInputBuilder()
            {
                Label = "Vibe",
                CustomId = "vibe",
                Style = Discord.TextInputStyle.Short,
                Placeholder = "1-10",
                MinLength = 1,
                MaxLength = 2,
                Required = true,
            },
        };
    }
}
