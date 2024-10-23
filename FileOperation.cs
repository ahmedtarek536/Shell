using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public static class FileOperation
    {
        public static void ListFilesAndDirectories(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count > 1 || Arguments.Count != 0)
            {
                Settings.ErrorMessage("Error: not vaild arguments or flags.");
                return;
            }
            bool LongPath = false;
            if (Flags.Count == 1 && Flags[0] == "-d" ) LongPath = true;
            var CurrentPath = Settings.CurrentPath;

            if (LongPath)
            {
                foreach (var entry in Directory.GetDirectories(CurrentPath))
                    Console.WriteLine($"\t[Dir] {(entry)}");
                foreach (var entry in Directory.GetFiles(CurrentPath))
                    Console.WriteLine($"\t[File] {(entry)}");
            }
            else
            {
                foreach (var entry in Directory.GetDirectories(CurrentPath))
                    Console.Write($"{Path.GetFileName(entry)}  ");
                foreach (var entry in Directory.GetFiles(CurrentPath))
                    Console.Write($"{Path.GetFileName(entry)}  ");
            }

           

       
        }

        public static void PrintWorkingDirectory(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count != 0 || Arguments.Count != 0)
                Settings.ErrorMessage("Error: this command don't take any arguments or flags.");
            else Console.WriteLine(Settings.CurrentPath);
        }
        public static void ChangeCurrentDirectory(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 1 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: Error: not vaild arguments.");
                return;
            };
            var path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
            if (Directory.Exists(path)) Settings.CurrentPath = path;
            else Settings.ErrorMessage("Error: This path not found");
        }
        public static void MakeDirectory(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count != 0 || Arguments.Count != 1)
            {
                Settings.ErrorMessage("Error: not vaild arguments.");
            }
            else
            {
                string path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
                if (Directory.Exists(path)) Settings.ErrorMessage("Error: This Directory already Exisit");
                else Directory.CreateDirectory(path);
            }
        }
        public static void MakeFile(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count != 0 || Arguments.Count != 1)
            {
                Settings.ErrorMessage("Error: not vaild arguments.");
            }
            else
            {
                string path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
                if (File.Exists(path)) Settings.ErrorMessage("Error: This File already Exisit");
                else
                {
                    var file = File.Create(path);
                    file.Close();
                }
            }
        }
        public static void RemoveDirectory(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count != 0 || Arguments.Count != 1)
            {
                Settings.ErrorMessage("Error: not vaild arguments.");
            }
            else
            {
                string path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
                if (!Directory.Exists(path)) Settings.ErrorMessage("Error: This Directory not found!");
                else Directory.Delete(path,true);
            }
        }
        public static void RemoveFileOrDirectory(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count > 1 || Arguments.Count != 1)
            {
                Settings.ErrorMessage("Error: not vaild arguments or flags.");
                return;
            }
            bool force = false;
            if (Flags.Count == 1 && (Flags[0] == "-f" || Flags[0] == "-r") ) force = true;
 
            string path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, force);
                }
                catch { Settings.ErrorMessage("Error: The directory is not empty. Use the -r flag to remove it recursively or -f to force deletion."); }
            }
            else if (File.Exists(path)) File.Delete(path);
            else Settings.ErrorMessage("Error: This directory or file not found!");
        }
        
    }
}
