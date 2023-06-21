using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

class UpdateDiscordCommand : SlashCommand
{
    public UpdateDiscordCommand()
    {
        _name = "update-discord";
        _description = "Updates the discord of the person";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "id",
                "ID of the person (ask ash if unable to get)",
                true,
                Discord.ApplicationCommandOptionType.Number),
            new SlashCommandOption(
                "new-discord",
                "The new discord of the person",
                true,
                Discord.ApplicationCommandOptionType.User)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);
        var person = new Person();
        await person.GetFromDatabase(long.Parse(command.Data.Options.Where(x => x.Name == "id").First().Value.ToString()));
        person.UpdateDiscord(((SocketGuildUser)command.Data.Options.Where(x => x.Name == "new-discord").First().Value).Username + "#" + ((SocketGuildUser)command.Data.Options.Where(x => x.Name == "new-discord").First().Value).Discriminator);
        await person.PushToDatabase();

        await command.FollowupAsync("Discord Updated", ephemeral: Ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
