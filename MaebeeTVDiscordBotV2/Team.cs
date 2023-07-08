using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static PersonOLD;
using static Postgrest.Constants;

class TeamOLD : SupabaseClient
{
    private supabaseTeam _supabaseTeam;

    public long ID
    {
        get
        {
            return _supabaseTeam.TeamID;
        }
    }
    public string Name
    {
        get
        {
            return _supabaseTeam.TeamName;
        }
    }

    public async Task<supabaseTeam> GetFromDatabase(long ID)
    {
        var person = await client
            .From<supabaseTeam>()
            .Select("*")
            .Where(x => x.TeamID == ID)
            .Get()
            ;
            _supabaseTeam = person.Model;
        return _supabaseTeam;
    }
    public async Task<supabaseTeam> GetFromDatabase(string Name)
    {
        var person = await client
            .From<supabaseTeam>()
            .Select("*")
            .Where(x => x.TeamName == Name)
            .Get()
            ;
            _supabaseTeam = person.Model;
        return _supabaseTeam;
    }
    public async Task<List<supabaseTeam>> GetAllFromDatabase()
    {
        var output = await client
            .From<supabaseTeam>()
            .Select("*")
            .Order(x => x.TeamID, Ordering.Ascending)
            .Get()
            ;
        return output.Models;
    }
    public async Task<List<Person>> GetPeopleFromTeam()
    {
        var output = new List<Person>();
        var ids = await client
            .From<supabasePersonTeam>()
            .Select("*")
            .Where(x => x.TeamID == ID)
            .Get()
            ;
        foreach (var id in ids.Models)
        {
            var temp = await client
                .From<supabasePerson>()
                .Select("*")
                .Where(x => x.PersonID == id.PersonID)
                .Get()
                ;
            output.Add(new Person(temp.Model));
        }
        return output;
    }

    public void SetTeam(supabaseTeam person)
    {
        _supabaseTeam = person;
    }

    public override async Task PushToDatabase()
    {
        if (GetFromDatabase(ID) != null)
        {
            var response = await client.From<supabaseTeam>().Insert(_supabaseTeam);
            _supabaseTeam = response.Model;
        }
        else
        {
            await client.From<supabaseTeam>()
                .Where(x => x.TeamID == ID)
                .Set(x => x.TeamName, Name)
                .Update();
        }
    }

    public Team()
    {

    }
    public Team(string Name)
    {
        _supabaseTeam = new supabaseTeam()
        {
            TeamName = Name
        };
    }
    public Team(supabaseTeam supabaseTeam)
    {
        _supabaseTeam = supabaseTeam;
    }
}

