using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DescribeYourselfCommand : SlashCommand
{
    public DescribeYourselfCommand()
    {
        _name = "describe-yourself";
        _description = "Enters a description of yourself to be displayed publically";
        _ephemeral = true;
    }

    public override async Task HandleCommand(SocketSlashCommand command)
    {
        _modal = new DescriptionModal(HandleAccept, _name);

        await command.RespondWithModalAsync(_modal.Build());
    }

    public async Task HandleAccept(SocketModal command)
    {
        Person2 person = await new DatabasePersonController().useDiscord(command.User);
        person.setDescription(command.Data.Components.Where(x => x.CustomId == "description").First().Value);
        await new DatabasePersonController().PushToDatabase(person);
        await command.RespondAsync("Description Updated", ephemeral: Ephemeral);
    }
}
