using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TaskEmbed : TEmbed
{
    protected List<STask> _tasks;

    public override async Task SetupEmbed(STask task)
    {
        _tasks = new List<STask>()
        {
            task
        };
        _title = "Check details for " + task.Name;
        _description = "Check the details for this task before commiting them to the database";
        _fields = new List<Discord.EmbedFieldBuilder>();
        string people;
        try
        {
            var temp = await task.GetPeopleForTask();
            people = temp[0].Name;
            for (int i = 1; i < temp.Count; i++)
            {
                people += ", ";
                people += temp[i].Name;
            }
        }
        catch
        {
            people = "N/A";
        }
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "ID",
            Value = task.ID,
            IsInline = false
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Title",
            Value = task.Name,
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Description",
            Value = task.Description,
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Team",
            Value = task.Team,
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
            Value = ListToString(task.Resources),
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Output",
            Value = task.Output,
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Date Created",
            Value = task.Created,
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Date Modified",
            Value = task.Modified,
            IsInline = true
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Due",
            Value = task.Due,
            IsInline = true
        });
    }
    public override async Task SetupEmbed(List<STask> tasks, bool acceptedTasks)
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
        foreach (STask task in tasks)
        {
            string people;
            try
            {
                var temp = await task.GetPeopleForTask();
                people = temp[0].Name;
                for (int i = 1; i < temp.Count; i++)
                {
                    people += ", ";
                    people += temp[i].Name;
                }
            }
            catch
            {
                people = "N/A";
            }
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "ID",
                Value = task.ID,
                IsInline = false
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Title",
                Value = task.Name,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Description",
                Value = task.Description,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Team",
                Value = task.Team,
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
                Value = ListToString(task.Resources),
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Output",
                Value = task.Output,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Date Created",
                Value = task.Created,
                IsInline = true
            });
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Date Modified",
                Value = task.Modified,
                IsInline = true
            });           
            _fields.Add(new Discord.EmbedFieldBuilder()
            {
                Name = "Due",
                Value = task.Due,
                IsInline = true
            });

        }
    }
}
