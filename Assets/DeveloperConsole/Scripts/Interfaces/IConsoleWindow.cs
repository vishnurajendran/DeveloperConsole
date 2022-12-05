namespace RuntimeDeveloperConsole
{
    public interface IConsoleWindow
    {
        /// <summary>
        /// returns full console output
        /// </summary>
        public string ConsoleOutput { get; }

        /// <summary>
        /// prints a message to console
        /// </summary>
        public void PrintLineToConsole(string message)
        {

        }
    }
}
