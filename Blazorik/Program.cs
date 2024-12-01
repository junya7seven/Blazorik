using Blazored.LocalStorage;
using Blazorik;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7231/api/"); 
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
await builder.Build().RunAsync();
