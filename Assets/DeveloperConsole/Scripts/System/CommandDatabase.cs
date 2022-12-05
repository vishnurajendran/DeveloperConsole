using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace RuntimeDeveloperConsole {
    public class CommandDatabase
    {
        private static Dictionary<string, MethodInfo> registeredCommands;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialise()
        {
            Debug.Log("Initialising Console Database");
            System.Diagnostics.Stopwatch perfTimer = new System.Diagnostics.Stopwatch();
            perfTimer.Start();
            registeredCommands = new Dictionary<string, MethodInfo>();

            var currentAssembly = Assembly.GetExecutingAssembly();
            var methods = currentAssembly.GetTypes()
                          .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                          .Where(m => m.GetCustomAttributes(typeof(ConsoleCommandAttribute), false).Length > 0)
                          .ToArray();

            foreach (var method in methods)
            {
                registeredCommands.Add(method.Name.ToLower(), method);
            }
            perfTimer.Stop();
            Debug.Log($"Initialising Console Database ({registeredCommands.Count} commands in {perfTimer.Elapsed.TotalSeconds}s)");
        }

        public static string Run(ConsoleCommand command)
        {
            var cmdName = command.Command.ToLower();
            if (!registeredCommands.ContainsKey(cmdName.ToLower()))
                return string.Format(ConsoleConstants.COMMAND_NOT_FOUND_STRING, cmdName);

            object result = null;
            var methodInfo = registeredCommands[cmdName];
            var methodArgs = methodInfo.GetParameters();
            if (methodArgs == null || methodArgs.Length <= 0)
            {
                methodInfo.Invoke(null, null);
            }
            else if(methodArgs.Length == 1)
            {
                result = methodInfo.Invoke(null, new object[] { command.Arguments });
            }
            else
            {
                Debug.LogError(ConsoleConstants.COMMAND_INCORRECT_SIGNATURE);
                return string.Empty;
            }

            return result != null ? result.ToString() : string.Empty;
        }

        public static string GetAvailableCommandsHelp(string[] filter)
        {
            string text = "-- Help --\n";
            if (filter == null || filter.Length <= 0)
            {
                foreach (var kv in registeredCommands)
                {
                    var attrib = kv.Value.GetAttribute<ConsoleCommandAttribute>();
                    text += $"{kv.Key}\n\t{attrib.Description}\n\t{attrib.ArgHelpText}\n";
                }
                return text;
            }
            else if (filter.Length == 1)
            {
                if (registeredCommands.ContainsKey(filter[0]))
                    return ConsoleConstants.COMMAND_NOT_FOUND;
                else
                {
                    var cmd = registeredCommands[filter[0]];
                    var attrib = cmd.GetAttribute<ConsoleCommandAttribute>();
                    return $"{cmd.Name.ToLower()}\n\t{attrib.Description}\n\t{attrib.ArgHelpText}\n";
                }
            }
            else return ConsoleConstants.TOO_MANY_ARGS_TEXT;
        }
    }
}