using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DescriptionModal : TModal
{
    public DescriptionModal(Func<SocketModal, Task> onAccept, string CommandName)
    {
        _title = "Description";
        _id = CommandName + "_DescriptionModal";
        _onAccept = onAccept;
        _inputs = new List<Discord.TextInputBuilder>
        {
            new Discord.TextInputBuilder()
            {
                Label = "Description",
                CustomId = "description",
                Style = Discord.TextInputStyle.Paragraph,
                Placeholder = "Description",
                MinLength = 20,
                Required = true,
            }
        };
    }
}
