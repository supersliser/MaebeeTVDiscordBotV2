using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TaskEmbed : TEmbed
{
    protected List<Task2> _tasks;

    public void SetupEmbed(Task2 task)
    {
        _tasks = new List<Task2>()
        {
            task
        };
        _title = "Check details for " + task.getTitle();
        _description = "Check the details for this task before commiting them to the database";
        _fields = new List<Discord.EmbedFieldBuilder>();
        string people;
        try
        {
            people = task.getAssigneesAsString();
        }
        catch
        {
            people = "N/A";
        }
        if (people == "")
        {
            people = "N/A";
        }
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "ID",
            Value = task.getID(),
            IsInline = false
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Title",
            Value = task.getTitle(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Description",
            Value = task.getDescription(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Team",
            Value = task.getTeam(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "People",
            Value = people,
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Resources",
            Value = task.getResourcesAsOne(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Output",
            Value = task.getOutput(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Date Created",
            Value = task.getCreated(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Date Modified",
            Value = task.getUpdated(),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Due",
            Value = task.getDue(),
            IsInline = true
        });
    }
    public void SetupEmbed(List<Task2> tasks, bool acceptedTasks)
    {
        _tasks = tasks;
        _title = "Tasks";
        if (acceptedTasks)
        {
            _description = "These are your current tasks";
        }
        else
        {
            _description = "These tasks are available to you";
        }
        //convert team
        _fields = new List<Discord.EmbedFieldBuilder>();
        foreach (Task2 task in tasks)
        {
            string people;
            try
            {
                people = task.getAssigneesAsString();
            }
            catch
            {
                people = "N/A";
            }
            if (people == "")
            {
                people = "N/A";
            }
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "ID",
                Value = task.getID(),
                IsInline = false
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Title",
                Value = task.getTitle(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Description",
                Value = task.getDescription(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Team",
                Value = task.getTeam(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "People",
                Value = people,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Resources",
                Value = task.getResourcesAsOne(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Output",
                Value = task.getOutput(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Date Created",
                Value = task.getCreated(),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Date Modified",
                Value = task.getUpdated(),
                IsInline = true
            });           
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Due",
                Value = task.getDue(),
                IsInline = true
            });

        }
    }
}
