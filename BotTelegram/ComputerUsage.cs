using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTelegram
{
    class ComputerUsage
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        public ComputerUsage()
        {
            

            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }
        public string getCurrentCpuUsage()
        {
            return cpuCounter.NextValue() + "%";
        }

        public string getAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }
        public string shutdown()
        {
            Process.Start("shutdown", "/s /t 60");
            return "Wird heruntergefahren...";
        }
        public string restart()
        {
            Process.Start("shutdown", "/r");
            return "Wird neugestartet...";
        }
        public string lockWorkstation()
        {
            Process.Start("rundll32.exe", "user32.dll,LockWorkStation");
            return "Workstation gesperrt";
        }
    }
}
