using Beyond.Todo.WebApp.Client.Configuration;
using Beyond.Todo.WebApp.Client.Services;
using Beyond.Todo.WebApp.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Beyond.Todo.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddScoped<IHttpResponseHandler, HttpResponseHandler>();

            var config = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

            builder.Services.AddHttpClient("TodoApi", client =>
            {
                client.BaseAddress = new Uri(config.BaseUrl);
            });
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("TodoApi"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
