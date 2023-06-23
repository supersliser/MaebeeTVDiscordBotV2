using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class WelcomeCommand : SlashCommand
{
    public WelcomeCommand()
    {
        _name = "initiate-hello";
        _description = "Says hello";
        _ephemeral = false;
        _options = new List<SlashCommandOption>()
        {
            new SlashCommandOption(
                "vc",
                "The VC which the bot should join",
                true,
                Discord.ApplicationCommandOptionType.Channel)
        };
    }

    [Command("initiate-hello", RunMode = RunMode.Async)]
    public override async Task HandleCommand(SocketSlashCommand command)
    {
        await command.RespondAsync("Preparing to get funky");

        embed = new List<TEmbed>
        {
            new WelcomeEmbed()
        };

        var voice = new VoiceHandler();
        
        //await voice.JoinAudio(((IVoiceChannel)command.Data.Options.Where(x => x.Name == "vc").Last().Value).Guild,(IVoiceChannel)command.Data.Options.Where(x => x.Name == "vc").Last().Value);
        //await voice.SendAudioAsync(((IVoiceChannel)command.Data.Options.Where(x => x.Name == "vc").Last().Value).Guild, command.Channel, "'/voicebooking-speech.wav/'");

        await embed.Last().SetupEmbed();
        await command.FollowupAsync(embed: embed.Last().Build(), ephemeral: _ephemeral);
        await command.DeleteOriginalResponseAsync();
    }
}
