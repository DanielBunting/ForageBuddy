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
                if (process.MainWindowTitle.Contains("Puzzle Pirates -") &&
                    process.MainWindowTitle.Contains(" on the ") &&
                    process.MainWindowTitle.Contains("ocean"))
                    yield return (process.MainWindowTitle.ToShortClientName(), process.Handle);
        }

        private static string ToShortClientName(this string clientName)
        {
            var segmentedName = clientName.Split('-')[1].Split("on the");

            return $"{segmentedName[0].Trim()} - {segmentedName[1].Trim().Split(' ')[0]}";
        }
    }
}
