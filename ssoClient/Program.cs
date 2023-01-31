using ssoCommon;

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
            builder.Services.AddControllersWithViews();

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

            app.UseAuthorization();

            app.Use((context, next) =>
            {
                if (string.IsNullOrWhiteSpace(ConfigOption.DefaultConfig.CurrentDomain)
                    && context.Request.Host.Host.Contains('.'))
                {
                    ConfigOption.DefaultConfig.CurrentDomain ="http://"+ context.Request.Host.ToString();
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