@description('The location where you want to create the resources.')
param location string = resourceGroup().location

@description('The name of the environment. It will be used to create the name of the resources in the resource group.')
@maxLength(16)
@minLength(3)
param environmentName string = 'sts${uniqueString(resourceGroup().id)}'

//-------------------------------------------------------------
// EventGrid Event Viewer
//-------------------------------------------------------------
var eventViewerAppName =toLower('${environmentName}-eventviewer')
var eventViewerAppPlanName=toLower('${environmentName}-eventviewerplan')
var viewerRepoUrl = 'https://github.com/azure-samples/azure-event-grid-viewer.git'

resource eventViewerAppServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: eventViewerAppPlanName
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    family: 'F'
    capacity: 0
  }
  properties: {}
  kind: 'app'
}

resource eventViewerAppService 'Microsoft.Web/sites@2022-03-01' = {
  name: eventViewerAppName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: eventViewerAppServicePlan.id
    hostNameSslStates: [
      {
        hostType: 'Standard'
        sslState: 'Disabled'
        name: '${eventViewerAppName}.azurewebsites.net'
      }
      {
        hostType: 'Standard'
        sslState: 'Disabled'
        name: '${eventViewerAppName}.scm.azurewebsites.net'
      }
    ]
    siteConfig: {
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

resource eventViewerAppServiceDeploy 'Microsoft.Web/sites/sourcecontrols@2022-03-01' = {
  parent: eventViewerAppService
  name: 'web'
  properties: {
    repoUrl: viewerRepoUrl
    branch: 'main'
    isManualIntegration: true
  }
}

output eventGridViewerSubscriptionEndpoint string ='https://${eventViewerAppService.properties.defaultHostName}/api/updates' 
