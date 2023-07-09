using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

class DatabaseWorkTimeTrackerController : SupabaseClient
{
    protected async Task<WorkTimeTracker2> GetNonLocalData(supabaseWorkTracker report)
    {
        WorkTimeTracker2 output = new WorkTimeTracker2(report);
        var request = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.PersonID == report.Person)
            .Get();
        output.setPerson(new Person2(request.Model));
        return output;
    }

    public async Task<List<WorkTimeTracker2>> useBeforeStart(DateTime start)
    {
        var report = await client
        .From<supabaseWorkTracker>()
        .Select("*")
        .Where(x => x.Start < start)
        .Get();
        var output = new List<WorkTimeTracker2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }    
    public async Task<WorkTimeTracker2> useStart(DateTime start)
    {
        var report = await client
        .From<supabaseWorkTracker>()
        .Select("*")
        .Where(x => x.Start == start)
        .Get();
        return new WorkTimeTracker2(report.Model);
    }    
    public async Task<List<WorkTimeTracker2>> useAfterStart(DateTime start)
    {
        var report = await client
        .From<supabaseWorkTracker>()
        .Select("*")
        .Where(x => x.Start > start)
        .Get();
        var output = new List<WorkTimeTracker2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<WorkTimeTracker2>> useBeforeEnd(DateTime end)
    {
        var report = await client
        .From<supabaseWorkTracker>()
        .Select("*")
        .Where(x => x.End < end)
        .Get();
        var output = new List<WorkTimeTracker2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }    
    public async Task<WorkTimeTracker2> useEnd(DateTime end)
    {
        var report = await client
        .From<supabaseWorkTracker>()
        .Select("*")
        .Where(x => x.End == end)
        .Get();
        return new WorkTimeTracker2(report.Model);
    }    
    public async Task<List<WorkTimeTracker2>> useAfterEnd(DateTime end)
    {
        var report = await client
        .From<supabaseWorkTracker>()
        .Select("*")
        .Where(x => x.End > end)
        .Get();
        var output = new List<WorkTimeTracker2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<WorkTimeTracker2>> usePerson(Person2 person)
    {
        var report = await client
            .From<supabaseWorkTracker>()
            .Select("*")
            .Where(x => x.Person == person.getID())
            .Get();
        var output = new List<WorkTimeTracker2>();
        foreach (var item in report.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }

    public async Task PushToDatabase(WorkTimeTracker2 tracker)
    {
        var test = await client
            .From<supabaseWorkTracker>()
            .Select("*")
            .Where(x => x.Start == tracker.getStart())
            .Get();
        if (test.Model == null)
        {
            var item = await client
                .From<supabaseWorkTracker>()
                .Insert(tracker.getSupabase());
        }
        else
        {
            var item = await client
                .From<supabaseWorkTracker>()
                .Where(x => x.Start == tracker.getStart())
                .Set(x => x.Person, tracker.getPerson().getID())
                .Set(x => x.End, tracker.getEnd())
                .Update();
        }
    }
}
