using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

public class TButton
{
    protected string _title;
    protected string _id;
    protected ButtonStyle _style;
    protected Func<SocketMessageComponent, Task> _onClick;

    public string Title
    {
        get
        {
            return _title;
        }
    }
    public string Id
    {
        get
        {
            return _id;
        }
    }
    public ButtonStyle Style
    {
        get
        {
            return _style;
        }
    }
    public Func<SocketMessageComponent, Task> Click
    {
        get
        {
            return _onClick;
        }
    }

    public TButton()
    {

    }
    public TButton(string title, string id, ButtonStyle style)
    {
        _title = title;
        _id = id;
        _style = style;
    }
    public TButton(string title, string id, ButtonStyle style, Func<SocketMessageComponent, Task> onClick)
    {
        _title = title;
        _id = id;
        _style = style;
        _onClick = onClick;
    }
    public TButton(Func<SocketMessageComponent, Task> onClick, string commandName)
    {
        _onClick = onClick;
        _id = commandName + "_Button";
    }
    
    public ButtonComponent Build()
    {
        return new ButtonBuilder(_title, _id, _style).Build();
    }

    public ButtonBuilder ButtonBuilder()
    {
       return new ButtonBuilder(_title, _id, _style);
    }
}
