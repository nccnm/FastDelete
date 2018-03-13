using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"Please enter folder name that you want to delete. Example: C:\example");
                Console.ReadLine();

                return;
            }

            DeleteFolder(args[0]);
        }

        static void DeleteFolder(string foldername)
        {
            var command1 = $"del /f/s/q \"{foldername}\" > nul";
            var command2 = $"rmdir /s/q \"{foldername}\"";

            if (ExecuteCommand(command1) && ExecuteCommand(command2))
                Console.WriteLine($"Delete folder \"{foldername}\" successfully.");
        }

        static bool ExecuteCommand(string command)
        {
            var isSuccess = true;
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (!string.IsNullOrEmpty(e.Data)) Console.WriteLine("output>>" + e.Data);
            };

            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    isSuccess = false;
                    Console.WriteLine("error>>" + e.Data);
                }
            };

            process.BeginErrorReadLine();
            process.WaitForExit(); 
            process.Close();

            return isSuccess;
        }
    }
}
