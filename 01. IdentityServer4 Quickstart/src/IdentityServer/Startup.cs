// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
//using System.Configuration;
using System.Threading.Tasks;
using System.Configuration;
using IdentityServer4.Configuration;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());

            services.AddAuthentication()
                .AddOpenIdConnect("azuread", "Azure AD", options => Configuration.Bind("AzureAd", options));
            services.Configure<OpenIdConnectOptions>("azuread", options =>
            {
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Events = new OpenIdConnectEvents()
                {
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Account/Logout");
                        return Task.FromResult(0);
                    }
                };
            });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
