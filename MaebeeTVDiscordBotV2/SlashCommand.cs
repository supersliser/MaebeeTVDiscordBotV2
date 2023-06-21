using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SlashCommand
{
    protected string _name;
    protected string _description;
    protected List<SlashCommandOption> _options;
    protected bool _ephemeral;
    protected List<TButton> _buttons;
    protected List<TEmbed> embed;
    protected TModal _modal;


    public string Name 
    { 
        get
        {
            return _name;
        }
    }
    public string Description
    { 
        get 
        { 
            return _description; 
        } 
    }
    public bool Ephemeral
    {
        get { return _ephemeral; }
    }
    public List<SlashCommandOption> Options
    {
        get
        {
            return _options;
        }
    }
    public void AddOption(SlashCommandOption option)
    {
        _options.Add(option);
    }
    public bool HasEmbed
    {
        get
        {
            return embed != null;
        }
    }
    public bool HasButtons
    {
        get
        {
            try
            {
                return _buttons.Count() > 0;
            }
            catch
            {
                return _buttons != null;
            }
        }
    }
    public bool HasModal
    {
        get
        {
            return _modal != null;
        }
    }


    public SlashCommand()
    {

    }

    public virtual SlashCommandProperties BuildCommand()
    {
        var output = new SlashCommandBuilder()
        {
            Name = _name,
            Description = _description,
        };
        if (_options != null)
        {
            foreach (var option in _options) 
            { 
                output.AddOption(option.Build()); 
            }
        }
            

        return output.Build();
    }


    public virtual async Task HandleCommand(SocketSlashCommand command)
    {
        await command.RespondAsync("Command Processing", ephemeral: _ephemeral);
    }

    public MessageComponent GetButtons()
    {
        var ouput = new ComponentBuilder();
        foreach (var button in _buttons)
        {
            ouput.WithButton(button.ButtonBuilder());
        }
        return ouput.Build();
    }    
    public List<ButtonBuilder> GetButtonsUnBuilt()
    {
        var ouput = new List<ButtonBuilder>();
        foreach (var button in _buttons)
        {
            ouput.Add(button.ButtonBuilder());
        }
        return ouput;
    }
    public List<TButton> GetButtonsRaw()
    {
        return _buttons;
    }
    public TModal GetModal()
    {
        return _modal;
    }
    public string ListToString(List<string> items)
    {
        string output;
        output = items[0];
        for (int i = 1; i < items.Count; i++)
        {
            output += ", ";
            output += items[i];
        }

        return output;
    }
}

public class SlashCommandOption
{
    protected string _name;
    protected string _description;
    protected bool _required;
    protected ApplicationCommandOptionType _type;

    public string Name
    {
        get { return _name; }
    }
    public string Description
    { 
        get { return _description; } 
    }
    public bool Required
    {
        get { return _required; }
    }
    public ApplicationCommandOptionType Type
    {
        get { return _type; }
    }

    public SlashCommandOption(string name, string description, bool required, ApplicationCommandOptionType type)
    {
        _name = name;
        _description = description;
        _required = required;
        _type = type;
    }

    public object GetValue(SocketSlashCommand command)
    {
        try
        {
            return command.Data.Options.Where(x => x.Name == _name).FirstOrDefault().Value;
        }
        catch { return null; }
    }

    public SocketUser GetUser(SocketSlashCommand command)
    {
        return command.User;
    }

    public SlashCommandOptionBuilder Build()
    {
        return new SlashCommandOptionBuilder()
        {
            Name = _name,
            Description = _description,
            IsRequired = _required,
            Type = _type,
        };
    }
}