using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

class TeamEmbed : TEmbed
{
    protected List<Team2> _teams;

    public void SetupEmbed(List<Team2> teams)
    {
        _teams = teams;
        _title = "Teams";
        _description = "A list of teams that meet a specified criteria";

        _fields = new List<Discord.EmbedFieldBuilder>();
        foreach (var team in teams)
        {
            if (team.getID() != 16)
            {
                string members;
                    List<Person2> temp = team.getMembers();
                    members = temp[0].getName();
                for (int i = 1; i < temp.Count; i++)
                {
                    members += ", ";
                    members += temp[i].getName();
                }

                _fields.Add(new Discord.EmbedFieldBuilder()
                {
                    Name = "ID",
                    Value = team.getID(),
                    IsInline = true,
                });
                _fields.Add(new Discord.EmbedFieldBuilder()
                {
                    Name = "Name",
                    Value = team.getName(),
                    IsInline = true,
                });
                _fields.Add(new Discord.EmbedFieldBuilder()
                {
                    Name = "Members",
                    Value = members,
                    IsInline = true
                });
            };
        }
    }
    public void SetupEmbed(Team2 team)
    {
        _teams = new List<Team2>() { team };
        _title = team.getName();
        _description = "Information about this team";

        _fields = new List<Discord.EmbedFieldBuilder>();
        List<Person2> temp = team.getMembers();
        string members = temp[0].getName();
        for (int i = 1; i < temp.Count; i++)
        {
            members += ", ";
            members += temp[i].getName();
        }
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "ID",
            Value = team.getID(),
            IsInline = true,
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Name",
            Value = team.getName(),
            IsInline = true,
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Members",
            Value = members,
            IsInline = true
        });
    }
}
