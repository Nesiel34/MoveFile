using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            string directoryPath = @"D:\Libraries\Desktop\files\WebPage\obj\Host\bin";

            // Create a new FileSystemWatcher and set its properties
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = directoryPath;

            // Watch for changes in LastWrite and CreationTime
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Only watch text files.
            watcher.Filter = "*.txt";

            // Add event handlers for specific events
            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            // Begin watching
            watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }
        void OnCreated(object sender, FileSystemEventArgs e)
        {

            string move = @"D:\Libraries\Desktop\newDest\" + e.Name;
            File.Move(e.FullPath, move, true);
            File.SetAttributes(move, FileAttributes.Normal);

            Console.WriteLine($"File Created: {e.FullPath}");
        }

        void OnChanged(object sender, FileSystemEventArgs e)
        {

            Console.WriteLine($"File Changed: {e.FullPath}");
        }

        void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File Deleted: {e.FullPath}");
        }

        void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"File Renamed: {e.OldFullPath} to {e.FullPath}");
        }


        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);


            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
