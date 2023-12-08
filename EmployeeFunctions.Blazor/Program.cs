using EmployeeFunctions.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7184/") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://azureemployeecrud.azurewebsites.net") });

await builder.Build().RunAsync();
