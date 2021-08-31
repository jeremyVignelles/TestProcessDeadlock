using System;
using System.Diagnostics;
using System.Threading;


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

var memory = new Memory<char>(new char[10]);
await proc.StandardOutput.ReadAsync(memory, new CancellationTokenSource(100).Token);

proc.Kill();

proc.Dispose();