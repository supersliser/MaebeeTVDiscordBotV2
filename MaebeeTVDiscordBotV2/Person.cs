using Discord.WebSocket;
using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PersonOLD;


class PersonOLD 
{
    private supabasePerson _supabasePerson;
    private List<Team> _supabaseTeams;

    public string Name
    {
        get
        {
            if (_supabasePerson != null)
            {
                return _supabasePerson.PersonName;
            }
            else { return "unnamed_person"; }
        }
    }
    public long ID
    {
        get
        {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (_supabasePerson.PersonID != null)
            {
                return _supabasePerson.PersonID;
            }
            else { return 0; }
        }
    }
    public string Discord
    {
        get
        {
            if (_supabasePerson.Discord != null)
            {
                return _supabasePerson.Discord;
            }
            else { return "N/A"; }
        }
    }
    public short StrikeCount
    {
        get
        {
            if (_supabasePerson.StrikeCount != null)
            {
                return _supabasePerson.StrikeCount;
            }
            else { return 0; }
        }
    }

    public bool PersonExists
    {
        get
        {
            try
            {
                return GetFromDatabase(ID) != null;
            }
            catch
            {
                return false;
            }
        }
    }

#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
    public async Task<supabasePerson> GetFromDatabase(string Name)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonName == Name)
            .Get()
            ;
        _supabasePerson = person.Model;
        return _supabasePerson;
    }
    public async Task<supabasePerson> GetFromDatabase(SocketUser Discord)
    {
        string temp = Discord.Username + "#" + Discord.Discriminator;
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == temp)
            .Get()
            ;
        _supabasePerson = person.Model;
        return _supabasePerson;
    }
    public async Task<supabasePerson> GetFromDatabase(long ID)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == ID)
            .Get()
            ;
        _supabasePerson = person.Model;
        await GetTeamsFromDatabase();
        return _supabasePerson;
    }

    public async Task<List<Team>> GetTeamsFromDatabase()
    {
        var request = await client
            .From<supabasePersonTeam>()
            .Select("*")
            .Where(x => x.PersonID == ID)
            .Get();

        if (_supabaseTeams == null)
        {
            _supabaseTeams = new List<Team>();
        }
        foreach (var item in request.Models)
        {
            _supabaseTeams.Add(new Team());
            await _supabaseTeams.Last().GetFromDatabase(item.TeamID);
        }

        return _supabaseTeams;
    }
    public async Task<List<Team>> GetTeamsFromDatabase(string[] names)
    {
        _supabaseTeams = new List<Team>();
        foreach (var item in names)
        {
            _supabaseTeams.Add(new Team());
            await _supabaseTeams.Last().GetFromDatabase(item);
        }
        return _supabaseTeams;
    }

    public void SetPerson(supabasePerson person)
    {
        _supabasePerson = person;
    }
    public async Task SetPerson(string name, string discord, string[] teams, long strikes = 0)
    {
        _supabasePerson = new supabasePerson()
        {
            PersonName = name,
            Discord = discord,
            StrikeCount = short.Parse(strikes.ToString())
        };
        if (teams.Length > 0)
        {
            await GetTeamsFromDatabase(teams);
        }
    }
    public List<Team> GetTeams()
    {
        return _supabaseTeams;
    }
    public void UpdateDiscord(string discord)
    {
        _supabasePerson.Discord = discord;
    }

    public async Task<int> GetActivity()
    {
        var temp = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reported == ID)
            .Get();
        float output = 0;
        foreach (var report in temp.Models)
        {
            output += report.Activity;
        }
        output /= temp.Models.Count;
        output /= 10;
        return (int)(output * 100);
    }
    public async Task<int> GetProductivity()
    {
        var temp = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reported == ID)
            .Get();
        float output = 0;
        foreach (var report in temp.Models)
        {
            output += report.Productivity;
        }
        output /= temp.Models.Count;
        output /= 10;
        return (int)(output * 100);
    }
    public async Task<int> GetVibe()
    {
        var temp = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reported == ID)
            .Get();
        float output = 0;
        foreach (var report in temp.Models)
        {
            output += report.Vibe;
        }
        output /= temp.Models.Count;
        output /= 10;
        return (int)(output * 100);
    }

    public async Task<bool> CheckPersonTeamExists(long TeamID)
    {
        var request = await client.From<supabasePersonTeam>()
            .Select("*")
            .Where(x => x.PersonID == ID)
            .Where(x => x.TeamID == TeamID)
            .Get();
        return request.Model != null;
    }

    public override async Task PushToDatabase()
    {
        if (ID == 0)
        {
            _supabasePerson.PersonName = Name;
            _supabasePerson.Discord = Discord;
            _supabasePerson.StrikeCount = StrikeCount;
            var response = await client.From<supabasePerson>().Insert(_supabasePerson);
            _supabasePerson = response.Model;
            foreach (Team team in _supabaseTeams)
            {
                if (team.GetFromDatabase(team.ID) == null)
                {
                    await team.PushToDatabase();
                }
                if (!await CheckPersonTeamExists(team.ID))
                {
                    supabasePersonTeam item = new supabasePersonTeam()
                    {
                        PersonID = ID,
                        TeamID = team.ID
                    };
                    await client.From<supabasePersonTeam>().Insert(item);
                }
            }
        }
        else
        {
            await client.From<supabasePerson>()
                .Where(x => x.PersonID == ID)
                .Set(x => x.PersonName, Name)
                .Set(x => x.Discord, Discord)
                .Set(x => x.StrikeCount, StrikeCount)
                .Update();
        }
    }

    public void AddStrike()
    {
        _supabasePerson.StrikeCount++;
    }

    public Person()
    {

    }
    public Person(supabasePerson supabasePerson)
    {
        _supabasePerson = supabasePerson;
    }
}



[Table("PersonTeam")]
#pragma warning disable IDE1006 // Naming Styles
public class supabasePersonTeam : BaseModel
#pragma warning restore IDE1006 // Naming Styles
{
    [PrimaryKey("ID")]
    public long ID { get; set; }

    [Column("PersonID")]
    public long PersonID { get; set; }

    [Column("TeamID")]
    public long TeamID { get; set; }
}
[Table("PersonTask")]
#pragma warning disable IDE1006 // Naming Styles
public class supabasePersonTask : BaseModel
#pragma warning restore IDE1006 // Naming Styles
{
    [PrimaryKey("ID")]
    public long ID { get; set; }

    [Column("PersonID")]
    public long PersonID { get; set; }

    [Column("TaskID")]
    public long TaskID { get; set; }
}