using System;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using WebServiceForAngular.Helpers;
using WebServiceForAngular.Auth;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.DAL.EF;
using WebServiceForAngular.DAL.Repositories;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.BLL.Services;
using System.Threading.Tasks;

namespace WebServiceForAngular
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Data Source=NASTYUHA;Initial Catalog=UserPostDB;Integrated Security=True";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("WebServiceForAngular.DAL")));
            services.AddIdentity<AppUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IAppUserService, AppUserService>();
            services.AddTransient<ICheckItemService, CheckItemService>();
            services.AddTransient<ICheckListPostService, CheckListPostService>();
            services.AddTransient<IUserPostService, UserPostService>();
            services.AddTransient<IClaimPrincipalService, ClaimPrincipalService>();
            // Register the ConfigurationBuilder instance of FacebookAuthSettings

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            // add identity
            var builder = services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders", 


                      b =>
                      {
                          b.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                      });
            });
            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 6200;
            //});
            services.AddAutoMapper();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>()).AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;
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
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }
            app.UseHttpsRedirection();
            //app.UseExceptionHandler(
            //    builder =>
            //    {
            //        builder.Run(
            //            async context =>
            //                {
            //                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //                    var error = context.Features.Get<IExceptionHandlerFeature>();
            //                    if (error != null)
            //                    {
            //                        context.Response.AddApplicationError(error.Error.Message);
            //                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            //                    }
            //                });
            //    });

            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
