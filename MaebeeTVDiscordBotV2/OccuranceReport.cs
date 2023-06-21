using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class OccuranceReport : SupabaseClient
{
    protected supabaseOccuranceReport _supabaseOccuranceReport;
    protected supabasePerson _reporter;
    protected supabasePerson _reported;

    public DateTime Timestamp
    {
        get
        {
            if (_supabaseOccuranceReport.Timestamp == null)
            {
                return DateTime.Now;
            }
            else
            {
                return _supabaseOccuranceReport.Timestamp;
            }
        }
    }
    public long ReportedID
    {
        get
        {
            return _supabaseOccuranceReport.Reported;
        }
    }
    public long ReporterID
    {
        get
        {
            return _supabaseOccuranceReport.Reporter;
        }
    }
    public string Description
    {
        get
        {
            if (_supabaseOccuranceReport.Description == null)
            {
                return "N/A";
            }
            else
            {
                return _supabaseOccuranceReport.Description;
            }
        }
    }

    public bool Exists
    {
        get
        {
            return _supabaseOccuranceReport != null;
        }
    }

    public async Task SetResponse(supabaseOccuranceReport response)
    {
        _supabaseOccuranceReport = response;
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == _supabaseOccuranceReport.Reported)
            .Get();
        _reported = temp.Model;
        temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == _supabaseOccuranceReport.Reporter)
            .Get();
        _reporter = temp.Model;
    }

    public async Task SetResponse(string ReportedDiscord, string Description, string ReporterDiscord)
    {
        _supabaseOccuranceReport.Timestamp = DateTime.Now;
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReportedDiscord)
            .Get();
        _reported = temp.Model;
        _supabaseOccuranceReport.Reported = _reported.PersonID;
        _supabaseOccuranceReport.Description = Description;
        temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReporterDiscord)
            .Get();
        _reporter = temp.Model;
        _supabaseOccuranceReport.Reporter = _reporter.PersonID;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task SetResponse(string description)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        if (_supabaseOccuranceReport == null)
        {
            _supabaseOccuranceReport = new supabaseOccuranceReport();
        }
        _supabaseOccuranceReport.Description = description;
        _supabaseOccuranceReport.Timestamp = DateTime.Now;
    }

    public async Task SetResponse(string ReportedDiscord, string ReporterDiscord)
    {
        if (_supabaseOccuranceReport == null)
        {
            _supabaseOccuranceReport = new supabaseOccuranceReport();
        }
        _supabaseOccuranceReport.Timestamp = DateTime.Now;
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReportedDiscord)
            .Get();
        _reported = temp.Model;
        _supabaseOccuranceReport.Reported = _reported.PersonID;
        temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReporterDiscord)
            .Get();
        _reporter = temp.Model;
        _supabaseOccuranceReport.Reporter = _reporter.PersonID;
    }

    public async Task<supabaseOccuranceReport> GetFromDatabase(DateTime Timestamp)
    {
        var output = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Timestamp == Timestamp)
            .Get();
        _supabaseOccuranceReport = output.Model;
        return _supabaseOccuranceReport;
    }
    public async Task<List<supabaseOccuranceReport>> GetFromDatabaseForReported(string ReportedDiscord)
    {
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReportedDiscord)
            .Get()
            ;
        _reported = temp.Model;
        var output = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Reported == _reported.PersonID)
            .Get()
            ;
        return output.Models;
    }
    public async Task<List<supabaseOccuranceReport>> GetFromDatabaseForReporter(string ReporterDiscord)
    {
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == ReporterDiscord)
            .Get()
            ;
        var output = await client
            .From<supabaseOccuranceReport>()
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
            var response = await client.From<supabaseOccuranceReport>().Insert(_supabaseOccuranceReport);
            _supabaseOccuranceReport = response.Model;
        }
        else
        {
            await client.From<supabaseOccuranceReport>()
                .Where(x => x.Timestamp == Timestamp)
                .Set(x => x.Reporter, ReporterID)
                .Set(x => x.Reported, ReportedID)
                .Set(x => x.Description, Description)
                .Update();
        }
    }
}

[Table("OccuranceReport")]
#pragma warning disable IDE1006 // Naming Styles
public class supabaseOccuranceReport : BaseModel
#pragma warning restore IDE1006 // Naming Styles
{
    [PrimaryKey("Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("Description")]
    public string Description { get; set; }

    [Column("Reporter")]
    public long Reporter { get; set; }

    [Column("Reported")]
    public long Reported { get; set; }
}