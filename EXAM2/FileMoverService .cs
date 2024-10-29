using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EXAM2
{
    public partial class FileMoverService : ServiceBase
    {
        private FileSystemWatcher _fileWatcher;
        private const string Folder1 = @"C:\Folder1";
        private const string Folder2 = @"C:\Folder2";
        private const string LogFilePath = @"C:\Logs\FileMoverService.log";

        public FileMoverService()
        {
            InitializeComponent();
            _fileWatcher = new FileSystemWatcher(Folder1);
            _fileWatcher.Created += OnFileCreated;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            int maxRetries = 3;
            int delayBetweenRetries = 500; 

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    string destFile = Path.Combine(Folder2, Path.GetFileName(e.FullPath));

                    File.Move(e.FullPath, destFile);

                    LogEvent($"File '{e.Name}' was moved to '{Folder2}'.");
                    break; 
                }
                catch (IOException ioEx)
                {
                    if (attempt == maxRetries - 1)
                    {
                        LogEvent($"Error moving file '{e.Name}': {ioEx.Message}");
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(delayBetweenRetries);
                    }
                }
                catch (Exception ex)
                {
                    LogEvent($"Unexpected error moving file '{e.Name}': {ex.Message}");
                    break;
                }
            }
        }
        private void LogEvent(string message)
        {
            EventLog.WriteEntry("FileMoverService", message, EventLogEntryType.Information);
            LogToFile(message);
        }
        private void LogToFile(string message)
        {
            using (StreamWriter sw = new StreamWriter(LogFilePath, true))
            {
                sw.WriteLine($"{DateTime.Now:G}: {message}");
            }
        }

        protected override void OnStart(string[] args)
        {
            if (!EventLog.SourceExists("FileMoverService"))
            {
                EventLog.CreateEventSource("FileMoverService", "Application");
            }
            EventLog.Source = "FileMoverService";
            EventLog.Log = "Application";

            LogEvent("Service started");
        }

        protected override void OnStop()
        {
            _fileWatcher.EnableRaisingEvents = false;
            LogEvent("Service stopped");
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
