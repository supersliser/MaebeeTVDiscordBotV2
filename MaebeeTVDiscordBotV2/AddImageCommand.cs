using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class AddImageCommand : SlashCommand
{
    public AddImageCommand()
    {
        _name = "add-image";
        _description = "Adds your image to the database";
        _ephemeral = true;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "image-link",
                "a link to the image (can be a discord link if needed)",
                true,
                Discord.ApplicationCommandOptionType.String)
        };
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await base.HandleCommand(command);
        var person = await new DatabasePersonController().useDiscord(command.User);
        person.setImage(command.Data.Options.Where(x => x.Name == "image-link").First().Value.ToString());
        await new DatabasePersonController().PushToDatabase(person);
        await command.FollowupAsync("Image updated", ephemeral: Ephemeral);
    }
}
