using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabaseActivityReportController : SupabaseClient
{
    protected async Task<ActivityReport2> GetNonLocalData(supabaseActivityReport activityReport)
    {
        ActivityReport2 output = new ActivityReport2(activityReport);
        var request = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == activityReport.Reported)
            .Get();
        output.setReported(new Person2(request.Model));
        request = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == activityReport.Reporter)
            .Get();
        output.setReporter(new Person2(request.Model));
        return output;
    }

    public async Task<List<ActivityReport2>> useBeforeTimestamp(DateTime timestamp)
    {
        var report = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Timestamp < timestamp)
            .Get();
        var output = new List<ActivityReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<ActivityReport2> useTimestamp(DateTime timestamp)
    {
        var report = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Timestamp == timestamp)
            .Get();
        return await GetNonLocalData(report.Model);
    }
    public async Task<List<ActivityReport2>> useAfterTimestamp(DateTime timestamp)
    {
        var report = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Timestamp > timestamp)
            .Get();
        var output = new List<ActivityReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<ActivityReport2>> useReported(Person2 person)
    {
        var report = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reported == person.getID())
            .Get();
        var output = new List<ActivityReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<ActivityReport2>> useReporter(Person2 person)
    {
        var report = await client
            .From<supabaseActivityReport>()
            .Select("*")
            .Where(x => x.Reporter == person.getID())
            .Get();
        var output = new List<ActivityReport2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }


    public async Task PushToDatabase(ActivityReport2 report)
    {
        supabaseActivityReport output = report.getSupabase();
        var response = await client
            .From<supabaseActivityReport>()
            .Insert(output);
    }
}
