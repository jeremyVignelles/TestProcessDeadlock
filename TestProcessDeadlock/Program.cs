using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestProcessDeadlock
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo("ChildProcess.exe")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            proc.EnableRaisingEvents = true;
            if (!proc.Start())
            {
                Console.WriteLine("Failed to start process");
                return;
            }

#if NETCORE
            var memory = new Memory<char>(new char[10]);
            await proc.StandardOutput.ReadAsync(memory, new CancellationTokenSource(100).Token);
#else
            var buffer = new byte[10];
            await proc.StandardOutput.BaseStream.ReadAsync(buffer, 0, 10, new CancellationTokenSource(100).Token);
#endif

            proc.Kill();

            proc.Dispose();
        }
    }
}
