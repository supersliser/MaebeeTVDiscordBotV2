using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

class TeamEmbed : TEmbed
{
    protected List<Team> _teams;

    public override async Task SetupEmbed(List<Team> teams)
    {
        _teams = teams;
        _title = "Teams";
        _description = "A list of teams that meet a specified criteria";

        _fields = new List<Discord.EmbedFieldBuilder>();
        foreach (var team in teams)
        {
            if (team.ID != 16)
            {
                string members;
                    List<Person> temp = await team.GetPeopleFromTeam();
                    members = temp[0].Name;
                for (int i = 1; i < temp.Count; i++)
                {
                    members += ", ";
                    members += temp[i].Name;
                }

                _fields.Add(new Discord.EmbedFieldBuilder()
                {
                    Name = "ID",
                    Value = team.ID,
                    IsInline = true,
                });
                _fields.Add(new Discord.EmbedFieldBuilder()
                {
                    Name = "Name",
                    Value = team.Name,
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
    public override async Task SetupEmbed(Team team)
    {
        _teams = new List<Team>() { team };
        _title = team.Name;
        _description = "Information about this team";

        _fields = new List<Discord.EmbedFieldBuilder>();
        List<Person> temp = await team.GetPeopleFromTeam();
        string members = temp[0].Name;
        for (int i = 1; i < temp.Count; i++)
        {
            members += ", ";
            members += temp[i].Name;
        }
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "ID",
            Value = team.ID,
            IsInline = true,
        });
        _fields.Add(new Discord.EmbedFieldBuilder()
        {
            Name = "Name",
            Value = team.Name,
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
