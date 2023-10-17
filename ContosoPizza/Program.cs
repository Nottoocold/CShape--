using ContosoPizza.Services;
// Additional using declarations
using ContosoPizza.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the PizzaContext
builder.Services.AddSqlite<PizzaDbContext>("Data Source=ContosoPizza.db");
/*
dotnet ef migrations add {InitialCreate} --context {PizzaContext} 用来生成迁移文件
dotnet ef database update --context {PizzaContext} 用来应用迁移文件
*/

// Add the PromotionsContext

builder.Services.AddScoped<PizzaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Add the CreateDbIfNotExists method call
// 每次运行应用时，此代码都会调用定义的扩展方法。
app.CreateDbIfNotExists();

app.MapGet("/", () => @"Contoso Pizza management API. Navigate to /swagger to open the Swagger test UI.");

app.Run();
