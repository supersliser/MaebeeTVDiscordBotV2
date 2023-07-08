using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

class Task2
{
    protected long ID;
    protected string Title;
    protected string Description;
    protected Team2 Team;
    protected List<string> ResourceLocations;
    protected string OutputLocation;
    protected DateTime DateCreated;
    protected DateTime DateUpdated;
    protected bool Completed;
    protected DateTime DateCompleted;
    protected DateTime DateDue;
    protected List<Person2> Assignees;


    public Task2()
    {

    }
    public Task2(long iD, string title, string description, Team2 team, List<string> resourceLocations, string outputLocations, DateTime dateCreated, DateTime dateUpdated, bool completed, DateTime dateCompleted, DateTime dateDue, List<Person2> assignees)
    {
        ID = iD;
        Title = title;
        Description = description;
        Team = team;
        ResourceLocations = resourceLocations;
        OutputLocation = outputLocations;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
        Completed = completed;
        DateCompleted = dateCompleted;
        DateDue = dateDue;
        Assignees = assignees;
    }
    public Task2(supabaseTask task)
    {
        setID(task.TaskID);
        setTitle(task.TaskName);
        setDescription(task.Description);
        setTeam(new DatabaseTeamController().useID(task.TeamID).Result);
        setResources(task.ResourceLocations);
        setOutput(task.OutputLocation);
        setCreated(task.DateCreated);
        setUpdated(task.DateLastModified);
        setCompleted(task.Completed);
        setCompletedTime(task.DateCompleted);
        setDue(task.DueDate);
    }

    public void setID(long ID)
    {
        this.ID = ID;
    }
    public void setID(int ID)
    {
        this.ID = ID;
    }
    public void setID(short ID)
    {
        this.ID = ID;
    }
    public void setTitle(string Title)
    {
        this.Title = Title;
    }
    public void setDescription(string Description)
    {
        this.Description = Description;
    }
    public void setTeam(Team2 Team)
    {
        this.Team = Team;
    }
    public void setResources(List<string> ResourceLocations)
    {
        this.ResourceLocations = ResourceLocations;
    }
    public void setResources(string input)
    {
        string[] temp = new string[0];
        temp[0] = ", ";
        ResourceLocations = input.Split(temp, StringSplitOptions.None).ToList();
    }
    public void addResources(string ResourceLocation)
    {
        if (this.ResourceLocations == null)
        {
            this.ResourceLocations = new List<string>();
        }
        this.ResourceLocations.Add(ResourceLocation);
    }
    public void setOutput(string OutputLocation)
    {
        this.OutputLocation = OutputLocation;
    }
    public void setCreated(DateTime created)
    {
        this.DateCreated = created;
    }
    public void setCreated(string created)
    {
        this.DateCreated = DateTime.Parse(created);
    }
    public void setCreated()
    {
        this.DateCreated = DateTime.Now;
    }
    public void setUpdated(DateTime updated)
    {
        this.DateUpdated = updated;
    }
    public void setUpdated(string updated)
    {
        this.DateUpdated = DateTime.Parse(updated);
    }
    public void setUpdated()
    {
        this.DateUpdated = DateTime.Now;
    }
    public void setCompleted(bool completed)
    {
        this.Completed = completed;
    }
    public void setCompleted()
    {
        this.Completed = !this.Completed;
    }
    public void setCompletedTime(DateTime completed)
    {
        this.DateCompleted = completed;
    }
    public void setCompletedTime(string completed)
    {
        this.DateCompleted = DateTime.Parse(completed);
    }
    public void setCompletedTime()
    {
        this.DateCompleted = DateTime.Now;
    }
    public void setDue(DateTime due)
    {
        this.DateDue = due;
    }
    public void setDue(string due)
    {
        this.DateDue = DateTime.Parse(due);
    }
    public void setDue()
    {
        this.DateDue = DateTime.Now;
    }
    public void setSupabase(supabaseTask task)
    {
        setID(task.TaskID);
        setTitle(task.TaskName);
        setDescription(task.Description);
        setTeam(new DatabaseTeamController().useID(task.TeamID).Result);
        setResources(task.ResourceLocations);
        setOutput(task.OutputLocation);
        setCreated(task.DateCreated);
        setUpdated(task.DateLastModified);
        setCompleted(task.Completed);
        setCompletedTime(task.DateCompleted);
        setDue(task.DueDate);
    }
    public void setAssignees(List<Person2> people)
    {
        this.Assignees = people;
    }
    public void addAssignee(Person2 person)
    {
        if (this.Assignees == null)
        {
            Assignees = new List<Person2>();
        }
        Assignees.Add(person);
    }

    public long getID()
    {
        return this.ID;
    }
    public string getTitle()
    {
        return this.Title;
    }
    public string getDescription()
    {
        return this.Description;
    }
    public Team2 getTeam()
    {
        return this.Team;
    }
    public List<string> getResources()
    {
        return this.ResourceLocations;
    }
    public string getResourcesAsOne()
    {
        if (this.ResourceLocations == null)
        {
            return "N/A";
        }
        string output = ResourceLocations[0];
        for (int i = 1; i < ResourceLocations.Count; i++)
        {
            output += ", ";
            output += ResourceLocations[i].ToString();
        }
        return output;
    }
    public string getOutput()
    {
        return OutputLocation;
    }
    public DateTime getCreated()
    {
        return DateCreated;
    }
    public DateTime getUpdated()
    {
        return DateUpdated;
    }
    public bool getCompleted()
    {
        return Completed;
    }
    public DateTime getCompletedTime()
    {
        return DateCompleted;
    }
    public DateTime getDue()
    {
        return DateDue;
    }
    public List<Person2> getPeople()
    {
        return Assignees;
    }
}

[Table("Task")]
public class supabaseTask : BaseModel
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