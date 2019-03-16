using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReversiApi.Dal;
using ReversiApi.Models;

namespace ReversiApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Session settings
            services.AddSession();
            services.AddMemoryCache();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme
                ).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                    //expire after 15 minuten of inactivity
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(900);


                });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
           
            //add mvc to application
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //set databases for migrations
            services.AddDbContext<GameContext>(options => options.UseSqlServer(Configuration.GetSection("CustomConfig").GetValue<String>("ConnectionString")));
            services.AddDbContext<PlayerContext>(options => options.UseSqlServer(Configuration.GetSection("CustomConfig").GetValue<String>("ConnectionString")));
            services.AddDbContext<ScoreContext>(options => options.UseSqlServer(Configuration.GetSection("CustomConfig").GetValue<String>("ConnectionString")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();


        }
    }
}
