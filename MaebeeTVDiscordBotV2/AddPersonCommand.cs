using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

class AddPersonCommand : SlashCommand
{
    Person _person;
    public AddPersonCommand()
    {
        _name = "add-person";
        _description = "Adds a person to the Database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "name",
                "The nickname or chosen name of the person",
                true,
                Discord.ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "teams",
                "The teams this member is a part of (comma separated, '/display-teams' to show all available teams",
                true,
                Discord.ApplicationCommandOptionType.String),
            new SlashCommandOption(
                "discord",
                "The @ of the user on discord",
                true,
                Discord.ApplicationCommandOptionType.User),
            new SlashCommandOption(
                "strike-count",
                "The number of strikes this person has recieved",
                false,
                Discord.ApplicationCommandOptionType.Integer),
        }
        ;
    }

    public async Task HandleAccept(SocketMessageComponent command)
    {
        await _person.PushToDatabase();
        await command.RespondAsync("Database Updated", ephemeral: Ephemeral);
    }
    public async Task HandleCancel(SocketMessageComponent command)
    {
        _person = null;
        await command.RespondAsync("Datebase Update Cancelled", ephemeral: Ephemeral);

    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        embed = new List<TEmbed>();
        await base.HandleCommand(command);

        string[] temp = new string[1];
        temp[0] = ", ";

        long strikesValue;
        if (command.Data.Options.Count < 4)
        {
            strikesValue = 0;
        }
        else
        {
            strikesValue = long.Parse(command.Data.Options.Where(x => x.Name == "strike-count").First().Value.ToString());
        }

        var teams = command.Data.Options.Where(x => x.Name == "teams").First().Value.ToString().Split(temp, StringSplitOptions.None);

        _person = new Person();
        await _person.SetPerson(
            command.Data.Options.Where(x => x.Name == "name").First().Value.ToString(),
            ((SocketGuildUser)command.Data.Options.Where(x => x.Name == "discord").First().Value).Username + "#" + ((SocketGuildUser)command.Data.Options.Where(x => x.Name == "discord").First().Value).Discriminator,
            teams,
            strikesValue
            );
        var thing = new UserEmbed();
        await thing.SetupEmbed(_person, _person.GetTeams());
        embed.Add(thing);

        _buttons = new List<TButton>()
        {
            new AcceptButton(HandleAccept, _name),
            new CancelButton(HandleCancel, _name),
        };

        await command.FollowupAsync(embed: embed[0].Build(), ephemeral: Ephemeral, components: GetButtons());
        await command.DeleteOriginalResponseAsync();
    }
}

