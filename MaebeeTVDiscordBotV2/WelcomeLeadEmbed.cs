using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WelcomeLeadEmbed : TEmbed
{
    public void SetupEmbed()
    {
        _title = "Hi There";
        _description = "So heres how to setup a task";
        _fields = new List<Discord.EmbedFieldBuilder>()
        {
            new Discord.EmbedFieldBuilder()
            {
                Name = "Command",
                Value = "Do /add-task to see the options. Make sure to include either the a team or some people",
                IsInline = true
            },
        };
    }
}
