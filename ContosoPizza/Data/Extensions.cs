using ContosoPizza.Data;

public static class Extensions
{
    /*
        CreateDbIfNotExists 方法被定义为 IHost 的扩展。
        IHost 是 ASP.NET Core 应用程序的主机。 在 ASP.NET Core 中，主机是应用程序的启动点。 主机负责启动应用程序并将其配置为运行。
        创建对 PizzaContext 服务的引用。 PizzaContext 是 DbContext 的子类，DbContext 是 EF Core 的主要类。 DbContext 表示数据库会话，并提供查询和保存数据的方法。
        EnsureCreated 可确保数据库存在。 如果数据库不存在，则 EF Core 会创建数据库并使用模型创建数据库架构。 如果数据库存在，则 EnsureCreated 不执行任何操作。
            注意：如果数据库不存在，EnsureCreated 会创建一个新的数据库。 新数据库是没有针对迁移进行配置的，因此请谨慎使用此方法。
        Initialize 方法是在 ContosoPizza/Data/DbInitializer.cs 中定义的。 Initialize 方法会将示例数据添加到数据库中
    */
    public static void CreateDbIfNotExists(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<PizzaDbContext>();
            context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}