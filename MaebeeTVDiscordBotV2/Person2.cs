using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

class Person2
{
    protected long ID;
    protected string Name;
    protected string Discord;
    protected short StrikeCount;
    protected string Image;
    protected string Description;
    protected short Activity;
    protected short Productivity;
    protected short Vibe;
    protected List<Team2> Teams = new List<Team2>();
    
    public Person2()
    {

    }
    public Person2(long iD, string name, string discord, short strikeCount, string image, string description, short activity, short productivity, short vibe, List<Team2> teams)
    {
        ID = iD;
        Name = name;
        Discord = discord;
        StrikeCount = strikeCount;
        Image = image;
        Description = description;
        Activity = activity;
        Productivity = productivity;
        Vibe = vibe;
        Teams = teams;
    }
    public Person2(supabasePerson person)
    {
        setID(person.PersonID);
        setName(person.PersonName);
        setDiscord(person.Discord);
        setImage(person.Image);
        setDescription(person.Description);
        setStrikeCount(person.StrikeCount);
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
    public void setDiscord(string Discord)
    {
        this.Discord = Discord;
    }
    public void setDiscord(SocketUser Discord)
    {
        this.Discord = Discord.Username + "#" + Discord.Discriminator;
    }
    public void setStrikeCount(short StrikeCount)
    {
        this.StrikeCount = StrikeCount;
    }
    public void setStrikeCount(int StrikeCount)
    {
        this.StrikeCount = short.Parse(StrikeCount.ToString());
    }
    public void setStrikeCount(long StrikeCount)
    {
        this.StrikeCount = short.Parse(StrikeCount.ToString());
    }
    public void incrementStrikeCount()
    {
        this.StrikeCount++;
    }
    public void setImage(string Image)
    {
        this.Image = Image;
    }
    public void setDescription(string Description)
    { 
        this.Description = Description; 
    }
    public void setActivityProductivityVibe(List<supabaseActivityReport> reports)
    {
        float activity = 0;
        float productivity = 0;
        float vibe = 0;

        foreach (supabaseActivityReport report in reports)
        {
            activity += report.Activity;
            productivity += report.Productivity;
            vibe += report.Vibe;
        }
        activity /= reports.Count();
        productivity /= reports.Count();
        vibe /= reports.Count();
        activity /= 10;
        productivity /= 10;
        vibe /= 10;

        setActivity((int)(activity * 100));
        setProductivity((int)(productivity * 100));
        setVibe((int)(vibe * 100));
    }
    public void setActivity(int Activity)
    {
        this.Activity = short.Parse(Activity.ToString());
    }
    public void setProductivity(int Productivity)
    {
        this.Productivity = short.Parse(Productivity.ToString());
    }
    public void setVibe(int Vibe)
    {
        this.Vibe = short.Parse(Vibe.ToString());
    }
    public void setTeams(List<Team2> Teams)
    {
        this.Teams = Teams;
    }
    public void addTeam(Team2 Team)
    {
        Teams.Add(Team);
    }

    public long getID()
    {
        return ID;
    }
    public string getName()
    {
        return Name;
    }
    public string getDiscord()
    {
        return Discord;
    }
    public short getStrikeCount()
    {
        return StrikeCount;
    }
    public string getImage()
    {
        return Image;
    }
    public string getDescription()
    {
        return Description;
    }
    public short getActivity()
    {
        return Activity;
    }
    public short getProductivity()
    {
        return Productivity;
    }
    public short getVibe()
    {
        return Vibe;
    }
    public List<Team2> getTeams()
    {
        return Teams;
    }
    public List<string> getTeamNames()
    {
        List<string> output = new List<string>();
        foreach (Team2 Team in Teams)
        {
            output.Add(Team.getName());
        }
        return output;
    }
}
