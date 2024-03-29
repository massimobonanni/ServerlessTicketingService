targetScope = 'subscription'

@description('The name of the resource group that contains all the resources')
param resourceGroupName string = 'ServerlessTicketingService-rg'

@description('The name of the environment. It will be used to create the name of the resources in the resource group.')
@maxLength(16)
@minLength(3)
param environmentName string = 'sts${uniqueString(subscription().id)}'

@description('The location of the resource group and resources')
param location string = deployment().location

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: location
}

module eventGridViewer 'eventGridViewer.bicep' = {
  scope: resourceGroup
  name: 'eventGridViewer'
  params: {
    location: location
    environmentName: environmentName
  }
}

module resourcesModule 'resources.bicep' = {
  scope: resourceGroup
  name: 'resources'
  params: {
    location: location
    environmentName: environmentName
    eventGridViewerEndpointUrl: eventGridViewer.outputs.eventGridViewerSubscriptionEndpoint
  }
}
