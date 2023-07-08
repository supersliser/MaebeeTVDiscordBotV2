using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

class AddPersonCommand : SlashCommand
{
    Person2 _person;
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
        }
        ;
    }

    public async Task HandleAccept(SocketMessageComponent command)
    {
        await new DatabasePersonController().PushToDatabase(_person);
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

        var teams = command.Data.Options.Where(x => x.Name == "teams").First().Value.ToString().Split(temp, StringSplitOptions.None);

        _person = new Person2();
        _person.setName(command.Data.Options.Where(x => x.Name == "name").First().Value.ToString());
        _person.setDiscord((SocketGuildUser)command.Data.Options.Where(x => x.Name == "discord").First().Value);
        _person.setTeams();
        var thing = new UserEmbed();
        await thing.SetupEmbed(_person, _person.getTeams());
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

