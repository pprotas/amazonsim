using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//Need this part!!
using Microsoft.EntityFrameworkCore;

using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Views;
using Controllers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace TestAPI
{
    public class Startup
    {
        public static SimulationController simulationController;

        public Startup(IConfiguration configuration)
        {
            simulationController = new SimulationController(new Models.World());

            Thread InstanceCaller = new Thread(
                new ThreadStart(simulationController.Simulate));

            // Start the thread.
            InstanceCaller.Start();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //They call this dependency injection
            // services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".mtl"] = "text/plain";
            provider.Mappings[".obj"] = "text/plain";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/connect_client")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                        ClientView cs = new ClientView(webSocket);
                        simulationController.AddView(cs);
                        await cs.StartReceiving();
                        simulationController.RemoveView(cs);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseMvc();

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions());
        }
    }
}
