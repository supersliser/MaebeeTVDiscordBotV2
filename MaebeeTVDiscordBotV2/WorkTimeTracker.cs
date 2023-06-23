using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

class WorkTimeTracker : SupabaseClient
{
    private supabaseWorkTracker _supabaseWorkTracker;

    public long ID
    {
        get
        {
            return _supabaseWorkTracker.Person;
        }
    }

    public async Task SetPerson(string discord)
    {
        if (_supabaseWorkTracker == null)
        {
            _supabaseWorkTracker = new supabaseWorkTracker();
        }
        var temp = await client
            .From<supabasePerson>()
            .Select("*")
            .Where(x => x.Discord == discord)
            .Get();
        _supabaseWorkTracker.Person = temp.Model.PersonID;
    }

    public async Task PushStartToDatabase()
    {
        _supabaseWorkTracker.Start = DateTime.Now;
        await client.From<supabaseWorkTracker>().Insert(_supabaseWorkTracker);
    }
    public async Task PushEndToDatabase()
    {
        await client.From<supabaseWorkTracker>()
            .Where(x => x.Person == _supabaseWorkTracker.Person)
            .Set(x => x.End, DateTime.Now)
            .Update();
    }

    public async Task<List<WorkAmountForMonth>> GetListPreviousTimes()
    {
        var output = new List<WorkAmountForMonth>();
        var temp = await client
            .From<supabaseWorkTracker>()
            .Select("*")
            .Where(x => x.Person == _supabaseWorkTracker.Person)
            .Order("Start", Postgrest.Constants.Ordering.Ascending)
            .Get();
        WorkAmountForMonth ignore;
        ignore.hours = 0;
        ignore.monthYear = DateTime.MinValue;
        for (int i = 0; i < temp.Models.Count; i++)
        {
            if (temp.Models[i].Start < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
            {
                if (temp.Models[i].Start.Month > ignore.monthYear.Month)
                {
                    output.Add(ignore);
                    ignore.hours = 0;
                    ignore.monthYear = new DateTime(temp.Models[i].Start.Year, temp.Models[i].Start.Month, 1);
                }
                if (temp.Models[i].Start.Day < temp.Models[i].End.Day)
                {
                    ignore.hours += ((temp.Models[i].End.Hour - temp.Models[i].Start.Hour) - 12) * -1;
                }
                else
                {
                    ignore.hours += temp.Models[i].End.Hour - temp.Models[i].Start.Hour;
                }
            }
        }
        output.Add(ignore);
        return output;
    }

public async Task<WorkAmountForMonth> GetMonthTimes()
{
    WorkAmountForMonth output;
        var temp = await client
            .From<supabaseWorkTracker>()
            .Select("*")
            .Where(x => x.Person == _supabaseWorkTracker.Person)
            .Order("Start", Postgrest.Constants.Ordering.Descending)
            .Get();
        output.hours = 0;
        output.monthYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        for (int i = 0; i < temp.Models.Count; i++)
        {
            if (temp.Models[i].Start.Month == DateTime.Now.Month && temp.Models[i].Start.Year == DateTime.Now.Year && temp.Models[i].Start != temp.Models[i].End)
            {
                if (temp.Models[i].Start.Day < temp.Models[i].End.Day)
                {
                    output.hours += ((temp.Models[i].End.Hour - temp.Models[i].Start.Hour) - 12) * -1;
                }
                else
                {
                    output.hours += temp.Models[i].End.Hour - temp.Models[i].Start.Hour;
                }
            }
        }
        return output;
    }
}

public struct WorkAmountForMonth
{
    public int hours;
    public DateTime monthYear;
}

[Table("WorkTimeTracker")]
public class supabaseWorkTracker : BaseModel
{
    [PrimaryKey("Start")]
    public DateTime Start { get; set; }

    [Column("End")]
    public DateTime End { get; set; }

    [Column("Person")]
    public long Person { get; set; }
}