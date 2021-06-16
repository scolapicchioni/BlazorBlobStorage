using Azure.Identity;
using Azure.Storage;
using BlazorBlobStorage.Server.Core;
using BlazorBlobStorage.Server.Core.Interfaces;
using BlazorBlobStorage.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace BlazorBlobStorage.Server {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllersWithViews();
            services.AddRazorPages();
            //https://devblogs.microsoft.com/azure-sdk/best-practices-for-using-azure-sdk-with-asp-net-core/
            //https://docs.microsoft.com/en-us/dotnet/azure/sdk/dependency-injection

/*
subscriptionid=$(az account show --query id -o tsv)
az group create --name simodemo01-rg --location westeurope
az storage account create --name simodemo01sa --resource-group simodemo01-rg
az ad signed-in-user show --query objectId -o tsv | az role assignment create --role "Storage Blob Data Contributor" --assignee @- --scope "/subscriptions/$subscriptionid/resourceGroups/simodemo01-rg/providers/Microsoft.Storage/storageAccounts/simodemo01sa"
az storage container create --account-name simodemo01sa --name demo --auth-mode login
*/

services.AddAzureClients(builder => {
    // Add a KeyVault client
    //builder.AddSecretClient(Configuration.GetSection("KeyVault"));

    // Add a storage account client
    builder.AddBlobServiceClient(Configuration.GetSection("Storage"));//.WithCredential(new DefaultAzureCredential());

    // Use the environment credential by default
    builder.UseCredential(new DefaultAzureCredential());
});
services.AddScoped<IPicturesService, PicturesService>();
services.AddScoped<IPicturesRepository, BlobStoragePicturesRepository>();

}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
if (env.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});
}
}
}
