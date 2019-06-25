using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AccountingApi.Data;
using AccountingApi.Data.Repository;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers;
using AccountingApi.Helpers.Extentions;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountingApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x =>
                           x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.Culture = new CultureInfo("en-GB");
                    opt.SerializerSettings.DateFormatString = "dd/MM/yyyy";
                    //ReferenceLooping errorunun qarsini almaq ucundur
                    opt.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            //Globalizaton datetime format
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.RequestCultureProviders.Clear();
            });
            //Url-ni goturmek ucun MyHttpContext classinda middlware yaradiriq app-de de cagiririq. bunu yaziriq ki baseUrl-ni goture bilek
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //autoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            });
            //If you have other mapping profiles defined, that profiles will be loaded too

            //Repository interface oxutdururuq
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INomenklaturaRepository, NomenklaturaRepository>();
            services.AddScoped<IPathProvider, PathProvider>();
            services.AddScoped<IAccountsPlanRepository, AccountsPlanRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();

            //JWT servis edirik
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options => {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                         .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //seeder.SeedUsers();
            //Globalization datetime
            var defaultCulture = new CultureInfo("en-GB");
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            //Faylari yuklemek ucun
            app.UseStaticFiles();
            //BaseUrl-ni elde etmek ucun
            app.UseHttpContext();
            app.UseCors(builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

            app.UseMvc();
        }
    }
}
