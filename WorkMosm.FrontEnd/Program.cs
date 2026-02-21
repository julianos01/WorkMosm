using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WorkMosm.FrontEnd;
using WorkMosm.FrontEnd.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
    return new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl!)
    };
});

builder.Services.AddFrontEndServices(builder.Configuration);

await builder.Build().RunAsync();
