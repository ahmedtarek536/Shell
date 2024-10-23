using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public class StateMachine
    {
        public string CurrentState { get; set; } = "start";
        public Dictionary<string, Action<List<string>, List<string>>> commandDictionary = new Dictionary<string, Action<List<string>, List<string>>>
        {
           { "ls", FileOperation.ListFilesAndDirectories },
           { "pwd", FileOperation.PrintWorkingDirectory },
           { "cd", FileOperation.ChangeCurrentDirectory },
           { "mkdir", FileOperation.MakeDirectory },
           { "touch", FileOperation.MakeFile },
           { "rmdir", FileOperation.RemoveDirectory },
           { "rm", FileOperation.RemoveFileOrDirectory },

           { "echo", SystemOperation.PrintText },
           { "vw", SystemOperation.ViewFile },
           { "write", SystemOperation.WriteInFile },
           { "df", SystemOperation.DisplayDiskSpaceUsage },
           { "du", SystemOperation.DisplayFileSpaceUsage },
           { "help", SystemOperation.DisplayAllCommands },
           { "cls", SystemOperation.ClearConsole },
           { "exisit", SystemOperation.Exisit },

           { "ps" , ProcessManagement.DisplayRunningProcesses},
           { "kill", ProcessManagement.KillProcesse},
           { "top", ProcessManagement.DisplayTasks},
           { "bg", ProcessManagement.MoveProcessInBackground},
           { "fg", ProcessManagement.MoveProcessToForeground},

           { "ifconfig", Networking.DisplayNetworkInterfaces},
           { "netstat", Networking.DisplayNetworkConnections},
           { "ping", Networking.PingHost},
           { "ssh", Networking.ConnectToHost},
        };


        public void Process(List<string> flags, List<string> arguments)
        {
            if (commandDictionary.ContainsKey(CurrentState))
            {
                commandDictionary[CurrentState].Invoke(flags, arguments);
            }
            else
            {
                Settings.ErrorMessage($"Unknown command: {CurrentState}");
            }
            
        }
        public void SwitchState(string state)
        {
            CurrentState = state;
        }
    } 
}
