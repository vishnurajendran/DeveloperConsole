using UnityEngine;
using UnityEngine.UI;

namespace RuntimeDeveloperConsole
{
    public class ConsoleWindow : MonoBehaviour, IConsoleWindow
    {
        [SerializeField]
        RectTransform layoutToRebuild;

        [SerializeField]
        private TMPro.TMP_InputField inputField;
        [SerializeField]
        private TMPro.TMP_Text consoleOutput;

        public string ConsoleOutput => consoleOutput.text;

        private void Start()
        {
            inputField.onSubmit.AddListener(SubmitCommand);
            ConsoleSystem.SetConsoleWindow(this);
        }

        private void SubmitCommand(string commandString)
        {
            if (string.IsNullOrEmpty(commandString))
            {
                inputField.text = string.Empty;
                return;
            }

            //add command to output
            consoleOutput.text += $"{ConsoleConstants.TERM_KEY}<color=yellow>{commandString}</color>\n";
            inputField.text = string.Empty;

            HandleCommand(commandString);
        }

        private void HandleCommand(string commandString)
        {
            ConsoleSystem.HandleCommand(commandString);
        }

        public void PrintLineToConsole(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            consoleOutput.text += $"{ConsoleConstants.TERM_KEY}{message}\n";
            LayoutRebuilder.MarkLayoutForRebuild(layoutToRebuild);
        }
    }
}
