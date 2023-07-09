using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

class ActivityReport2
{
    protected DateTime Timestamp;
    protected Person2 Reported;
    protected Person2 Reporter;
    protected short Activity;
    protected short Productivity;
    protected short Vibe;

    public ActivityReport2()
    {

    }
    public ActivityReport2(DateTime timestamp, Person2 reported, Person2 reporter, short activity, short producitivity, short vibe)
    {
        Timestamp = timestamp;
        Reported = reported;
        Reporter = reporter;
        Activity = activity;
        Productivity = producitivity;
        Vibe = vibe;
    }
    public ActivityReport2(supabaseActivityReport activityReport)
    {
        setTimestamp(activityReport.Timestamp);
        setReported(new DatabasePersonController().useID(activityReport.Reported).Result);
        setReporter(new DatabasePersonController().useID(activityReport.Reporter).Result);
        setActivity(activityReport.Activity);
        setProductivity(activityReport.Productivity);
        setVibe(activityReport.Vibe);
    }

    public void setTimestamp(DateTime timestamp)
    {
        Timestamp = timestamp;
    }
    public void setTimestamp(string timestamp)
    {
        Timestamp = DateTime.Parse(timestamp);
    }
    public void setReported(Person2 reported)
    {
        Reported = reported;
    }
    public void setReporter(Person2 reporter)
    {
        Reporter = reporter;
    }
    public void setActivity(short activity)
    {
        Activity = activity;
    }
    public void setActivity(int activity)
    {
        Activity = short.Parse(activity.ToString());
    }
    public void setActivity(long activity)
    {
        Activity = short.Parse(activity.ToString());
    }
    public void setProductivity(short Productivity)
    {
        this.Productivity = Productivity;
    }
    public void setProductivity(int Productivity)
    {
        this.Productivity = short.Parse(Productivity.ToString());
    }
    public void setProductivity(long Productivity)
    {
        this.Productivity = short.Parse(Productivity.ToString());
    }
    public void setVibe(short Vibe)
    {
        this.Vibe = Vibe;
    }
    public void setVibe(int Vibe)
    {
        this.Vibe = short.Parse(Vibe.ToString());
    }
    public void setVibe(long Vibe)
    {
        this.Vibe = short.Parse(Vibe.ToString());
    }
    public async void setSupabase(supabaseActivityReport report)
    {
        setTimestamp(report.Timestamp);
        setReporter(await (new DatabasePersonController()).useID(report.Reporter));
        setReported(await (new DatabasePersonController()).useID(report.Reported));
        setActivity(report.Activity);
        setProductivity(report.Productivity);
        setVibe(report.Vibe);
    }

    public DateTime getTimestamp()
    {
        if (Timestamp == null)
        {
            return DateTime.Now;
        }
        return Timestamp;
    }
    public Person2 getReported()
    {
        return Reported;
    }
    public Person2 getReporter()
    {
        return Reporter;
    }
    public short getActivity()
    {
        return Activity;
    }
    public short getProductivity()
    {
        return Productivity;
    }
    public short getVibe()
    {
        return Vibe;
    }
    public supabaseActivityReport getSupabase()
    {
        return new supabaseActivityReport()
        {
            Timestamp = getTimestamp(),
            Reported = getReported().getID(),
            Reporter = getReporter().getID(),
            Activity = getActivity(),
            Productivity = getProductivity(),
            Vibe = getVibe(),
        };
    }
}


[Table("ActivityReport")]
public class supabaseActivityReport : BaseModel
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