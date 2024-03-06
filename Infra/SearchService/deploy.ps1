$resourceGroup = YourResourceGroup
az group deployment create `
    --resource-group $resourceGroup  `
    --template-file searchService.template.json `
    --parameters @searchService.parameters.json 

az group deployment create `
--resource-group $resourceGroup `
--template-file dataSource.template.json `
--parameters @dataSource.parameters.json

az group deployment create `
--resource-group $resourceGroup `
--template-file indexer.template.json `
--parameters @indexer.parameters.json


az group deployment create `
    --resource-group $resourceGroup `
    --template-file index.template.json `
    --parameters @index.parameters.json 
