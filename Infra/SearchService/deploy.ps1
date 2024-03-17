
$resourceGroup = YourResourceGroup
$search_Service_response = 
    az group deployment create `
    --resource-group $resourceGroup  `
    --template-file .\templates\searchservice.template.json `
    --parameters .\parameters\searchservice.parameter.json 

# set variables
$search_service_name = $search_Service_response.name
$apiKey = $search_Service_response.api_key

# Create data source
$resources = @("categories","orders",'products')

$resources % {
    Add-SearchDataSource `
        -SearchServiceName $search_service_name `
        -ApiKey $apiKey `
        -DataSourceName $dataSourceName `
        -DatabaseConnectionString $connectionString `
        -ContainerName $containerName `
        -Payload $dsPayload
    
    # Create indexer
    Add-SearchIndex `
    -SearchServiceName $search_service_name `
    -ApiKey $apiKey `
    -IndexName $indexName
    
    # Create index
    Add-SearchIndexer 
    -SearchServiceName $search_service_name `
    -ApiKey $apiKey `
    -DataSourceName $dataSourceName `
    -IndexName $indexName `
    -IndexerName $indexerName
}