using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using ssoClient.Common;
using ssoClient.Common.JWT;
using ssoClient.Models; 
using System.Text;

namespace ssoClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(a => { });
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


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
                    //  o.TokenValidationParameters.ValidAudience = WebSiteConfig.Config.WebSiteMark;

                    o.TokenValidationParameters.ValidateLifetime = true; //验证生命周期
                    o.TokenValidationParameters.RequireExpirationTime = true; //过期时间
                    o.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(2);

                    //是否验证密钥
                    o.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    //o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(JwtHelper.DecodeKey(JwtHelper.jwtKey));
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

            IConfiguration config = app.Services.GetRequiredService<IConfiguration>();

            ConfigOption.DefaultConfig = new ConfigOption();
            config.Bind("ConfigOption", ConfigOption.DefaultConfig);
            config.Bind("WebSiteConfig", WebSiteConfig.Config);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //  app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
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

            app.Use((context, next) =>
            {
                ///这里做定期续期通知


                var loginID = context.Request.Cookies["WebSiteAccountToken"];
                if (string.IsNullOrWhiteSpace(loginID))
                {
                    ///判断是否登入
                    return next.Invoke();
                }

                var user = AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).GetData<UserInfo>(loginID);
                if (user == null)
                {
                    ///判断是否登入
                    return next.Invoke();
                }

                var dt1 = AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).GetData<long>("RefreshTime");
                if (dt1 < DateTime.Now.AddMinutes(-10).ToFileTime())
                {
                    ///10分钟更新一次
                    AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).SetData<long>("RefreshTime", DateTime.Now.ToFileTime());

                    APIClient.RenewaWebSiteAccount(new Models.RenewaWebSiteAccountParam { WebSiteAccountToken= user.LoginMark });
                }


                return next.Invoke();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
        private static IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            var key = new SymmetricSecurityKey(JwtHelper.DecodeKey(WebSiteConfig.Config.JwtSecret));

            return new List<SecurityKey> { key };
        }
    }
}