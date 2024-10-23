using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Shell
{
    public static class Networking
    {
        public static void DisplayNetworkInterfaces(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 0 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }
            Console.WriteLine("Network Interfaces:\n");

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                Console.WriteLine($"Interface: {networkInterface.Name}");
                Console.WriteLine($"  Description: {networkInterface.Description}");
                Console.WriteLine($"  Status: {networkInterface.OperationalStatus}");
                Console.WriteLine($"  Type: {networkInterface.NetworkInterfaceType}");
                Console.WriteLine($"  Speed: {networkInterface.Speed / 1_000_000} Mbps");
                Console.WriteLine($"  MAC Address: {networkInterface.GetPhysicalAddress()}");
                Console.WriteLine($"  IP Addresses:");

                var ipProps = networkInterface.GetIPProperties();
                foreach (var ip in ipProps.UnicastAddresses)
                {
                    Console.WriteLine($"    - {ip.Address}");
                }

                Console.WriteLine();
            }
        }
        public static void DisplayNetworkConnections(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 0 || Flags.Count > 1)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }
            bool showTcp = true;
            bool showUdp = true;
            if (Flags.Count == 1 && Flags[0] == "-t") showUdp = false;
            else if (Flags.Count == 1 && Flags[0] == "-u")showTcp = false;
            else if (Flags.Count == 1)
            {
                Settings.ErrorMessage("Error: not vaild flags!!");
                return;
            }

            Console.WriteLine("Active Network Connections:\n");
            Console.WriteLine("Proto\tLocal Address\t\tForeign Address\t\tState");

            // Get TCP connections
            if (showTcp)
            {
                var tcpConnections = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();

                foreach (var conn in tcpConnections)
                {
                    string localAddress = conn.LocalEndPoint.Address.ToString();
                    string foreignAddress = conn.RemoteEndPoint.Address.ToString();
                    Console.WriteLine($"TCP\t{localAddress}\t{foreignAddress}\t{conn.State}");
                }
            }

            // Get UDP connections
            if (showUdp)
            {
                var udpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners();

                foreach (var listener in udpListeners)
                {
                    string localAddress =  listener.Address.ToString();
                    Console.WriteLine($"UDP\t{localAddress}\t\t*\t\tLISTENING");
                }
            }
        }
        public static void PingHost(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 1 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }
            string hostname = Arguments[0];
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(hostname);

                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine($"Ping to {hostname} successful.");
                        Console.WriteLine($"Response Time: {reply.RoundtripTime} ms");
                        Console.WriteLine($"IP Address: {reply.Address}");
                    }
                    else
                    {
                        Settings.ErrorMessage($"Ping to {hostname} failed. Status: {reply.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Settings.ErrorMessage($"Error: {ex.Message}");
            }
        }
        public static void ConnectToHost(List<string> Flags, List<string> Arguments)
        {
            if (Arguments.Count != 3 || Flags.Count != 0)
            {
                Settings.ErrorMessage("Error: not valid arguments or flags.");
                return;
            }
            string host = Arguments[0];
            string username = Arguments[1];
            string password = Arguments[2];
            using (var client = new SshClient(host, username, password))
            {
                try
                {
                    client.Connect();
                    Console.WriteLine("Connection established.");
                    var command = client.CreateCommand("ls -l");
                    var result = command.Execute();
                    Console.WriteLine(result);

                    client.Disconnect();
                    Console.WriteLine("Connection closed.");
                }
                catch (Exception ex)
                {
                    Settings.ErrorMessage($"Error: {ex.Message}");
                }
            }
        }
    }
}
