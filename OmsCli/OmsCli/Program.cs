using Microsoft.Azure.Management.DataFactory;
using OmsCli;
using OmsCli.AdfManager;
using OmsCli.AzureManager;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using Azure.ResourceManager.DataFactory.Models;
using Azure;

internal class Program
{
    static Response<PipelineCreateRunResult>? pipelineRunId;
    private static void Main(string[] args)
    {
        _ = new
        Settings();

        var levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = LogEventLevel.Verbose
        };

        Log.Logger = new LoggerConfiguration()
             .MinimumLevel.ControlledBy(levelSwitch)
             .WriteTo.Console()
             .WriteTo.File(@"log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Order Management System Menu");
            Console.WriteLine("1. Generate random data and trigger ADF pipeline");
            Console.WriteLine("2. Monitor ADF Pipeline");
            Console.WriteLine("3. Search");
            Console.WriteLine("4. Exit");

            Console.Write("Enter your choice (1-4): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateAndLoadData();
                    break;
                case "2":
                    Monitor();
                    break;
                case "3":
                    Search();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private static void Search()
    {
        SearchManager.Search("products-index");
    }

    private static void Monitor()
    {
        AdfManager.MonitorPipeline(pipelineRunId.Value.RunId.ToString(), AdfManager.PIPELINE);
    }

    private static void GenerateAndLoadData()
    {
        DataGenerator dg = new DataGenerator(1000);
        dg.Generate();
        pipelineRunId = AdfManager.TriggerPipeline(AdfManager.PIPELINE, null);
    }
}