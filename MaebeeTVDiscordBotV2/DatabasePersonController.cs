using Discord.WebSocket;
using Google.Apis.Auth.OAuth2;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase.Realtime.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class DatabasePersonController : SupabaseClient
{
    protected async Task<Person2> GetNonLocalData(supabasePerson person)
    {
        Person2 output = new Person2(person);
        if (person.PersonID != 0)
        {
            var request = await client
                .From<supabasePersonTeam>()
                .Select("*")
                .Where(x => x.PersonID == person.PersonID)
                .Get();

            foreach (var item in request.Models)
            {
                var thing = await client
                    .From<supabaseTeam>()
                    .Select("*")
                    .Where(x => x.TeamID == item.TeamID)
                    .Get();
                output.addTeam(new Team2(thing.Model));
            }

            var activity = await client
                .From<supabaseActivityReport>()
                .Select("*")
                .Where(x => x.Reported == person.PersonID)
                .Get();

            output.setActivityProductivityVibe(activity.Models);

            var request2 = await client.From<supabasePersonTask>()
                .Select("*")
                .Where(x => x.PersonID == person.PersonID)
                .Get();

            foreach (var item in request2.Models)
            {
                var thing = await client
                    .From<supabaseTask>()
                    .Select("*")
                    .Where(x => x.TaskID == item.TaskID)
                    .Get();
                output.addTask(new Task2(thing.Model));
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
    public async Task<bool> CheckPersonTaskExists(long TaskID, long PersonID)
    {
        var request = await client
            .From<supabasePersonTask>()
            .Select("*")
            .Where(x => x.PersonID == PersonID)
            .Where(x => x.TaskID == TaskID)
            .Get();
        return request.Model != null;
    }

    public async Task<Person2> useID(long ID)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == ID)
            .Get();
        return await GetNonLocalData(person.Model);
    }
    public async Task<Person2> useName(string Name)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonName == Name)
            .Get();
        return await GetNonLocalData(person.Model);
    }
    public async Task<Person2> useDiscord(string Discord)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == Discord)
            .Get();
        return await GetNonLocalData(person.Model);
    }
    public async Task<Person2> useDiscord(SocketUser Discord)
    {
        string discordPerson = Discord.Username + "#" + Discord.Discriminator;
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == discordPerson)
            .Get();
        return await GetNonLocalData(person.Model);
    }
    public async Task<Person2> useStrikeCount(short StrikeCount)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.StrikeCount == StrikeCount)
            .Get();
        return await GetNonLocalData(person.Model);
    }
    public async Task<Person2> useDescription(string Description)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Description == Description)
            .Get();
        return await GetNonLocalData(person.Model);
    }
    public async Task<Person2> useImage(string Image)
    {
        var person = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Image == Image)
            .Get();
        return await GetNonLocalData(person.Model);
    }


    public async Task PushToDatabase(Person2 person)
    {
        supabasePerson output = person.getSupabase();
        List<Team2> teams = person.getTeams();
        List<Task2> tasks = person.getTasks();
        if (output.PersonID == 0)
        {
            var response = await client
                .From<supabasePerson>()
                .Insert(output);
            output = response.Model;
            foreach (Team2 team in teams)
            {
                if (!await CheckPersonTeamExists(team.getID(), output.PersonID))
                {
                    supabasePersonTeam item = new supabasePersonTeam()
                    {
                        PersonID = output.PersonID,
                        TeamID = team.getID()
                    };
                    await client
                        .From<supabasePersonTeam>()
                        .Insert(item);
                }
            }
            foreach (Task2 task in tasks)
            {
                if (!await CheckPersonTaskExists(task.getID(), output.PersonID))
                {
                    supabasePersonTask item = new supabasePersonTask()
                    {
                        PersonID = output.PersonID,
                        TaskID = task.getID()
                    };
                    await client
                        .From<supabasePersonTask>()
                        .Insert(item);
                }
            }
        }
        else
        {
            await client.From<supabasePerson>()
                .Where(x => x.PersonID == output.PersonID)
                .Set(x => x.PersonName, output.PersonName)
                .Set(x => x.Discord, output.Discord)
                .Set(x => x.StrikeCount, output.StrikeCount)
                .Set(x => x.Description, output.Description)
                .Set(x => x.Image, output.Image)
                .Update();
        }
    }
}

[Table("Person")]
public class supabasePerson : BaseModel
{
    [PrimaryKey("PersonID")]
    public long PersonID { get; set; }

    [Column("PersonName")]
    public string PersonName { get; set; }

    [Column("Discord")]
    public string Discord { get; set; }

    [Column("StrikeCount")]
    public short StrikeCount { get; set; }

    [Column("Image")]
    public string Image { get; set; }

    [Column("Description")]
    public string Description { get; set; }
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