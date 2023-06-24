using Discord;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

class UserEmbed : TEmbed
{
    protected Person _person;
    protected List<STask> _tasks;

    public string Name
    {
        get
        {
            return _person.Name;
        }
    }
    public long ID
    {
        get
        {
            return _person.ID;
        }
    }
    public string Discord
    {
        get
        {
            return _person.Discord; 
        }
    }
    public short StrikeCount
    {
        get
        {
            return _person.StrikeCount;
        }
    }

    public async Task<List<Team>> GetTeams()
    {
        return await _person.GetTeamsFromDatabase();
    }    
    public async Task<List<STask>> GetTasks()
    {
        var temp = new STask();
        _tasks = new List<STask>();
        await temp.GetTasksForPerson(Discord);
        _tasks.Add(temp);
        return _tasks;
    }
    public async Task<string> TeamsToText()
    {
        var teams = await GetTeams();
        string output = teams[0].Name;
        for (int i = 1; i < teams.Count; i++)
        {
            output += ", ";
            output += teams[i].Name;
        }
        return output;
    }    
    public async Task<string> TasksToText()
    {
        var tasks = await GetTasks();
        string output = tasks[0].Name;
        for (int i = 1; i < tasks.Count; i++)
        {
            output += ", ";
            output += tasks[i].Name;
        }
        return output;
    }

    public async Task SetupEmbed(Person person, short activity, short productivity, short vibe)
    {
        _person = person;
        _title = Name;
        _description = "These are the details for " + Name;

        _fields = new List<Discord.EmbedFieldBuilder>
        {
            new EmbedFieldBuilder()
            {
                Name = "ID",
                Value = ID,
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Name",
                Value = Name,
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Discord",
                Value = Discord,
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
                Value = StrikeCount,
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Teams",
                Value = await TeamsToText(),
                IsInline = false,
            },
        };
        if (_tasks != null && _tasks.Count > 0)
        {
            Fields.Add(
                new EmbedFieldBuilder()
                {
                    Name = "Currently working on:",
                    Value = await TasksToText(),
                    IsInline = true,
                });
        }
    }
    public async Task SetupEmbed(Person person, List<Team> teams)
    {
        string text;
        text = teams[0].Name;
        for (int i = 1; i < teams.Count; i++)
        {
            text += ", ";
            text += teams[i].Name;
        }
        _person = person;
        _title = Name;
        _description = "These are the details for " + Name;
        _fields = new List<Discord.EmbedFieldBuilder>
        {
            new EmbedFieldBuilder()
            {
                Name = "ID",
                Value = ID,
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Name",
                Value = Name,
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Discord",
                Value = Discord,
                IsInline = true,
            },
            new EmbedFieldBuilder()
            {
                Name = "Teams",
                Value = text,
                IsInline = false,
            },
        };
    }
}
