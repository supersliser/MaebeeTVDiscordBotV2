using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class AcceptButton : TButton
{
    public AcceptButton(Func<SocketMessageComponent, Task> onClick, string commandName)
    {
        _onClick = onClick;
        _title = "Accept";
        _id = commandName + "_AcceptButton";
        _style = Discord.ButtonStyle.Success;
    }
}
