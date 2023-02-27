using Common;
using FreeSqlExtend;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SSOBLL;

namespace SSOWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            ///配置 IDistributedCache 分布式缓存
            ///
            string redisConnStr = builder.Configuration.GetConnectionString("RedisConn");
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnStr;
                // options.InstanceName = "SampleInstance";


            });
            //builder.Services.AddDistributedMemoryCache(option => { 

            //});

            ///分布式部署,配置秘钥在redis存储
            builder.Services.AddDataProtection()
                .SetApplicationName("SSOWeb_ApplicationName")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
                // .DisableAutomaticKeyGeneration()
                .PersistKeysToStackExchangeRedis(StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnStr)
                , Constant.DataProtectionRedisKey)
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                }); ;



            ///session配置
            builder.Services.AddSession(options =>
            {

            });

            var app = builder.Build();
            Console.WriteLine($"app 已经启动{DateTime.Now.ToString()}");

            ///配置redis
            RedisHelperStatic.InitStatic(redisConnStr);

            var provider = app.Services;

            ///初始化db连接层
            ///            
            FreeSqlHelperStatic.InitStaticDB(new SettingHelper(provider.GetService<IConfiguration>()));
            FreeSqlHelperStatic.StaticDB.WriteMsg += (a, b) =>
            {
                Console.WriteLine(b);
            };

            //配置缓存帮助类
            CatchHelper.InitCache(provider.GetService<IDistributedCache>());


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}