namespace SwinAdventure;

public class CommandProcessor : Command
{
    private List<Command> _commands; 

    public CommandProcessor() : base(new string[] {})
    {
        _commands = new List<Command>();
    }

    public override string Execute(Player p, string[] text)
    {
        foreach(var command in _commands)
        {
            if (command.AreYou(text[0]))
                return command.Execute(p, text);
        }

        return "wrong command or not supported";
    }

    public void AddCommand(Command command)
    {
        _commands.Add(command);
    }
}
