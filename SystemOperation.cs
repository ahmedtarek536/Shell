using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    public class SystemOperation
    {
        public static void ViewFile(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count > 1 || Arguments.Count != 1)
            {
                Settings.ErrorMessage("Error: not vaild arguments or flags.");
                return;
            }
            bool info = false;
            bool content = true;
            if (Flags.Count == 1 && Flags[0] == "-i")
            {
                info = true;
                content = false;
            }
            else if (Flags.Count == 1 && Flags[0] == "-c") content = true;
            else if (Flags.Count == 1)
            {
                Settings.ErrorMessage("Error: not vaild flags!!");
                return;
            }


            string path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
            if (File.Exists(path))
            {
                if (info)
                {
                    var file = new FileInfo(path);
                    Console.WriteLine("Name: " + file.Name);
                    Console.WriteLine("Type: File");
                    Console.WriteLine($"Create At {file.CreationTime}");
                    Console.WriteLine($"Modified At {file.LastWriteTime}");
                    Console.WriteLine($"Size in {file.Length}");
                }
                else if (content)
                {

                    var Text = File.ReadAllText(path);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(Text);
                    Console.ForegroundColor = ConsoleColor.White;

                }
            }
            else Settings.ErrorMessage("Error: File not found !!");

        }
        public static void WriteInFile(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count > 1 || Arguments.Count != 2)
            {
                Settings.ErrorMessage("Error: not vaild arguments or flags.");
                return;
            }
            bool write = false;
            bool append = true;
            if (Flags.Count == 1 && Flags[0] == "-w")
            {
                write = true;
                append = false;
            }
            else if (Flags.Count == 1 && Flags[0] == "-a") write = true;
            else if (Flags.Count == 1)
            {
                Settings.ErrorMessage("Error: not vaild flags!!");
                return;
            }

            string path = Path.GetFullPath(Path.Combine(Settings.CurrentPath, Arguments[0]));
            if (File.Exists(path))
            {
                if (write)
                {
                    File.WriteAllText(path, Arguments[1] + Environment.NewLine);
                }
                else if (append)
                {
                    File.AppendAllText(path, Arguments[1] + Environment.NewLine);
                }

            }
            else Settings.ErrorMessage("Error: File not found !!");

        }
        public static void PrintText(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count != 0 || Arguments.Count != 1)
            {
                Settings.ErrorMessage("Error: not vaild arguments.");
            }
            else Console.WriteLine(Arguments[0]);
        }
        public static void DisplayDiskSpaceUsage(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count != 0 || Arguments.Count != 0)
            {
                Settings.ErrorMessage("Error: not vaild arguments or flags.");
                return;
            }

            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10}", "Filesystem", "Size", "Used", "Available", "Usage");

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    long totalSize = drive.TotalSize;
                    long availableSpace = drive.AvailableFreeSpace;
                    long usedSpace = totalSize - availableSpace;

                    string size = ConvertToHumanReadable(totalSize);
                    string used = ConvertToHumanReadable(usedSpace);
                    string available = ConvertToHumanReadable(availableSpace);
                    double usagePercentage = ((double)usedSpace / totalSize) * 100;
                    Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,5:0.0}%", drive.Name, size, used, available, usagePercentage);
                }
            }
        }
        public static void DisplayFileSpaceUsage(List<string> Flags, List<string> Arguments)
        {
            string path = Arguments.Count > 0 ? Arguments[0] : Directory.GetCurrentDirectory();

            if (Directory.Exists(path))
            {
                long totalSize = GetDirectorySize(path);

                string humanReadableSize = ConvertToHumanReadable(totalSize);

                Console.WriteLine($"Total size of {path}: {humanReadableSize}");
            }
            else if (File.Exists(path))
            {
                long fileSize = new FileInfo(path).Length;

                string humanReadableSize = ConvertToHumanReadable(fileSize);

                Console.WriteLine($"Size of {path}: {humanReadableSize}");
            }
            else
            {
                Settings.ErrorMessage($"The path '{path}' does not exist.");
            }
        }
        public static void DisplayAllCommands(List<string> Flags, List<string> Arguments) {
            Console.WriteLine("all command will provide soon");
        }
        public static void Exisit(List<string> Flags, List<string> Arguments) {
            if (Flags.Count != 0 || Arguments.Count != 0)
            {
                Settings.ErrorMessage("Error: not vaild arguments.");
            }
            else Settings.Running = false;
        }

        static long GetDirectorySize(string directoryPath)
        {
            long size = 0;

            foreach (string filePath in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                size += fileInfo.Length;
            }

            return size;
        }


        public static string ConvertToHumanReadable(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double readableSize = size;
            while (readableSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                readableSize /= 1024;
            }
            return $"{readableSize:0.00} {sizes[order]}";
        }
    }
}
