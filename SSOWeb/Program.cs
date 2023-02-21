using Common;
using FreeSqlExtend;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
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

            var provider = builder.Services.BuildServiceProvider();

            ///��ʼ��db���Ӳ�
            ///            
            FreeSqlHelperStatic.InitStaticDB(new SettingHelper(provider.GetService<IConfiguration>()));


            // Add services to the container.
            builder.Services.AddControllersWithViews();


            ///���� IDistributedCache �ֲ�ʽ����
            ///
            string redisConnStr = builder.Configuration.GetConnectionString("RedisConn");
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnStr;
                // options.InstanceName = "SampleInstance";


            });
            ///�ֲ�ʽ����,������Կ��redis�洢
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

            ///����redis
            RedisHelperStatic.InitStatic(redisConnStr);

            ///session����
            builder.Services.AddSession(options =>
            {

            });

            var app = builder.Build();

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