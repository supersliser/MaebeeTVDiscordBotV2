using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabaseOccuranceReportController : SupabaseClient
{
    protected async Task<OccuranceReport2> GetNonLocalData(supabaseOccuranceReport occuranceReport)
    {
        OccuranceReport2 output = new OccuranceReport2(occuranceReport);
        var request = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == occuranceReport.Reported)
            .Get();
        output.setReported(new Person2(request.Model));
        request = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == occuranceReport.Reporter)
            .Get();
        output.setReporter(new Person2(request.Model));
        return output;
    }

    public async Task<List<OccuranceReport2>> useBeforeTimestamp(DateTime timestamp)
    {
        var report = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Timestamp < timestamp)
            .Get();
        var output = new List<OccuranceReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<OccuranceReport2> useTimestamp(DateTime timestamp)
    {
        var report = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Timestamp == timestamp)
            .Get();
        return await GetNonLocalData(report.Model);
    }
    public async Task<List<OccuranceReport2>> useAfterTimestamp(DateTime timestamp)
    {
        var report = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Timestamp > timestamp)
            .Get();
        var output = new List<OccuranceReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<OccuranceReport2>> useReported(Person2 person)
    {
        var report = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Reported == person.getID())
            .Get();
        var output = new List<OccuranceReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<OccuranceReport2>> useReporter(Person2 person)
    {
        var report = await client
            .From<supabaseOccuranceReport>()
            .Select("*")
            .Where(x => x.Reporter == person.getID())
            .Get();
        var output = new List<OccuranceReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }

    public async Task PushToDatabase(OccuranceReport2 report)
    {
        supabaseOccuranceReport output = report.getSupabase();
        var response = await client
            .From<supabaseOccuranceReport>()
            .Insert(output);
    }
}
