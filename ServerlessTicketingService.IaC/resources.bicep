@description('The location wher you want to create the resources.')
param location string = resourceGroup().location

@description('The name of the environment. It will be used to create the name of the resources in the resource group.')
@maxLength(16)
@minLength(3)
param environmentName string = 'sts${uniqueString(resourceGroup().id)}'

@description('EventGrid viewer web site endopint for event grid subscription')
param eventGridViewerEndpointUrl string

var keyVaultName = toLower('${environmentName}-kv')

//-------------------------------------------------------------
// EventGrid Event Viewer Subscription
//-------------------------------------------------------------
var eventViewerSubName=toLower('${environmentName}-eventviewersub')

resource eventViewerSubscription 'Microsoft.EventGrid/eventSubscriptions@2022-06-15' = {
  name: eventViewerSubName
  scope: eventGridTopic
  properties: {
    destination: {
      endpointType: 'WebHook'
      properties: {
        endpointUrl: eventGridViewerEndpointUrl
      }
    }
  }
}

//-------------------------------------------------------------
// Data Storage
//-------------------------------------------------------------
var dataStorageAccountName = toLower('${environmentName}dstore')

resource dataStorageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: dataStorageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

resource dataTable 'Microsoft.Storage/storageAccounts/tableServices/tables@2022-05-01'= { 
  name: '${dataStorageAccount.name}/default/Tickets'
  properties:{
    
  }
}

//-------------------------------------------------------------
// Azure Functions resources
//-------------------------------------------------------------
var functionAppStorageAccountName = toLower('${environmentName}appstore')
var funcHostingPlanName = toLower('${environmentName}-plan')
var functionAppName = toLower('${environmentName}-func')
var applicationInsightsName = toLower('${environmentName}-ai')

resource functionAppStorageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: functionAppStorageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

resource funcHostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: funcHostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: funcHostingPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionAppStorageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionAppStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionAppStorageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionAppStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_NODE_DEFAULT_VERSION'
          value: '~10'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'TicketStorageConnection'
          value: 'DefaultEndpointsProtocol=https;AccountName=${dataStorageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${dataStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'TopicEndpoint'
          value: eventGridTopic.properties.endpoint
        }
        {
          name: 'TopicKey'
          value: eventGridTopic.listKeys().key1
        }
      ]
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

//-------------------------------------------------------------
// Notification topic
//-------------------------------------------------------------
var eventGridTopicName = toLower('${environmentName}-topic')

resource eventGridTopic 'Microsoft.EventGrid/topics@2022-06-15' = {
  name: eventGridTopicName
  location: location
}
