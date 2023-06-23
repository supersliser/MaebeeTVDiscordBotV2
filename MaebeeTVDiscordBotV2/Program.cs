using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using Discord.Net;
using Newtonsoft.Json;

class Program
{
    private DiscordSocketClient _client;
#pragma warning disable IDE0044 // Add readonly modifier
    List<SlashCommand> commands = new List<SlashCommand>();
#pragma warning restore IDE0044 // Add readonly modifier


#pragma warning disable IDE0060 // Remove unused parameter
    public static Task Main(string[] args) => new Program().MainAsync();
#pragma warning restore IDE0060 // Remove unused parameter

    public async Task MainAsync()
    {
        Environment.SetEnvironmentVariable("DiscordToken", "MTEwNTU3MTcwNDUxMzc3MzY2OQ.Gm9Jpt.SIsXuMbPse96fukI9rI91fjLnUWbAj_cOOXTOA", EnvironmentVariableTarget.Process);

        _client = new DiscordSocketClient();
        _client.Log += Log;
        await _client.LoginAsync(TokenType.Bot,
            Environment.GetEnvironmentVariable("DiscordToken"));
        await _client.StartAsync();

        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        _client.ButtonExecuted += ButtonHandler;
        _client.ModalSubmitted += ModalHandler;
        await Task.Delay(-1);
    }
    public async Task Client_Ready()
    {
        UInt64[] guilds = new UInt64[2];
        guilds[0] = 903258487872700517;
        guilds[1] = 833059868272885781;


        commands.Add(new FrogSlashCommand());
        commands.Add(new AddPersonCommand());
        commands.Add(new AddTeamCommand());
        commands.Add(new DisplayTeamsCommand());
        commands.Add(new AddTaskCommand());
        commands.Add(new DisplayTasksCommand());
        commands.Add(new ReportActivityCommand());
        commands.Add(new ReportOccuranceCommand());
        commands.Add(new DisplayPersonCommand());
        commands.Add(new UpdateDiscordCommand());
        commands.Add(new StartTimerCommand());
        commands.Add(new StopTimerCommand());
        commands.Add(new DisplayWorkHoursCommand());

        for (int i = 0; i < guilds.Length; i++)
        {
            var guild = _client.GetGuild(guilds[i]);
            try
            {
                foreach (var command in commands)
                {
                    await guild.CreateApplicationCommandAsync(command.BuildCommand());
                }
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }

        }
    }
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        foreach (var ExistantCommand in commands)
        {
            if (command.Data.Name == ExistantCommand.Name)
            {
                await ExistantCommand.HandleCommand(command);
            }
        }
    }
    private async Task ButtonHandler(SocketMessageComponent component)
    {
        char[] temp = new char[1];
        temp[0] = char.Parse("_");
        foreach (var ExistantCommand in commands)
        {
            if (component.Data.CustomId.Split(temp).ElementAt(0) == ExistantCommand.Name)
            {
                if (ExistantCommand.HasButtons)
                foreach (var Button in ExistantCommand.GetButtonsRaw())
                    {
                        if (Button.Id == component.Data.CustomId)
                        {
                            await Button.Click(component);
                        }
                    }
            }
        }
    }
    private async Task ModalHandler(SocketModal component)
    {
        char[] temp = new char[1];
        temp[0] = char.Parse("_");
        foreach (var ExistantCommand in commands)
        {
            if (component.Data.CustomId.Split(temp).First() == ExistantCommand.Name)
            {
                if (ExistantCommand.HasModal)
                {
                    await ExistantCommand.GetModal().Accept(component);
                }
            }
        }
    }
}
