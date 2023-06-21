using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class ActivityReport : SupabaseClient
{
    protected supabaseActivityReport _supabaseFormResponse;
    protected supabasePerson _reporter;
    protected supabasePerson _reported;

    public DateTime Timestamp
    {
        get
        {
            if (_supabaseFormResponse.Timestamp == null)
            {
                return DateTime.Now;
            }
            else
            {
                return _supabaseFormResponse.Timestamp;
            }
        }
    }
    public long ReportedID
    {
        get
        {
            return _supabaseFormResponse.Reported;
        }
    }
    public short Activity
    {
        get
        {
            return _supabaseFormResponse.Activity;
        }
    }
    public short Productivity
    {
        get
        {
            return _supabaseFormResponse.Productivity;
        }
    }
    public short Vibe
    {
        get
        {
            return _supabaseFormResponse.Vibe;
        }
    }
    public long ReporterID
    {
        get
        {
            return _supabaseFormResponse.Reporter;
        }
    }

    public bool Exists
    {
        get
        {
            return _supabaseFormResponse != null;
        }
    }

    public async Task SetResponse(supabaseActivityReport response)
    {
        _supabaseFormResponse = response;
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == _supabaseFormResponse.Reported)
            .Get();
        _reported = temp.Model;
        temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == _supabaseFormResponse.Reporter)
            .Get();
        _reporter = temp.Model;
    }

    public async Task SetResponse(string ReportedDiscord, short Activity, short Productivity, short Vibe, string ReporterDiscord)
    {
        if (_supabaseFormResponse == null)
        {
            _supabaseFormResponse = new supabaseActivityReport();
        }
        _supabaseFormResponse.Timestamp = DateTime.Now;
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReportedDiscord)
            .Get();
        _reported = temp.Model;
        _supabaseFormResponse.Reported = _reported.PersonID;
        _supabaseFormResponse.Activity = Activity;
        _supabaseFormResponse.Productivity = Productivity;
        _supabaseFormResponse.Vibe = Vibe;
        temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReporterDiscord)
            .Get();
        _reporter = temp.Model;
        _supabaseFormResponse.Reporter = _reporter.PersonID;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task SetResponse(short Activity, short Productivity, short Vibe)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        if (_supabaseFormResponse == null)
        {
            _supabaseFormResponse = new supabaseActivityReport();
        }
        _supabaseFormResponse.Timestamp = DateTime.Now;
        _supabaseFormResponse.Activity = Activity;
        _supabaseFormResponse.Productivity = Productivity;
        _supabaseFormResponse.Vibe = Vibe;
    }

    public async Task SetResponse(string ReportedDiscord, string ReporterDiscord)
    {
        if (_supabaseFormResponse == null)
        {
            _supabaseFormResponse = new supabaseActivityReport();
        }
        _supabaseFormResponse.Timestamp = DateTime.Now;
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReportedDiscord)
            .Get();
        _reported = temp.Model;
        _supabaseFormResponse.Reported = _reported.PersonID;
        temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReporterDiscord)
            .Get();
        _reporter = temp.Model;
        _supabaseFormResponse.Reporter = _reporter.PersonID;
    }

    public async Task<supabaseActivityReport> GetFromDatabase(DateTime Timestamp)
    {
        var output = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Timestamp == Timestamp)
            .Get();
        _supabaseFormResponse = output.Model;
        return _supabaseFormResponse;
    }
    public async Task<List<supabaseActivityReport>> GetFromDatabaseForReported(string ReportedDiscord)
    {
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReportedDiscord)
            .Get()
            ;
        _reported = temp.Model;
        var output = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reported == _reported.PersonID)
            .Get()
            ;
        return output.Models;
    }
    public async Task<List<supabaseActivityReport>> GetFromDatabaseForReporter(string ReporterDiscord)
    {
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReporterDiscord)
            .Get()
            ;
        var output = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reporter == temp.Model.PersonID)
            .Get()
            ;
        return output.Models;
    }

    public supabasePerson GetReporter()
    {
        return _reporter;
    }
    public supabasePerson GetReported()
    {
        return _reported;
    }

    public override async Task PushToDatabase()
    {
        if (GetFromDatabase(Timestamp) != null)
        {
            var response = await client.From<supabaseActivityReport>().Insert(_supabaseFormResponse);
            _supabaseFormResponse = response.Model;
        }
        else
        {
            await client.From<supabaseActivityReport>()
                .Where(x => x.Timestamp == Timestamp)
                .Set(x => x.Reporter, ReporterID)
                .Set(x => x.Reported, ReportedID)
                .Set(x => x.Activity, Activity)
                .Set(x => x.Productivity, Productivity)
                .Set(x => x.Vibe, Vibe)
                .Update();
        }
    }
}

[Table("ActivityReport")]
#pragma warning disable IDE1006 // Naming Styles
public class supabaseActivityReport : BaseModel
#pragma warning restore IDE1006 // Naming Styles
{
    [PrimaryKey("Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("Reported")]
    public long Reported { get; set; }

    [Column("Activity")]
    public short Activity { get; set; }

    [Column("Productivity")]
    public short Productivity { get; set; }

    [Column("Reporter")]
    public long Reporter { get; set; }

    [Column("Vibe")]
    public short Vibe { get; set; }
}