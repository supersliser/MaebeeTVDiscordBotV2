using Discord.WebSocket;
using Google.Apis.Auth.OAuth2;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase.Realtime.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabasePersonController : SupabaseClient
{
    protected async Task<Person2> GetNonLocalData(supabasePerson person)
    {
        Person2 output = new Person2(person);
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
        return output;
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
}

[Table("Person")]
#pragma warning disable IDE1006 // Naming Styles
public class supabasePerson : BaseModel
#pragma warning restore IDE1006 // Naming Styles
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