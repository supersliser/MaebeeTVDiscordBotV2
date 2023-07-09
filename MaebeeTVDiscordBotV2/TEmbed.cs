using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TEmbed
{
    protected string _title;
    protected string _description;
    protected EmbedAuthorBuilder _author = new EmbedAuthorBuilder() { Name = "MaebeeTVBot" };
    protected Discord.Color _color = 0xff9dd0;
    protected List<EmbedFieldBuilder> _fields;

    public string Title
    { get { return _title; } }
    public string Description
    { get { return _description; } }
    public EmbedAuthorBuilder Author
    { get { return _author; } }
    public Discord.Color Color
    { get { return _color; } }
    public List<EmbedFieldBuilder> Fields
    { get { return _fields; } }

    public TEmbed()
    {

    }

    public string ListToString(List<string> items)
    {
        string output;
        output = items[0];
        for (int i = 1; i < items.Count; i++)
        {
            output += ", ";
            output += items[i];
        }

        return output;
    }
    public Embed Build()
    {
        var output = new EmbedBuilder()
        {
            Title = _title,
            Description = _description,
            Author = _author,
            Color = _color,
        };
        foreach (var field in _fields)
        {
          output.AddField(field);
        }
        return output.Build();
    }
}
