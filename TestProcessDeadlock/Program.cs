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

var cts = new CancellationTokenSource(100);

cts.Token.Register(() => {
    Console.WriteLine("Cancellation token canceled");
});

var memory = new Memory<char>(new char[10]);
Console.WriteLine("Start reading");
await proc.StandardOutput.ReadAsync(memory, cts.Token);
Console.WriteLine("Read unlocked");

proc.Kill();

proc.Dispose();