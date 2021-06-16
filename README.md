# BlazorBlobStorage

## Open the Azure Portal

- Go to https://portal.azure.com/
- Login with your account
- Open the Azure cli (by clicking on the icon that looks like a command prompt) and make sure that Bash is selected
- Type the following commands

## Create Resource Group, Storage Account

```
subscriptionid=$(az account show --query id -o tsv)
az group create --name simodemo01-rg --location westeurope
az storage account create --name simodemo01sa --resource-group simodemo01-rg
```

## Assign Rights to User

```
az ad signed-in-user show --query objectId -o tsv | az role assignment create --role "Storage Blob Data Contributor" --assignee @- --scope "/subscriptions/$subscriptionid/resourceGroups/simodemo01-rg/providers/Microsoft.Storage/storageAccounts/simodemo01sa"
```

## Create Blob Container

```
az storage container create --account-name simodemo01sa --name demo --auth-mode login
```

## Run the application from your machine

- Clone this repo on your machine.
- Open Visual Studio
- Log on to Azure from within Visual Studio.
- Run it from Visual Studio
- Go to the Upload page
- Select an image and upload it

## Run the application from Azure

In the Azure CLI, type the following commands

```
mkdir demo
cd $HOME/demo

git clone https://github.com/scolapicchioni/BlazorBlobStorage.git
az webapp up --resource-group simodemo01-rg --runtime "DOTNET|5.0" --location westeurope --sku B1 --name simodemo01wa --launch-browser --logs
```
