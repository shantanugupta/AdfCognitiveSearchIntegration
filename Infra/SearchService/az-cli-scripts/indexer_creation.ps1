function Add-SearchIndexer {
    param (
        [string]$SearchServiceName,
        [string]$ApiKey,
        [string]$DataSourceName,
        [string]$IndexName,
        [string]$IndexerName,
        [string]$ApiVersion = "2023-11-01"
    )

    $uri = "https://$SearchServiceName.search.windows.net/indexers?api-version=$ApiVersion"

    $headers = @{
        "Content-Type" = "application/json"
        "api-key" = $ApiKey
    }

    $jsonPayload = @{
        "name" = $IndexerName
        "dataSourceName" = $DataSourceName
        "targetIndexName" = $IndexName
        # Add other properties as needed
    }

    $jsonString = $jsonPayload | ConvertTo-Json

    $response = Invoke-RestMethod -Uri $uri -Headers $headers -Body $jsonString -Method Post

    return $response
}