using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WelcomeEmbed : TEmbed
{
    public override async Task SetupEmbed()
    {
        _title = "Welcome to MaebeeTVBot V2";
        _description = "We dont talk about V1, mans was too unorganised";
        _fields = new List<Discord.EmbedFieldBuilder>()
        {
            new Discord.EmbedFieldBuilder()
            {
                Name = "I'm here to help",
                Value = "My primary function is to store data about you and your tasks, this data is stored in a secure database named supabase, even mae does not have direct access to the data that is stored in there.",
                IsInline = false
            },
            new Discord.EmbedFieldBuilder()
            {
                Name = "Data I store",
                Value = "The data I store is your preferred name, discord and any information about reports or work hours. As well as your teams and which ones youre in. To view this data, enter /display-person and then select yourself",
                IsInline = false
            },
            new Discord.EmbedFieldBuilder()
            {
                Name = "Tasks",
                Value = "I alos store data about the tasks going on. Type /display-tasks to see all tasks available to you",
                IsInline = false
            },
            new Discord.EmbedFieldBuilder()
            {
                Name = "Work Hours",
                Value = "Mae has asked that we begin to log work hours, and I can help. Type /start-work-timer to begin recording work hours. When youve finished for the day, type /stop-work-timer",
                IsInline = false
            },
            new Discord.EmbedFieldBuilder()
            {
                Name = "The future",
                Value = "New features will be added soon. If you have any ideas please message Ash",
                IsInline = false
            },
            new Discord.EmbedFieldBuilder()
            {
                Name = "BTW",
                Value = "This was made purely out of spite because no one uses clickup so ash went full on thanos and just said 'fine ill do it myself'",
                IsInline = false
            },
        };
    }
}
