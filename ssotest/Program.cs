
using Microsoft.Extensions.Configuration;
using ssoCommon;
using System.Runtime.CompilerServices;

namespace ssoCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();

            //builder.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.
            //    options.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
            //    options.InstanceName = "SampleInstance";
            //});

            builder.Services.AddSession(a => { });
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddCors(option =>
            option.AddDefaultPolicy(policy =>
                policy.WithOrigins(
                                   "http://*.ssocenter.com:11001",
                                   "http://*.atest.com:11002",
                                   "http://*.btest.com:11003",
                                   "http://*.ctest.com:11004",
                                   "http://*.lssoCenter.com:5220",
                                   "http://*.latest.com:5221",

                                   "http://ssocenter.com:11001",
                                   "http://atest.com:11002",
                                   "http://btest.com:11003",
                                   "http://ctest.com:11004",

                                   "http://lssocenter.com:5220",
                                   "http://latest.com:5221",

                                   "http://ssocenter.com",
                                   "http://atest.com",
                                   "http://btest.com",
                                   "http://ctest.com",

                                   "http://lssoCenter.com",
                                   "http://latest.com"
                                   )
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
            )
            );

            builder.Services.AddCors(option =>
            option.AddPolicy("cors1", policy =>
            {
                policy.WithOrigins(
                                   "http://lssocenter.com:5220",
                                   "http://latest.com:5221",
                                   "https://lssocenter.com:6220",
                                   "https://latest.com:6221")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }));


            AnalogData.GetAnalogData(AnalogDataEnum.Password).SetData("test1", "test1");
            AnalogData.GetAnalogData(AnalogDataEnum.Password).SetData("test2", "test2");
            AnalogData.GetAnalogData(AnalogDataEnum.Password).SetData("test3", "test3");

            /////////////·Ö¸îÏß
            var app = builder.Build();


            IConfiguration config = app.Services.GetRequiredService<IConfiguration>();

            ConfigOption.DefaultConfig = new ConfigOption();

            config.Bind("ConfigOption", ConfigOption.DefaultConfig);
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseCors("cors1");

            app.UseAuthorization();


            app.Use((context, next) =>
            {
                if (string.IsNullOrWhiteSpace(ConfigOption.DefaultConfig.CurrentDomain)
                    && context.Request.Host.Host.Contains('.'))
                {
                    ConfigOption.DefaultConfig.CurrentDomain = "http://" + context.Request.Host.ToString();
                }

                return next.Invoke();
            });



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}