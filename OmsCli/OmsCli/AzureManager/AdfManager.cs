using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.DataFactory;
using Azure.ResourceManager.DataFactory.Models;
using Azure.ResourceManager.Resources;
using System;
using System.Collections.Generic;

namespace OmsCli.AdfManager
{
    public static class AdfManager
    {
        #region Azure parameters
        private static readonly Settings setting = new Settings();

        // Set variables
        private static readonly string tenantID = setting.TenantId;

        /// <summary>
        /// your application ID
        /// </summary>
        private static readonly string applicationId = setting.ApplicationId;
        /// <summary>
        /// authentication key for the application
        /// </summary>
        private static readonly string authenticationKey = setting.ApplicationSecret;

        /// <summary>
        /// your subscription ID where the data factory resides
        /// </summary>
        private static readonly string subscriptionId = setting.SubscriptionId;

        /// <summary>
        /// your resource group where the data factory resides
        /// </summary>
        private static readonly string resourceGroupName = setting.ResourceGroup;

        /// <summary>
        /// specify the name of data factory to create. It must be globally unique
        /// </summary>
        private static readonly string dataFactoryName = "omsadf-learning";


        public const string PIPELINE = "RandomData";

        #endregion

        public static Response<PipelineCreateRunResult> TriggerPipeline(string pipelineName, IDictionary<string, BinaryData>? parameters)
        {
            try {
                Console.WriteLine($"Reading datafactory: {dataFactoryName}");
                var dataFactoryResource = GetDataFactory();

                // Create a pipeline run
                Console.WriteLine("Creating pipeline run...");
                var pipelineResource = dataFactoryResource.GetDataFactoryPipeline(pipelineName);

                Console.WriteLine($"Starting pipeline run: {pipelineName}");
                var runResponse = pipelineResource.Value.CreateRun(parameters);
                Console.WriteLine("Pipeline run ID: " + runResponse.Value.RunId);

                return runResponse;
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Pipeline trigger failed - error details: {ex.Message}", ex);
            }
        }

        public static void MonitorPipeline(string pipelineRunId, string pipelineName)
        {
            try
            {
                Console.WriteLine($"Reading datafactory: {dataFactoryName}");
                var dataFactoryResource = GetDataFactory();


                // Monitor the pipeline run
                Console.WriteLine($"Checking pipeline ${pipelineName} run status...");
                DataFactoryPipelineRunInfo pipelineRun;
                while (true)
                {
                    pipelineRun = dataFactoryResource.GetPipelineRun(pipelineRunId);
                    Console.WriteLine($"Status: {pipelineRun.Status}");
                    if (pipelineRun.Status == "InProgress" || pipelineRun.Status == "Queued")
                    {
                        Console.WriteLine($"Sleeping for 15 seconds,  pipeline status {pipelineRun.Status}");
                        System.Threading.Thread.Sleep(15000);
                    }
                    else
                    {
                        Console.WriteLine($"Pipeline completed successfully. Status {pipelineRun.Status}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Monitoring failed - error details: {ex.Message}", ex);
            }
        }

        private static ArmClient ObtainArmClient()
        {
            ArmClient armClient = new ArmClient(
                new ClientSecretCredential(tenantID, applicationId, authenticationKey, new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                }),
                subscriptionId,
                new ArmClientOptions { Environment = ArmEnvironment.AzurePublicCloud }
            );
            return armClient;
        }

        private static SubscriptionResource ObtainSubscriptionResource(ArmClient armClient) {
            ResourceIdentifier resourceIdentifier = SubscriptionResource.CreateResourceIdentifier(subscriptionId);
            SubscriptionResource subscriptionResource = armClient.GetSubscriptionResource(resourceIdentifier);
            return subscriptionResource;
        }

        private static ResourceGroupResource GetResource(SubscriptionResource subscriptionResource) {
            Console.WriteLine("Get an existing resource group " + resourceGroupName + "...");
            var resourceGroupOperation = subscriptionResource.GetResourceGroup(resourceGroupName);

            return resourceGroupOperation.Value;
        }

        private static DataFactoryResource GetDataFactory()
        {
            try
            {
                ArmClient armClient = ObtainArmClient();
                SubscriptionResource subscriptionResource = ObtainSubscriptionResource(armClient);
                var resourceGroupResource = GetResource(subscriptionResource);

                Console.WriteLine("Get Data Factory " + dataFactoryName + "...");
                var dataFactoryOperation = resourceGroupResource.GetDataFactory(dataFactoryName);
                return dataFactoryOperation.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
                throw;
            }
        }
    }

}
