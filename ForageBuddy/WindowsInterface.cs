using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ForageBuddy
{
    public static class WindowsInterface
    {
        public static IEnumerable<(string clientName, IntPtr clientHandle)> GetOpenPuzzlePiratesClients()
        {
            foreach (var process in Process.GetProcesses())
                if (process.ProcessName.Contains("Puzzle Pirates -") &&
                    process.ProcessName.Contains(" on the ") &&
                    process.ProcessName.Contains("Ocean"))
                    yield return (process.ProcessName, process.Handle);
        }
    }
}
