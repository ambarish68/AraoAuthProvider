using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AraoAuthProvider.Data;
using AraoAuthProvider.Models;
using System.Reflection;
using System.Configuration;
using AraoAuthProvider.Services;

namespace AraoAuthProvider
{
    public class Startup
    {
        public const string connectionString = @"Server=localhost;Database=AraoAuthProvider;Trusted_Connection=True;";
        public string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddIdentityServer()
            //.AddDeveloperSigningCredential()
            //.AddTestUsers(Config.GetUsers())
            //// this adds the config data from DB (clients, resources)
            //.AddConfigurationStore(options =>
            //{
            //    options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(connectionString,
            //            sql => sql.MigrationsAssembly(migrationsAssembly));
            //})
            //// this adds the operational data from DB (codes, tokens, consents)
            //.AddOperationalStore(options =>
            //{
            //    options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(connectionString,
            //            sql => sql.MigrationsAssembly(migrationsAssembly));

            //    // this enables automatic token cleanup. this is optional.
            //    options.EnableTokenCleanup = true;
            //    options.TokenCleanupInterval = 30;
            //});

            //    services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(connectionString));

            //    services.AddIdentity<ApplicationUser, IdentityRole>()
            //        .AddEntityFrameworkStores<ApplicationDbContext>()
            //        .AddDefaultTokenProviders();

            //    // Add application services.
            //    services.AddTransient<IEmailSender, EmailSender>();

            //    services.AddMvc();

            //    // configure identity server with in-memory stores, keys, clients and scopes
            //    services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddTestUsers(Config.GetUsers())
            //    // this adds the config data from DB (clients, resources)
            //    .AddConfigurationStore(options =>
            //    {
            //        options.ConfigureDbContext = builder =>
            //            builder.UseSqlServer(connectionString,
            //                sql => sql.MigrationsAssembly(migrationsAssembly));
            //    })
            //    // this adds the operational data from DB (codes, tokens, consents)
            //    .AddOperationalStore(options =>
            //    {
            //        options.ConfigureDbContext = builder =>
            //            builder.UseSqlServer(connectionString,
            //                sql => sql.MigrationsAssembly(migrationsAssembly));

            //        // this enables automatic token cleanup. this is optional.
            //        options.EnableTokenCleanup = true;
            //        options.TokenCleanupInterval = 30;
            //    });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // app.UseAuthentication(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
