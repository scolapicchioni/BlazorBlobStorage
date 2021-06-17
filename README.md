# BlazorBlobStorage

## Open the Azure Portal

- Go to https://portal.azure.com/
- Login with your account
- Open the Azure cli (by clicking on the icon that looks like a command prompt) and make sure that Bash is selected
- Type the following commands


```
# If you change the storageaccount, you have to change the appsettings.json of the Server application 
resourcegroup=simodemo01-rg
storageaccount=simodemo01sa
serviceplan=simodemo01-asp
webappname=simodemo01wa
gitrepo=https://github.com/scolapicchioni/BlazorBlobStorage

# Create Resource Group, Storage Account
subscriptionid=$(az account show --query id -o tsv)
az group create --name $resourcegroup --location westeurope
az storage account create --name $storageaccount --resource-group $resourcegroup

# Assign Rights to User
az ad signed-in-user show --query objectId -o tsv | az role assignment create --role "Storage Blob Data Contributor" --assignee @- --scope "/subscriptions/$subscriptionid/resourceGroups/$resourcegroup/providers/Microsoft.Storage/storageAccounts/$storageaccount"

# Create Blob Container
az storage container create --account-name $storageaccount --name demo --auth-mode login

```

## Run the application from your machine

- Clone this repo on your machine.
- Open Visual Studio
- Log on to Azure from within Visual Studio.
- Run it from Visual Studio
- Go to the Upload page
- Select an image and upload it

## Deploy

## Option 1

```
# Create an App Service plan in `FREE` tier.
az appservice plan create --name $serviceplan --resource-group $resourcegroup --sku FREE

# Create a web app.
az webapp create --name $webappname --resource-group $resourcegroup --plan $serviceplan

# Deploy code from a public GitHub repository. 
az webapp deployment source config --name $webappname --resource-group $resourcegroup --repo-url $gitrepo --branch master --manual-integration

```

## Alternative

```
mkdir demo
cd $HOME/demo

git clone https://github.com/scolapicchioni/BlazorBlobStorage.git
cd BlazorBlobStorage/BlazorBlobStorage
az webapp up --resource-group $resourcegroup --runtime "DOTNET|5.0" --location westeurope --sku B1 --name $webappname --launch-browser --logs
```

## Permissions

```
# Copy the result of the following command into a browser to see the web app.
echo http://$webappname.azurewebsites.net

# Assign identity to webapp
az webapp identity assign --name $webappname --resource-group $resourcegroup

# Assign role to app
spID=$(az resource list -n $webappname --query [*].identity.principalId --out tsv)

storageId=$(az storage account show -n $storageaccount -g $resourcegroup --query id --out tsv)

az role assignment create --assignee $spID --role 'Storage Blob Data Contributor' --scope $storageId
```
