using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DisplayWorkHoursCommand : SlashCommand
{
    public DisplayWorkHoursCommand()
    {
        _name = "display-work-hours";
        _description = "Displays all work hours that have been logged to the database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "person",
                "the discord of the person whos hours you would like to see",
                true,
                Discord.ApplicationCommandOptionType.User)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);

        string discord = ((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value).Username + "#" + ((SocketGuildUser)command.Data.Options.Where(x => x.Name == "person").First().Value).Discriminator;

        WorkTimeTracker output = new WorkTimeTracker();
        await output.SetPerson(discord);
        embed = new List<TEmbed>();
        List<WorkAmountForMonth> temp = await output.GetListPreviousTimes();
        temp.Add(await output.GetMonthTimes());

        for (int i = 1; i <= (temp.Count / 25); i++)
        {
            var thing = new WorkHoursEmbed();
            var person = new Person();
            await person.GetFromDatabase(output.ID);
            await thing.SetupEmbed(temp.GetRange(i, 25), person.Name);
            embed.Add(thing);
        };
        var thingout = new WorkHoursEmbed();
        var personout = new Person();
        await personout.GetFromDatabase(output.ID);
        await thingout.SetupEmbed(temp.GetRange(((temp.Count / 25) * 25), temp.Count % 25), personout.Name);
        embed.Add(thingout);

        var embedOutput = new List<Embed>();
        foreach (var embed in embed)
        {
            embedOutput.Add(embed.Build());
        }

        await command.FollowupAsync(embeds: embedOutput.ToArray(), ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
