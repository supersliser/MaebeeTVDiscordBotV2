using Postgrest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabaseTeamController : SupabaseClient
{
    protected async Task<Team2> GetNonLocalData
        (supabaseTeam team)
    {
        Team2 output = new Team2(team);
        if (team.TeamID != 0)
        {
            var request = await client
                .From<supabasePersonTeam>()
                .Select("*")
                .Where(x => x.TeamID == team.TeamID)
                .Get();

            foreach (var item in request.Models)
            {
                var thing = await client
                    .From<supabasePerson>()
                    .Select("*")
                    .Where(x => x.PersonID == item.PersonID)
                    .Get();
                output.addMember(new Person2(thing.Model));
            }
        }
        return output;
    }
    public async Task<bool> CheckPersonTeamExists(long TeamID, long PersonID)
    {
        var request = await client.From<supabasePersonTeam>()
            .Select("*")
            .Where(x => x.PersonID == PersonID)
            .Where(x => x.TeamID == TeamID)
            .Get();
        return request.Model != null;
    }

    public async Task<Team2> useID(long ID)
    {
        var team = await client
            .From<supabaseTeam>()
            .Select("*")
            .Where(x => x.TeamID == ID)
            .Get();
        return await GetNonLocalData(team.Model);
    }
    public async Task<Team2> useName(string Name)
    {
        var team = await client
            .From<supabaseTeam>()
            .Select("*")
            .Where(x => x.TeamName == Name)
            .Get();
        return await GetNonLocalData(team.Model);
    }

    public async Task<List<Team2>> all()
    {
        var teams = await client
            .From<supabaseTeam>()
            .Select("*")
            .Get();
        var output = new List<Team2>();
        foreach (supabaseTeam team in teams.Models)
        {
            output.Add(await GetNonLocalData(team));
        }
        return output;
    }

    public async Task PushToDatabase(Team2 team)
    {
        supabaseTeam output = team.getSupabase();
        List<Person2> people = team.getMembers();
        if (output.TeamID == 0)
        {
            var response = await client
                .From<supabaseTeam>()
                .Insert(output);
            output = response.Model;
            foreach (Person2 person in people)
            {
                if (!await CheckPersonTeamExists(output.TeamID, person.getID()))
                {
                    supabasePersonTeam item = new supabasePersonTeam()
                    {
                        PersonID = person.getID(),
                        TeamID = output.TeamID,
                    };
                    await client
                        .From<supabasePersonTeam>()
                        .Insert(item);
                }
            }
        }
        else
        {
            await client.From<supabaseTeam>()
                .Where(x => x.TeamID == output.TeamID)
                .Set(x => x.TeamName, output.TeamName)
                .Update();
        }
    }
}
