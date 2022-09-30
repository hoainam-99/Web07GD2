using Misa.CukCukMaterial.CTM.BL;
using Misa.CukCukMaterial.CTM.DL;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

// Lấy dữ liệu connectionString từ file appsettings
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Dependency injection
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));

builder.Services.AddScoped<IMaterialBL, MaterialBL>();
builder.Services.AddScoped<IMaterialDL, MaterialDL>();

builder.Services.AddScoped<ICategoryBL, CategoryBL>();
builder.Services.AddScoped<ICategoryDL, CategoryDL>();

builder.Services.AddScoped<IStockBL, StockBL>();
builder.Services.AddScoped<IStockDL, StockDL>();

builder.Services.AddScoped<IUnitBL, UnitBL>();
builder.Services.AddScoped<IUnitDL, UnitDL>();

// Validate entity
builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
{
    option.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers();

// Fix CORS cho phép tất cả các port đều có thể call đến api
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
