using Discord;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

class PersonEmbed : TEmbed
{
    protected Person2 _person;
    protected List<Task2> _tasks;
   
    public string TasksToText()
    {
        var tasks = _person.getTasks();
        string output = tasks[0].getTitle();
        for (int i = 1; i < tasks.Count; i++)
        {
            output += ", ";
            output += tasks[i].getTitle();
        }
        return output;
    }

    public void SetupEmbed(Person2 person, short activity, short productivity, short vibe)
    {
        _person = person;
        _title = person.getName();
        _description = "These are the details for " + person.getName();

        _fields = new List<Discord.EmbedFieldBuilder>
        {
            new EmbedFieldBuilder()
            {
                Name = "ID",
                Value = person.getID(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Name",
                Value = person.getName(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Description",
                Value = person.getDescription(),
                IsInline = false,
            },
            new EmbedFieldBuilder()
            {
                Name = "Discord",
                Value = person.getDiscord(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Activity",
                Value = activity + "%",
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Productivity",
                Value = productivity + "%",
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Vibe",
                Value = vibe + "%",
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Strikes",
                Value = person.getStrikeCount(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Teams",
                Value = person.getTeamNames(),
                IsInline = false,
            },
        };
        if (_tasks != null && _tasks.Count > 0)
        {
            Fields.Add(
                new EmbedFieldBuilder()
                {
                    Name = "Currently working on:",
                    Value = TasksToText(),
                    IsInline = true,
                });
        }
    }
    public void SetupEmbed(Person2 person, List<Team2> teams)
    {
        _person = person;
        _title = person.getName();
        _description = "These are the details for " + person.getName();
        _fields = new List<Discord.EmbedFieldBuilder>
        {
            new EmbedFieldBuilder()
            {
                Name = "ID",
                Value = person.getID(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Name",
                Value = person.getName(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Discord",
                Value = person.getDiscord(),
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Teams",
                Value = person.getTeamNames(),
                IsInline = false,
            },
        };
    }
}
