
using BLOGPESSOAL.Data;
using BLOGPESSOAL.Model;
using BLOGPESSOAL.Validator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using BLOGPESSOAL.Service.Implements;
using BLOGPESSOAL.Service;
using BLOGPESSOAL.Security.Implements;
using BLOGPESSOAL.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLOGPESSOAL.Configuration;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace BLOGPESSOAL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
            	.AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                 }
);

            // Conexão com o Banco de dados

            if (builder.Configuration["Enviroment:Start"] == "PROD")
            {
                /* Conexão Remota (Nuvem) - PostgreSQL */

                builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");

                var connectionString = builder.Configuration
                    .GetConnectionString("ProdConnection");

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connectionString)
                );

            }
            else
            {
                /* Conexão Local - SQL Server */

                var connectionString = builder.Configuration.
                    GetConnectionString("DefaultConnection");

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString)
                );
            }

            //registrar a validação das entidades
            builder.Services.AddTransient<IValidator<Postagem>, PostagemValidator>();
            builder.Services.AddTransient<IValidator<Tema>, TemaValidator>();
            builder.Services.AddTransient<IValidator<User>, UserValidator>();

            // Registrar as Classes e Interfaces Service
            builder.Services.AddScoped<IPostagemService, PostagemService>();
            builder.Services.AddScoped<ITemaService, TemaService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddTransient<IAuthService, AuthService>();

            // Adicionar a Validação do Token JWT

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var Key = Encoding.UTF8.GetBytes(Settings.Secret);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //Registrar o Swagger
            builder.Services.AddSwaggerGen(options =>
            {

                //Personalizar a Págna inicial do Swagger
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Projeto Blog Pessoal",
                    Description = "Projeto Blog Pessoal - ASP.NET Core 7 - Entity Framework",
                    Contact = new OpenApiContact
                    {
                        Name = "Valéria Carvalho",
                        Email = "valeria.dcarvalho@hotmail.com",
                        Url = new Uri("https://github.com/dValsc")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Github",
                        Url = new Uri("https://github.com/dValsc/BLOG-PESSOAL")
                    }
                });

                //Adicionar a Segurança no Swagger
                options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Digite um Token JWT válido!",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                //Adicionar a configuração visual da Segurança no Swagger
                options.OperationFilter<AuthResponsesOperationFilter>();

            });

            // Adicionar o Fluent Validation no Swagger
            builder.Services.AddFluentValidationRulesToSwagger();

            //configuracao cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            //criar o banco de dados e as tabelas

            using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseDeveloperExceptionPage();

            // Habilitar o Swagger
            app.UseSwagger();

            app.UseSwaggerUI();

            // Swagger como Página Inicial (Home) na Nuvem
            if (app.Environment.IsProduction())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Pessoal - V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            //inicializa cors
            app.UseCors("MyPolicy");

            // Habilitar a Autenticação e a Autorização
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}