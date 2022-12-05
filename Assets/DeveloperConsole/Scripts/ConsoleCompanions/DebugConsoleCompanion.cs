using UnityEngine;
using RuntimeDeveloperConsole;

public class DebugConsoleCompanion
{
    [ConsoleCommand("print message to screen", "echo <message>")]
    public static void Echo(string[] args)
    {
        if (args == null || args.Length <= 0)
            return ;

        Debug.Log(string.Join(" ",args));
    }

    [ConsoleCommand("Display commands available", "help <filter>")]
    public static string Help(string[] args)
    {
        return CommandDatabase.GetAvailableCommandsHelp(args);
    }
}
