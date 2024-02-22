using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDoWebProject;
using BlazorStrap;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient(){BaseAddress = 
    new Uri("https://localhost:7106/api")});
builder.Services.AddBlazorStrap();
builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();

