using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Team2
{
    protected long ID;
    protected string Name;

    public Team2()
    {

    }
    public Team2(long iD, string name)
    {
        ID = iD;
        Name = name;
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

    public long getID()
    {
        return ID;
    }
    public string getName()
    {
        return Name;
    }
}
