function Add-SearchIndex {
    param (
        [string]$SearchServiceName,
        [string]$ApiKey,
        [string]$IndexName,
        [string]$ApiVersion = "2023-11-01"
    )

    $uri = "https://$SearchServiceName.search.windows.net/indexes?api-version=$ApiVersion"

    $headers = @{
        "Content-Type" = "application/json"
        "api-key" = $ApiKey
    }

    $jsonPayload = @{
        "name" = $IndexName
        # Add other properties as needed
    }

    $jsonString = $jsonPayload | ConvertTo-Json

    $response = Invoke-RestMethod -Uri $uri -Headers $headers -Body $jsonString -Method Post

    return $response
}