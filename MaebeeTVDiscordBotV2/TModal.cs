using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TModal
{
    protected string _title;
    protected string _id;
    protected List<TextInputBuilder> _inputs;
    protected Func<SocketModal, Task> _onAccept;

    public string Title
    {
        get
        {
            return _title;
        }
    }
    public string ID
    {
        get
        {
            return _id;
        }
    }
    public List<TextInputBuilder> Inputs
    {
        get
        {
            return _inputs;
        }
    }
    public Func<SocketModal, Task> Accept
    {
        get
        {
            return _onAccept;
        }
    }

    public TModal(Func<SocketModal, Task> onAccept, string commandName)
    {
        _onAccept = onAccept;
        _id = commandName + "_Modal";
    }

    public TModal()
    {

    }
    public TModal(string title, string id, List<TextInputBuilder> inputs)
    {
        _title = title;
        _id = id;
        _inputs = inputs;
    }
    public Modal Build()
    {
        ModalComponentBuilder builder = new ModalComponentBuilder();
        foreach (var input in _inputs)
        {
            builder.WithTextInput(input);
        }
        return new ModalBuilder(_title, _id, builder).Build();
    }
}
