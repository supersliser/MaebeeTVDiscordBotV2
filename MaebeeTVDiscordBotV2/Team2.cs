using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

class Team2
{
    protected long ID;
    protected string Name;
    protected List<Person2> Members;

    public Team2()
    {

    }
    public Team2(long iD, string name, List<Person2> members)
    {
        ID = iD;
        Name = name;
        Members = members;
    }
    public Team2(supabaseTeam team)
    {
        setID(team.TeamID);
        setName(team.TeamName);
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
    public void setName(string Name)
    {
        this.Name = Name;
    }
    public void setSupabase(supabasePerson person)
    {
        setID(person.PersonID);
        setName(person.PersonName);
    }
    public void setMembers(List<Person2> members)
    {
        this.Members = members;
    }
    public void addMember(Person2 member)
    {
        if (Members == null)
        {
            Members = new List<Person2>();
        }
        Members.Add(member);
    }
    public void setMembers(List<supabasePerson> members)
    {
        this.Members = new List<Person2>();
        foreach(supabasePerson person in members)
        {
            addMember(new Person2(person));
        }
    }

    public long getID()
    {
        if (ID == 0)
        {
            return 16;
        }
        return ID;
    }
    public string getName()
    {
        if (Name == null)
        {
            return "N/A";
        }
        return Name;
    }
    public supabaseTeam getSupabase()
    {
        return new supabaseTeam()
        {
            TeamID = getID(),
            TeamName = getName(),
        };
    }
    public List<Person2> getMembers()
    {
        if ( Members == null)
        {
            Members = new List<Person2>();
        }
        return Members;
    }
}

[Table("Team")]
public class supabaseTeam : BaseModel
{
    [PrimaryKey("TeamID")]
    public long TeamID { get; set; }

    [Column("TeamName")]
    public string TeamName { get; set; }
}