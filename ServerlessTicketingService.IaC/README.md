# Serverless Ticketing Service - IaC

## Complete deploying

This project contains the bicep templates you can use to create the environment to host the Serverless Ticketing Service project.

To create the resource group and to deploy the resources you need for the project, simply run the following command:

```
az deployment sub create --location <your region> --template-file main.bicep
```

where 
- `<your region>` is the location where you want to create the deployment


You can also set these parameters:

- `location` : the location you want to deploy (by default the location is the same of your deployment)
- `resourceGroupName` : the name of the resource group (by default its value is `ServerlessTicketingService-rg`)
- `environmentName` : the prefix used to create all the resources in the resource group. By default it is setted to 'sts' with a unique value.

```
az deployment sub create --location <your region> --template-file main.bicep --parameters location=<location to deploy> resourceGroupName=<rg name> environmentName=<env name>
```

## Deploying only the Event Grid Viewer

To deploy only the Event Grid Viewer web site run the following command:

```
az deployment group create --resource-group <resource group name> --template-file eventGridViewer.bicep
```

where 
- `<your region>` is the location where you want to create the deployment
- `<resource group name>` is the name of the resource group in which you want to deploy the event grid viewer

You can also set these parameters:

- `location` : the location you want to deploy (by default the location is the location of the resourcegroup)
- `environmentName` : the prefix used to create all the resources in the resource group. By default it is setted to 'sts' with a unique value.

```
az deployment sub create --resource-group <resource group name> --template-file main.bicep --parameters location=<location to deploy> environmentName=<env name>
```
