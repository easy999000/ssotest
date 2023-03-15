using Common;
using FreeSqlExtend;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SSOBLL;
using SSOBLL.DBModel;
using SSOBLL.ExpiredMonitor;
using SSOBLL.JWT;
using SSOBLL.Login;
using System.Diagnostics;

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

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddJwtBearer(JwtHelper.SchemeName, o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters();
                    o.RequireHttpsMetadata = false;
                    //是否验证发行人
                    o.TokenValidationParameters.ValidateIssuer = true;
                    o.TokenValidationParameters.ValidIssuer = JwtHelper.Issuer;

                    o.TokenValidationParameters.ValidateAudience = false;

                    o.TokenValidationParameters.ValidateLifetime = true; //验证生命周期
                    o.TokenValidationParameters.RequireExpirationTime = true; //过期时间
                    o.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(3);

                    //是否验证密钥
                    o.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    // o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(JwtHelper.DecodeKey(JwtHelper.jwtKey));
                    o.TokenValidationParameters.IssuerSigningKeyResolver = IssuerSigningKeyResolver;

                });

            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy(JwtHelper.SSOAuthorizationPllicy, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtHelper.SchemeName);
                    policy.RequireAuthenticatedUser();


                });
            });


            var app = builder.Build();

            var provider = app.Services;
            LoggerHelper.Init(provider.GetService<ILogger<LoggerHelper>>());
            LoggerHelper.LogInformation($"app 已经启动{DateTime.Now.ToString()}");
            ///配置redis
            RedisHelper.InitStatic(redisConnStr);


            ///初始化db连接层
            ///            
            FreeSqlHelperStatic.InitStaticDB(new SettingHelper(provider.GetService<IConfiguration>()));
            FreeSqlHelperStatic.StaticDB.WriteMsg += (a, b) =>
            {
                switch (a)
                {
                    case MsgType.Info:
                        LoggerHelper.LogInformation(b);
                        break;
                    case MsgType.Error:
                        LoggerHelper.LogError(b);
                        break;
                    default:
                        break;
                }
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
            //ILogger<string> l;
            //l.LogError()

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //WebThreadTest test= new WebThreadTest();
            //test.Start();

            RedisTokenExpired TokenExpired = new RedisTokenExpired(redisConnStr);

            TokenExpired.Subscribe();

            app.Run();
        }

        private static IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            var jwtToken = securityToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            if (jwtToken == null)
            {
                return new List<SecurityKey> { };
            }

            var WebSiteMark = jwtToken.Claims.FirstOrDefault(f => f.Type == "WebSiteMark");
            if (WebSiteMark == null)
            {
                return new List<SecurityKey> { };
            } 

            var secret = WebSite.GetWebSiteSecret_Catch(WebSiteMark.Value);

            var key = new SymmetricSecurityKey(JwtHelper.DecodeKey(secret.Key1));

            return new List<SecurityKey> { key };
             
        }
    }
}