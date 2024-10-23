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
            PrintDocumentation();
        }
        public static void ClearConsole(List<string> Flags, List<string> Arguments)
        {
            Console.Clear();
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
        static void PrintDocumentation()
        {
            Console.Clear();
            Console.WriteLine("\n==== Essential Linux Commands ====\n");
            Console.WriteLine("This guide covers common Linux commands with their essential flags and arguments for file management, system operations, process management, networking, and text processing.\n");

            PrintBasicFileOperations();
            PrintSystemOperations();
            PrintProcessManagement();
            PrintNetworking();
        }

        static void PrintBasicFileOperations()
        {
            Console.WriteLine("\n== Basic File Operations ==\n");

            Console.WriteLine("1. ls: List Directory Contents");
            Console.WriteLine("   - `ls`                : List contents of the current directory.");
            Console.WriteLine("   - `ls -l`             : List in long format (detailed view).\n");

            Console.WriteLine("2. cd: Change Directory");
            Console.WriteLine("   - `cd /path/to/dir`   : Change to a specified directory.");
            Console.WriteLine("   - `cd`               : Change to the home directory.");
            Console.WriteLine("   - `cd ../`           : Move up one directory level.\n");

            Console.WriteLine("3. pwd: Print Working Directory");
            Console.WriteLine("   - `pwd`              : Prints the full pathname of the current directory.\n");

            Console.WriteLine("4. mkdir: Make Directories");
            Console.WriteLine("   - `mkdir dir_name`    : Create a new directory.\n");

            Console.WriteLine("5. touch: Create or Update a File");
            Console.WriteLine("   - `touch file_name`   : Create a new empty file or update its timestamp.\n");

            Console.WriteLine("6. rm: Remove Files or Directories");
            Console.WriteLine("   - `rm file_name`      : Remove a specific file.");
            Console.WriteLine("   - `rm -r dir_name`    : Remove a directory and its contents.");
            Console.WriteLine("   - `rm -f file_name`   : Force removal of a file.\n");

            Console.WriteLine("7. rmdir: Remove Empty Directories");
            Console.WriteLine("   - `rmdir dir_name`    : Remove an empty directory.\n");
        }

        static void PrintSystemOperations()
        {
            Console.WriteLine("\n== System Operations ==\n");

            Console.WriteLine("1. echo: Display Text");
            Console.WriteLine("   - `echo \"Hello, World!\"`\n");

            Console.WriteLine("2. vw: View File Contents or Metadata");
            Console.WriteLine("   - `vw file_name -i`   : Display file contents.");
            Console.WriteLine("   - `vw file_name -c`   : Display file information.\n");

            Console.WriteLine("3. write: Write to Files");
            Console.WriteLine("   - `write -w file_name \"content\"`  : Overwrite file with content.");
            Console.WriteLine("   - `write -a file_name \"content\"`  : Append content to a file.\n");

            Console.WriteLine("4. df: Disk Space Usage");
            Console.WriteLine("   - `df`               : Show disk usage in human-readable format.\n");

            Console.WriteLine("5. du: Disk Usage of Directories");
            Console.WriteLine("   - `du path`           : Display disk usage of a specified directory.\n");

            Console.WriteLine("6. cls: Clear Console");
            Console.WriteLine("   - `cls`           : Clear console from all commands.\n");

            Console.WriteLine("7. help: Display All Commands");
            Console.WriteLine("   - `help`           : Display all commands like documantaion.\n");

            Console.WriteLine("8. exisit: Stop App");
            Console.WriteLine("   - `exisit`           : stop app from running and close.\n");
        }

        static void PrintProcessManagement()
        {
            Console.WriteLine("\n== Process Management ==\n");

            Console.WriteLine("1. ps: Process Snapshot");
            Console.WriteLine("   - `ps`               : List all running processes.");
            Console.WriteLine("   - `ps -aux`          : Show detailed information about all running processes.\n");

            Console.WriteLine("2. top: Interactive Process Viewer");
            Console.WriteLine("   - `top`              : Launch interactive process viewer.\n");

            Console.WriteLine("3. kill: Terminate Process");
            Console.WriteLine("   - `kill PID`         : Terminate a process by its PID.");
            Console.WriteLine("   - `kill -9 PID`      : Forcefully terminate a process.\n");

            Console.WriteLine("4. bg: Resume Background Job");
            Console.WriteLine("   - `bg job_id`        : Resume a specific job in the background.\n");

            Console.WriteLine("5. fg: Bring Job to Foreground");
            Console.WriteLine("   - `fg job_id`        : Bring a background job to the foreground.\n");
        }

        static void PrintNetworking()
        {
            Console.WriteLine("\n== Networking ==\n");

            Console.WriteLine("1. ping: Test Network Connectivity");
            Console.WriteLine("   - `ping hostname`    : Ping a specific host to test connectivity.\n");

            Console.WriteLine("2. ifconfig: Configure Network Interfaces");
            Console.WriteLine("   - `ifconfig`         : Display network interface configuration.\n");

            Console.WriteLine("3. netstat: Network Statistics");
            Console.WriteLine("   - `netstat`          : Show all active network connections.");
            Console.WriteLine("   - `netstat -t`       : Display active TCP connections.");
            Console.WriteLine("   - `netstat -u`       : Display active UDP connections.\n");

            Console.WriteLine("4. ssh: Secure Shell (SSH) Login");
            Console.WriteLine("   - `ssh user@hostname` password : Connect to a remote host using SSH.\n");
        }
    }
}
