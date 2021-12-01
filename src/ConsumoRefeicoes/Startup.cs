using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using backend.Helper.Connection;
using backend.Interfaces;
using backend.Interfaces.Services;
using backend.Interfaces.Services.Security;
using backend.Models;
using backend.Services;
using backend.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// using Microsoft.AspNetCore.SpaServices;
using Serilog;

namespace backend
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

            // services.AddControllers();
            services.AddResponseCompression();

            services.AddControllersWithViews();


            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });


            services.Configure<LdapConfig>(this.Configuration.GetSection("Ldap"));
            services.AddScoped<ILogin, LoginService>();
            services.AddScoped<IAuthentication, LdapAuthService>();

            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddSingleton<ILeitorService, LeitorService>();
            services.AddSingleton<IDigital, DigitalService>();
            services.AddHttpContextAccessor();//permite o acesso de dados via token jwt, em qlqer lugar da aplicação (IHttpContextAccessor _httpContextAccessor;)





            services.AddScoped<IUserSenior, UserSeniorService>();
            services.AddScoped<IRefeicoes, RefeicoesService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUser, UserService>();

            services.AddCors();


            var key = Encoding.ASCII.GetBytes((Configuration["Jwt:Key"]));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });

            services.AddSwaggerGen(c =>
                  {

                      c.SwaggerDoc("v1",
                  new OpenApiInfo
                  {
                      Title = "API de integração com apliativos Portal Unimed",
                      Version = "v1",
                      Description = "API de integração com o aplicativo do beneficiário do Portal Unimed",
                      Contact = new OpenApiContact
                      {
                          Name = "Unimed Chapecó",
                          Url = new Uri("https://www.unimedchapeco.coop.br")
                      }
                  });
                      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                      {
                          Description = "Favor inserir um token JWT válido",
                          Name = "Authorization",
                          Type = SecuritySchemeType.Http,
                          BearerFormat = "JWT",
                          In = ParameterLocation.Header,
                          Scheme = "bearer"
                      });
                      c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                  new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                      Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                  },
                  new string[] { }
                }
                    });

                  });

            Console.WriteLine("ConfigureServices");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {




            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(x => _ = true)
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSerilogRequestLogging();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            ILeitorService leitorService = app.ApplicationServices.GetService<ILeitorService>();
            leitorService.Initialize();

            IDigital digitalService = app.ApplicationServices.GetService<IDigital>();
            IEnumerable<UserDigital> allUsers = digitalService.GetAllUsers();
            leitorService.RegisterUsersDigital(allUsers);


            Console.WriteLine("Configure");
        }
    }
}
