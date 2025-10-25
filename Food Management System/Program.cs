using FOOD.DATA;
using FOOD.DATA.Infrastructure;
using FOOD.DATA.Repository.InventoryRepository;
using FOOD.DATA.Repository.MenuRepository;
using FOOD.DATA.Repository.OderRepository;
using FOOD.DATA.Repository.RecipeRepository;
using FOOD.DATA.Repository.UserRepository;
using FOOD.SERVICES.AuthenticationServices;
using FOOD.SERVICES.BackgrounServices;
using FOOD.SERVICES.Inventery;
using FOOD.SERVICES.MailServices;
using FOOD.SERVICES.Mapping;
using FOOD.SERVICES.MenuServices;
using FOOD.SERVICES.OrderServices;
using FOOD.SERVICES.RecipeServices;
using FOOD.SERVICES.ReportExport;
using FOOD.SERVICES.Reports;
using FOOD.SERVICES.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddApplicationPart(typeof(Controller).Assembly);
builder.Services.AddDbContext<DataContext>(options=>options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("FOOD.DATA")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IDailyOrderReport, DailyOrderReport>();  
builder.Services.AddScoped<ILowStockReports, LowStockReports>();
builder.Services.AddScoped<IDailyReportExcel, DailyReportExcel>();
builder.Services.AddScoped<ILowStockExcel, LowStockExcel>();
builder.Services.AddScoped<IEmailServices, EmailServices>();

builder.Services.AddHostedService<LowStockEmailNotificationServices>();

builder.Services.AddScoped<IAuth, Auth>();

ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital Management",
        Version = "v1",
        Description = "API documentation for the Hospital Outpatient Management System"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
