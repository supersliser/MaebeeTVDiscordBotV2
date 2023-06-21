using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CancelButton : TButton
{
    public CancelButton(Func<SocketMessageComponent, Task> onClick, string commandName)
    {
        _onClick = onClick;
        _title = "Cancel";
        _id = commandName + "_CancelButton";
        _style = Discord.ButtonStyle.Danger;
    }
}
