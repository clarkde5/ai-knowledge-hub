using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBlazorApp;
using Microsoft.SemanticKernel;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(sp => KernelPluginFactory.CreateFromType<ThemePlugin>(serviceProvider: sp));

builder.Services.AddKernel();
var aiConfig = builder.Configuration.GetSection("SmartComponents");
builder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: aiConfig["DeploymentName"]!,
    endpoint: aiConfig["Endpoint"]!,
    apiKey: aiConfig["ApiKey"]!);

await builder.Build().RunAsync();
