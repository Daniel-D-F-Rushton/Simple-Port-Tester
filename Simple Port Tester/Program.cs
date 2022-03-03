using System;
using System.Net.Sockets;

namespace Simple_Port_Tester
{
    class Program
    {
        static private int Port;
        static private int Timeout;
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                // No user arguments, return help info.
                case 0:
                    HelpInfo();
                    return;

                case 1:
                    // User asked for help
                    if (args[0] == "-h" || args[0] == "/h")
                    {
                        HelpInfo();
                        return;
                    }
                    // User input argument, let's see if the host is open. 
                    // If this was anything important I would error check... but it is not.
                    if (PortScan(args[0])) Console.WriteLine("Port 80 on host " + args[0] + " is open!");
                    else Console.WriteLine("Port 80 on host " + args[0] + " is closed!");
                    break;

                case 2:
                    // Make sure that the second input argument is an actual number
                    // Again could error check better, but if someone wants to port check on 99999 good luck to them
                    // In reality the TCPClient throws anything that is poor so will just give a closed message
                    if (!int.TryParse(args[1], out Port))
                    {
                        Console.WriteLine("Sorry but that port doesn't appear to be valid.");
                        return;
                    }
                    // If it was a number, let's try that port.
                    if (PortScan(args[0], Port)) Console.WriteLine("Port " + Port + " on host " + args[0] + " is open!");
                    else Console.WriteLine("Port " + Port + " on host " + args[0] + " is closed!");
                    break;

                case 3:
                    // Check the port is a real number
                    if (!int.TryParse(args[1], out Port))
                    {
                        Console.WriteLine("That port doesn't appear to be valid");
                        return;
                    }
                    // Check the timeout is a real number
                    if (!int.TryParse(args[2], out Timeout))
                    {
                        Console.WriteLine("That timeout doesn't appear to be valid");
                        return;
                    }
                    // Do the thing
                    if (PortScan(args[0], Port, Timeout)) Console.WriteLine("Port " + Port + " on host " + args[0] + " is open!");
                    else Console.WriteLine("Port " + Port + " on host " + args[0] + " is closed!");
                    break;

                default:
                    // If some reason there are a random number of arguments.
                    HelpInfo();
                    break;
            }
        }

        /// <summary>
        /// Just the help dialogue if it is needed.
        /// </summary>
        static private void HelpInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Simple Port Tester, by Daniel D F Rushton");
            Console.WriteLine();
            Console.WriteLine("Usage Port-Tester.exe,    ... gives you this");
            Console.WriteLine("      Port-Tester.exe -h, ... gives you this");
            Console.WriteLine("      Port-Tester.exe /h, ... gives you this");
            Console.WriteLine();
            Console.WriteLine("Port-Tester.exe [host] [port - default 80] [timeout - default 250]");
            Console.WriteLine("Port-Tester.exe google.com         ... tests google.com on port 80");
            Console.WriteLine("Port-Tester.exe google.com 135     ... tests google.com on port 135");
            Console.WriteLine("Port-Tester.exe google.com 135 200 ... tests google.com on port 135 with a 200ms timeout");
        }


        /// <summary>
        /// No thrills port scanner that takes a host/ip, port, and optional timeout and reports whether or not the
        /// port responds within the timeout stated.
        /// </summary>
        /// <param name="ip">This is the IP or Host of the target device</param>
        /// <param name="port">This is the port to be checked if open</param>
        /// <param name="timeout">This is how long to wait for a response</param>
        /// <returns>True if the port is open, false if the port is closed or times out</returns>
        static private bool PortScan(string ip, int port = 80, int timeout = 250)
        {
            using (TcpClient Scan = new TcpClient())
            {
                try
                {
                    var result = Scan.BeginConnect(ip, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeout));
                    if (!success) return false;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
