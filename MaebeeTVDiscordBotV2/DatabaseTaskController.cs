using Postgrest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabaseTaskController : SupabaseClient
{
    protected async Task<Task2> GetNonLocalData(supabaseTask task)
    {
        Task2 output = new Task2(task);
        if (task.TaskID == 0)
        {
            var request = await client
                .From<supabasePersonTask>()
                .Select("*")
                .Where(x => x.TaskID == task.TaskID)
                .Get();
            
            foreach (var item in request.Models)
            {
                var thing = await client
                    .From<supabasePerson>()
                    .Select("*")
                    .Where(x => x.PersonID == item.PersonID)
                    .Get();
                output.addAssignee(new Person2(thing.Model));
            }
        }
        return output;
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
    public async Task<Task2> useID(long ID)
    {
        var task = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.TaskID == ID)
            .Get();
        return await GetNonLocalData(task.Model);
    }
    public async Task<Task2> useTitle(string Title)
    {
        var task = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.TaskName == Title)
            .Get();
        return await GetNonLocalData(task.Model);
    }
    public async Task<Task2> useDescription(string Description)
    {
        var task = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.Description == Description)
            .Get();
        return await GetNonLocalData(task.Model);
    }
    public async Task<List<Task2>> useTeam(Team2 team)
    {
        var task = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.TeamID == team.getID())
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask item in task.Models)
        {
            output.Add(await GetNonLocalData(item));
        }
        return output;
    }
    public async Task<List<Task2>> useBeforeCreated(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCreated < date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }    
    public async Task<List<Task2>> useCreated(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCreated == date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }    
    public async Task<List<Task2>> useAfterCreated(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCreated > date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useBeforeUpdated(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateLastModified < date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }    
    public async Task<List<Task2>> useUpdated(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateLastModified == date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }    
    public async Task<List<Task2>> useAfterUpdated(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateLastModified > date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useCompleted(bool completed)
    {
        var tasks = await client
    .From<supabaseTask>()
    .Select("*")
    .Where(x => x.Completed == completed)
    .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useBeforeCompleted(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCompleted < date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useCompleted(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCompleted == date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useAfterCompleted(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DateCompleted > date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useBeforeDue(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DueDate < date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useDue(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DueDate == date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useAfterDue(DateTime date)
    {
        var tasks = await client
            .From<supabaseTask>()
            .Select("*")
            .Where(x => x.DueDate > date)
            .Get();
        var output = new List<Task2>();
        foreach (supabaseTask task in tasks.Models)
        {
            output.Add(await GetNonLocalData(task));
        }
        return output;
    }
    public async Task<List<Task2>> useAssignees(Person2 person)
    {
        var temp = await client
            .From<supabasePersonTask>()
            .Select("*")
            .Where(x => x.PersonID == person.getID())
            .Get();
        var output = new List<Task2>();
        foreach (supabasePersonTask thing in temp.Models)
        {
            var task = await client
                .From<supabaseTask>()
                .Select("*")
                .Where(x => x.TaskID == thing.TaskID)
                .Get();
            output.Add(new Task2(task.Model));
        }
        return output;
    }
}
