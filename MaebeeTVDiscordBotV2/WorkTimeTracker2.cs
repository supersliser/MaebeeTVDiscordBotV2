using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class WorkTimeTracker2
{
    protected DateTime Start;
    protected DateTime End;
    protected Person2 Person;

    public WorkTimeTracker2()
    {

    }
    public WorkTimeTracker2(DateTime start, DateTime end, Person2 person)
    {
        Start = start;
        End = end;
        Person = person;
    }
    public WorkTimeTracker2(supabaseWorkTracker workTracker)
    {
        setStart(workTracker.Start);
        setEnd(workTracker.End);
        setPerson(new DatabasePersonController().useID(workTracker.Person).Result);
    }

    public void setStart(DateTime start)
    {
        Start = start;
    }
    public void setEnd(DateTime end)
    {
        End = end;
    }
    public void setStart(string start)
    {
        Start = DateTime.Parse(start);
    }
    public void setEnd(string end)
    {
        End = DateTime.Parse(end);
    }
    public void setPerson(Person2 person)
    {
        Person = person;
    }
    public void setSupabase(supabaseWorkTracker workTracker)
    {
        setStart(workTracker.Start);
        setEnd(workTracker.End);
        setPerson(new DatabasePersonController().useID(workTracker.Person).Result);
    }

    public DateTime getStart()
    {
        return Start;
    }
    public DateTime getEnd()
    {
        return End;
    }
    public Person2 getPerson()
    {
        return Person;
    }
    public supabaseWorkTracker getSupabase()
    {
        return new supabaseWorkTracker()
        {
            Start = Start,
            End = End,
            Person = Person.getID()
        };
    }
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


public struct WorkAmountForMonth
{
    public int hours;
    public DateTime monthYear;
}