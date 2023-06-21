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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public virtual async Task SetupEmbed() { }
    public virtual async Task SetupEmbed(Person person) { }
    public virtual async Task SetupEmbed(List<Team> teams) { }
    public virtual async Task SetupEmbed(Team team) { }
    public virtual async Task SetupEmbed(List<STask> tasks, bool acceptedTasks) { }
    public virtual async Task SetupEmbed(STask task) { }
    public virtual async Task SetupEmbed(List<ActivityReport> reports) { }
    public virtual async Task SetupEmbed(List<OccuranceReport> reports) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
