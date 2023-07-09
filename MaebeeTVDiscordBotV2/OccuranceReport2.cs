using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class OccuranceReport2
{
    protected DateTime Timestamp;
    protected Person2 Reported;
    protected Person2 Reporter;
    protected string Description;

    public OccuranceReport2()
    {

    }
    public OccuranceReport2(DateTime timestamp, Person2 reported, Person2 reporter, string description)
    {
        Timestamp = timestamp;
        Reported = reported;
        Reporter = reporter;
        Description = description;
    }
    public OccuranceReport2(supabaseOccuranceReport occuranceReport)
    {
        setTimestamp(occuranceReport.Timestamp);
        setReported(new DatabasePersonController().useID(occuranceReport.Reported).Result);
        setReporter(new DatabasePersonController().useID(occuranceReport.Reporter).Result);
        setDescription(occuranceReport.Description);
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
    public void setDescription(string description)
    {
        Description = description;
    }
    public void setSupabase(supabaseOccuranceReport occuranceReport)
    {
        setTimestamp(occuranceReport.Timestamp);
        setReported(new DatabasePersonController().useID(occuranceReport.Reported).Result);
        setReporter(new DatabasePersonController().useID(occuranceReport.Reporter).Result);
        setDescription(occuranceReport.Description);
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
    public string getDescription()
    {
        if (Description == null)
        {
            return "N/A";
        }
        return Description;
    }
    public supabaseOccuranceReport getSupabase()
    {
        return new supabaseOccuranceReport()
        {
            Timestamp = getTimestamp(),
            Reported = getReported().getID(),
            Reporter = getReporter().getID(),
            Description = getDescription()
        };
    }
}

[Table("OccuranceReport")]
public class supabaseOccuranceReport : BaseModel
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