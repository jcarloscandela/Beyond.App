using Scalar.AspNetCore;
using Beyond.Todo.Application;
using Beyond.Todo.Infrastructure;
using Beyond.Todo.SignalR;
using Beyond.Todo.SignalR.Hubs;

namespace Beyond.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.AddSignalRServices();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:5173", "https://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<TodoHub>("/hubs/todo");

            app.Run();
        }
    }
}
