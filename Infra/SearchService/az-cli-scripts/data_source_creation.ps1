function Add-SearchDataSource {
    param (
        [string]$SearchServiceName,
        [string]$ApiKey,
        [string]$DataSourceName,
        [string]$DatabaseConnectionString,
        [string]$ContainerName,
        [string]$DataSourceType = "azuresql",
        [string]$ApiVersion = "2023-11-01"
    )

    $uri = "https://$SearchServiceName.search.windows.net/datasources?api-version=$ApiVersion"

    $headers = @{
        "Content-Type" = "application/json"
        "api-key" = $ApiKey
    }

    $jsonPayload = @{
        "name" = $DataSourceName
        "type" = $DataSourceType
        "credentials" = @{
            "connectionString" = $DatabaseConnectionString
        }
        "container" = @{
            "name" = $ContainerName
        }
    }

    $jsonString = $jsonPayload | ConvertTo-Json

    $response = Invoke-RestMethod -Uri $uri -Headers $headers -Body $jsonString -Method Post

    return $response
}