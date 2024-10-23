using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public class Settings
    {
        public static string CurrentPath = Path.GetFullPath(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));
        public static bool Running = true;
        public static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
