using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EXAM2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            EnsureDirectories();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileMoverService()
            };
            ServiceBase.Run(ServicesToRun);
        }

        static void EnsureDirectories()
        {
            if (!Directory.Exists(@"C:\Folder1"))
            {
                Directory.CreateDirectory(@"C:\Folder1");
            }

            if (!Directory.Exists(@"C:\Folder2"))
            {
                Directory.CreateDirectory(@"C:\Folder2");
            }

            if (!Directory.Exists(@"C:\Logs"))
            {
                Directory.CreateDirectory(@"C:\Logs");
            }
        }
    }
}
