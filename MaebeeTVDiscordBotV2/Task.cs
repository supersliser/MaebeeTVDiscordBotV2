using Postgrest.Attributes;
using Postgrest.Models;
using Postgrest.Responses;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

class STask : SupabaseClient
{
    protected supabaseTask _supabaseTask;
    protected supabaseTeam _supabaseTeam;
    protected List<supabasePerson> _supabasePeople;

    public long ID
    {
        get
        {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (_supabaseTask.TaskID == null)
            {
                return 0;
            }
            else
            {
                return _supabaseTask.TaskID;
            }
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
        }
    }
    public string Name
    {
        get
        {
            if (_supabaseTask.TaskName == null)
            {
                return "N/A";
            }
            else
            {
                return _supabaseTask.TaskName;
            }
        }
    }
    public string Description
    {
        get
        {
            if (_supabaseTask.Description == null)
            {
                return "N/A";
            }
            else
            {
                return _supabaseTask.Description;
            }
        }
    }
    public long Team
    {
        get
        {
            if (_supabaseTeam == null || _supabaseTask.TeamID == 0)
            {
                return 16;
            }
            else
            {
                return _supabaseTask.TeamID;
            }
        }
    }
    public async Task<Team> TeamAsTeam()
    {
        var output = await client
            .From<supabaseTeam>()
            .Select("*")
            .Where(x => x.TeamID == Team)
            .Get();
        return new Team(output.Model);
    }
    public List<string> Resources
    {
        get
        {
            if (_supabaseTask.ResourceLocations == null)
            {
                return new List<string>() { "N/A" };
            }
            else
            {
                return _supabaseTask.ResourceLocations;
            }
        }
    }
    public string Output
    {
        get
        {
            if (_supabaseTask.OutputLocation == null)
            {
                return "N/A";
            }
            else
            {
                return _supabaseTask.OutputLocation;
            }
        }
    }
    public DateTime Created
    {
        get
        {
            if (_supabaseTask.DateCreated == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return _supabaseTask.DateCreated;
            }
        }
    }
    public DateTime Modified
    {
        get
        {
            if (_supabaseTask.DateLastModified == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return _supabaseTask.DateLastModified;
            }
        }
    }
    public bool Complete
    {
        get
        {
            return _supabaseTask.Completed;
        }
    }
    public DateTime Completed
    {
        get
        {
            if (_supabaseTask.DateCompleted == null)
            {
                return DateTime.MaxValue;
            }
            else
            {
                return _supabaseTask.DateCompleted;
            }
        }
    }
    public DateTime Due
    {
        get
        {
            if (_supabaseTask.DueDate == null)
            {
                return DateTime.MaxValue;
            }
            else
            {
                return _supabaseTask.DueDate;
            }
        }
    }

    public bool Exists
    {
        get
        {
            return _supabaseTask != null;
        }
    }

    public void SetTask(supabaseTask task)
    {
        _supabaseTask = task;
    }
    public async Task SetTask(string name, string desc, bool inDatabase, string team = null, string[] resources = null, string output = null, string due = null, string[] people = null)
    {
        if (_supabaseTask == null)
        {
            _supabaseTask = new supabaseTask();
        }
        _supabaseTask.TaskName = name;
        _supabaseTask.Description = desc;
        if (!inDatabase)
        {
            _supabaseTask.DateCreated = DateTime.Today;
        }
        _supabaseTask.DateLastModified = DateTime.Today;
        if (team != null)
        {
            var temp = await client
                .From<supabaseTeam>()
                .Select("*")
                .Where(x => x.TeamName == team)
                .Get();
            _supabaseTeam = temp.Model;
            _supabaseTask.TeamID = _supabaseTeam.TeamID;
        }
        if (resources != null)
        {
            _supabaseTask.ResourceLocations = resources.ToList();
        }
        else
        {
            _supabaseTask.ResourceLocations = new List<string>()
            {
                "N/A"
            }
            ;
        }
        if (output != null)
        {
            _supabaseTask.OutputLocation = output;
        }
        else
        {
            _supabaseTask.OutputLocation = "N/A";
        }
        if (due != null)
        {
            _supabaseTask.DueDate = DateTime.Parse(due);
        }
        else
        {
            _supabaseTask.DueDate = DateTime.MaxValue;
        }
        if (people != null)
        {
            _supabasePeople = new List<supabasePerson>();
            foreach (var person in people)
            {
                var temp = await client
                    .From<supabasePerson>()
                    .Select("*")
                    .Where(x => x.PersonName == person)
                    .Get();
                _supabasePeople.Add(temp.Model);
            }
        }
        else
        {
            var temp = new string[1];
            temp[0] = "N/A";
            _supabasePeople = null;
        }
    }

    public async Task<supabaseTask> GetFromDatabase(long ID)
    {
        var output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.TaskID == ID)
            .Get()
            ;
        _supabaseTask = output.Model;
        return _supabaseTask;
    }
    public async Task<supabaseTask> GetFromDatabase(string Name)
    {
        var output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.TaskName == Name)
            .Get()
            ;
        _supabaseTask = output.Models[0];
        return _supabaseTask;
    }
    public async Task<List<supabaseTask>> GetFromDatabaseForTeam(long TeamID)
    {
        if (_supabaseTask == null)
        {
            _supabaseTask = new supabaseTask();
        }
        _supabaseTask.TeamID = TeamID;
        var output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.TeamID == TeamID)
            .Get()
            ;
        return output.Models;
    }
    public async Task<List<supabaseTask>> GetFromDatabaseForCreated(DateTime date, string condition)
    {
        ModeledResponse<supabaseTask> output;
        output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCreated == date)
            .Get();
        switch (condition)
        {
            case "before":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateCreated <= date)
                    .Get();
                break;
            case "at":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateCreated == date)
                    .Get();
                break;
            case "after":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateCreated >= date)
                    .Get();
                break;
        }
        return output.Models;
    }
    public async Task<List<supabaseTask>> GetFromDatabaseForModified(DateTime date, string condition)
    {
        ModeledResponse<supabaseTask> output;
        output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateLastModified == date)
            .Get();
        switch (condition)
        {
            case "before":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateLastModified <= date)
                    .Get();
                break;
            case "at":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateLastModified == date)
                    .Get();
                break;
            case "after":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateLastModified >= date)
                    .Get();
                break;
        }
        return output.Models;
    }
    public async Task<List<supabaseTask>> GetFromDatabaseForCompleted(DateTime date, string condition)
    {
        ModeledResponse<supabaseTask> output;
        output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCompleted == date)
            .Get();
        switch (condition)
        {
            case "before":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateCompleted <= date)
                    .Get();
                break;
            case "at":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateCompleted == date)
                    .Get();
                break;
            case "after":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DateCompleted >= date)
                    .Get();
                break;
        }
        return output.Models;
    }
    public async Task<List<supabaseTask>> GetFromDatabaseForDue(DateTime date, string condition)
    {
        ModeledResponse<supabaseTask> output;
        output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DueDate == date)
            .Get();
        switch (condition)
        {
            case "before":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DueDate <= date)
                    .Get();
                break;
            case "at":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DueDate == date)
                    .Get();
                break;
            case "after":
                output = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.DueDate >= date)
                    .Get();
                break;
        }
        return output.Models;
    }
    public async Task<List<supabaseTask>> GetFromDatabase(bool completed)
    {
        var output = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.Completed == completed)
            .Get()
            ;
        return output.Models;
    }
    public async Task<List<supabaseTask>> GetTasksForPerson(string discord)
    {
        List<supabaseTask> output = new List<supabaseTask>();
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == discord)
            .Get()
            ;

        var personTeams = await client
            .From<supabasePersonTeam>()
            .Select("*")
            .Where(x => x.PersonID == person.Model.PersonID)
            .Get();

        foreach (supabasePersonTeam teamid in personTeams.Models)
        {
            var tasks = await GetFromDatabaseForTeam(teamid.TeamID);

            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    if (!task.Completed)
                    {
                        output.Add(task);
                    }
                }
            }
        }
        return output;
    }
    public async Task<List<Person>> GetPeopleForTaskFromDatabase()
    {
        var output = new List<Person>();

        var temp = await client
            .From<supabasePersonTask>()
            .Select("*")
            .Where(x => x.TaskID == ID)
            .Get();

        var ids = temp.Models;
        _supabasePeople = new List<supabasePerson>();

        foreach (var id in ids)
        {
            var thing = await client
                .From<supabasePerson>()
                .Select("*")
                .Where(x => x.PersonID == id.PersonID)
                .Get();
            output.Add(new Person(thing.Model));
            _supabasePeople.Add(thing.Model);
        }
        return output;
    }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<List<Person>> GetPeopleForTask()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        var output = new List<Person>();
        if (_supabasePeople != null)
        {
            foreach (supabasePerson person in _supabasePeople)
            {
                output.Add(new Person(person));
            }
        }
        return output;
    }

    public override async Task PushToDatabase()
    {
        if (ID == 0)
        {
            _supabaseTask.DateCreated = DateTime.Today;
            _supabaseTask.DateLastModified = DateTime.Today;
            _supabaseTask.TeamID = Team;
            var response = await client.From<supabaseTask>().Insert(_supabaseTask);
            _supabaseTask = response.Model;
        }
        else
        {
            _supabaseTask.DateLastModified = DateTime.Today;
            await client.From<supabaseTask>()
                .Where(x => x.TeamID == ID)
                .Set(x => x.TaskName, Name)
                .Set(x => x.TeamID, Team)
                .Set(x => x.ResourceLocations, Resources)
                .Set(x => x.OutputLocation, Output)
                .Set(x => x.DateCreated, Created)
                .Set(x => x.DateLastModified, Modified)
                .Set(x => x.Completed, Complete)
                .Set(x => x.DateCompleted, Completed)
                .Set(x => x.DueDate, Due)
                .Update();
        }
        if (_supabasePeople != null)
        {
            foreach (var person in _supabasePeople)
            {
                await client.From<supabasePersonTask>().Insert(new supabasePersonTask()
                {
                    PersonID = person.PersonID,
                    TaskID = ID
                });
            }
        }
    }
    public async Task CompleteTask()
    {
        _supabaseTask.Completed = true;
        _supabaseTask.DateCompleted = DateTime.Today;
        await client.From<supabaseTask>()
            .Where(x => x.TaskID == ID)
            .Set(x => x.Completed, true)
            .Set(x => x.DateCompleted, DateTime.Today)
            .Update();
    }
    public async Task AddPerson(long personID)
    {
        var thing = new supabasePersonTask()
        {
            TaskID = ID,
            PersonID = personID
        };
        await client.From<supabasePersonTask>().Insert(thing);
    }
    public STask()
    {

    }
    public STask(supabaseTask supabaseTask)
    {
        _supabaseTask = supabaseTask;
    }
}


[Table("Task")]
#pragma warning disable IDE1006 // Naming Styles
public class supabaseTask : BaseModel
#pragma warning restore IDE1006 // Naming Styles
{
    [PrimaryKey("TaskID")]
    public long TaskID { get; set; }

    [Column("Description")]
    public string Description { get; set; }

    [Column("TaskName")]
    public string TaskName { get; set; }

    [Column("Team")]
    public long TeamID { get; set; }

    [Column("ResourceLocations")]
    public List<string> ResourceLocations { get; set; }

    [Column("OutputLocation")]
    public string OutputLocation { get; set; }

    [Column("DateCreated")]
    public DateTime DateCreated { get; set; }

    [Column("DateLastModified")]
    public DateTime DateLastModified { get; set; }

    [Column("Completed")]
    public bool Completed { get; set; }

    [Column("DateCompleted")]
    public DateTime DateCompleted { get; set; }

    [Column("DueDate")]
    public DateTime DueDate { get; set; }
}