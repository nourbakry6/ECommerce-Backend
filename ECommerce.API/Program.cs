
using ECommerce.API.Middleware;
using ECommerce.Application.Interface;
using ECommerce.Application.Services;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();


            // Add services to the container.

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();

                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["Bearer"] =
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "Enter JWT token"
                        };

                    return Task.CompletedTask;
                });
            });



            //////////////////////////muhem//////////////////////////////


            //adddbcontext kl ma bdu applicati.. st3ml hl connection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //bystkhdm hl connection string ta ytsl b sqlserver
            options.UseSqlServer(
            //bifut bykhd connenction string
            builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            ////////////applicatonuser
            /////addidentitycore ya3ni am ul bdi est3ml identity  a user wclass tb3i hwh applicationuser
            builder.Services.AddIdentityCore<ApplicationUser>(options=> {

            //am ghyr pass validation b identity lzm ykun fi symbole so ana chlta
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            
            })

            .AddRoles<IdentityRole<int>>()//zdt role identity
            .AddEntityFrameworkStores<ApplicationDbContext>()//khzn uuser w role b database bstkhdm ef
            .AddSignInManager()
            .AddDefaultTokenProviders();
            /////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////


            /////waeta ntlon iproductrepositpry mna3ti productrepository/////

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService,ProductService>();
            //////////////////////////////category//////////////////////////
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            //////////////////////////////USER//////////////////////////////
            
            builder.Services.AddScoped<IUserServercs,UserService>();
            //////////////////////////////CART//////////////////////////////
            builder.Services.AddScoped<ICartRepository,CartRepository>();
            builder.Services.AddScoped<ICartService,CartService>();
            ///////////////////////JWT///////////////////////////
            builder.Services.AddScoped<IJwtService,JwtService>();
            //////////////////////////////Order//////////////////////////////
            builder.Services.AddScoped<IOrderRepository,OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            //hyda lal trasaction
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // btkhd asp.core kif ytha2a2 msh jwt

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    }); 


            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            //l app fiya kl service t3un project useramanger dbcontext..
            var app = builder.Build();
            //htyta b program cs krml kl ma ychgthl l program yt2kd iza endi admin aw la aw by3ml
            //nhna bdna nst3ml usermnge w rolemnger mra whe awl ma ychgtchl project ta na3mols seeder so emlan scope
            using (var scope = app.Services.CreateScope())
            {//byftah endi maseha mu2akate

            //usermanger class jhex bi alb asp identity ta a3mol create delete... lal user applicatiioniser y3ni

            //scope.serviceprovider mn hl maseha jbli hl service li hwh... usermanger
                var userManager = scope.ServiceProvider
    .GetRequiredService<UserManager<ApplicationUser>>();

                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole<int>>>();

                await DbSeeder.Seed(userManager, roleManager);//bi nfza
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

               
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();//bt2ra token l jye mn client
            app.UseAuthorization();//bi tabie[authorize]a contorller

            app.MapControllers();

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.Run();
          
            
        }
    }
}
