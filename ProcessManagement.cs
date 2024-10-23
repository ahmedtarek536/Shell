using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Shell
{
    public class ProcessManagement
    {
        public static void DisplayRunningProcesses(List<string> Flags, List<string> Arguments)
        {
            if (Flags.Count > 1 || Arguments.Count != 0)
            {
                Settings.ErrorMessage("Error: not vaild arguments or flags.");
                return;
            }
            bool details = false;
            if (Flags.Count == 1 && Flags[0] == "-aux") details = true;
            else if (Flags.Count == 1)
            {
                Settings.ErrorMessage("Error: not vaild Flags.");
                return;
            }
            Process[] processes = Process.GetProcesses();


            foreach (Process process in processes)
            {
                if (details)
                {
                    try
                    {
                        Console.WriteLine($"Process Name: {process.ProcessName}, Process ID: {process.Id}, Start Time: {process.StartTime}, Total Processor Time: {process.TotalProcessorTime}, Physical Memory Usage: {process.WorkingSet64 / 1024} KB");
                    }
                    catch { }
                }
                else
                {
                    Console.WriteLine($"Process Name: {process.ProcessName}, ID: {process.Id}");
                }
            }
        }
        public static void KillProcesse(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 1 || Flags.Count > 1)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }

            bool force = false;
            if (Flags.Count == 1 && Flags[0] == "-9") force = true;
            else if (Flags.Count == 1)
            {
                Settings.ErrorMessage("Error: not valid Flags.");
                return;
            }

            string pid = Arguments[0];
            if (int.TryParse(pid, out int parsedPid))
            {
                try
                {
                    Process processToKill = Process.GetProcessById(parsedPid);
                    processToKill.Kill();
                }
                catch (Exception ex)
                {
                    Settings.ErrorMessage($"Error: Could not terminate the process. Details: {ex.Message}");
                }
            }
            else
            {
                Settings.ErrorMessage("Invalid PID. Please enter a valid Process ID.");
            }
        }
        public static void DisplayTasks(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 0 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }

            Console.WriteLine("PID\tUSER\t\tPR\tNI\tVIRT\tRES\tSHR\tS\t%CPU\t%MEM\tTIME+\tCOMMAND");
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                try
                {
                    var pid = process.Id;
                    var user = GetProcessUser(process);
                    var priority = process.PriorityClass.ToString();
                    var niceValue = 0; // .NET doesn't provide nice values directly
                    var virtMemory = process.VirtualMemorySize64 / 1024; // Convert bytes to KB
                    var resMemory = process.PrivateMemorySize64 / 1024;   // Convert bytes to KB
                    var shrMemory = 0; // Not directly available in .NET
                    var status = process.Responding ? "R" : "S"; // Running or Sleeping
                    var cpuUsage = GetCpuUsage(process);
                    var memoryUsage = (resMemory / (double)Environment.WorkingSet) * 100; // %MEM
                    var time = process.TotalProcessorTime.TotalSeconds; // Total CPU time
                    var command = process.ProcessName;

                    Console.WriteLine($"{pid}\t{user}\t{priority}\t{niceValue}\t{virtMemory}\t{resMemory}\t{shrMemory}\t{status}\t{cpuUsage:F1}\t{memoryUsage:F1}\t{time:F1}\t{command}");
                }
                catch (Exception ex)
                {
                   // Console.WriteLine($"Error retrieving process information: {ex.Message}");
                }
            }
        }
        public static void MoveProcessInBackground(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 1 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }
            int processId = int.Parse(Arguments[0]);
            try
            {
                Process process = Process.GetProcessById(processId);
                if (process != null)
                {
                    process.PriorityClass = ProcessPriorityClass.BelowNormal;
                }
                else
                {
                    Settings.ErrorMessage($"Process {processId} not found or already terminated.");
                }
            }
            catch (Exception ex)
            {
                Settings.ErrorMessage($"Error: {ex.Message}");
            }
        }
        public static void MoveProcessToForeground(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 1 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }
            int processId = int.Parse(Arguments[0]);

            try
            {
                Process process = Process.GetProcessById(processId);
                if (process != null)
                {
                    process.PriorityClass = ProcessPriorityClass.Normal;
                }
                else
                {
                    Settings.ErrorMessage($"Process {processId} not found or already terminated.");
                }
            }
            catch (Exception ex)
            {
                Settings.ErrorMessage($"Error: {ex.Message}");
            }
        }

        private static string GetProcessUser(Process process)
        {
            try
            {
                return process.StartInfo.UserName ?? "N/A";
            }
            catch
            {
                return "N/A";
            }
        }

        private static double GetCpuUsage(Process process)
        {
            try
            {
                var totalProcessorTime = process.TotalProcessorTime.TotalMilliseconds;
                var elapsedTime = (DateTime.Now - process.StartTime).TotalMilliseconds;

                if (elapsedTime > 0)
                {
                    return (totalProcessorTime / elapsedTime) * 100;
                }
            }
            catch
            {
                // In case of any exceptions, return 0
            }

            return 0;
        }
    }
}
