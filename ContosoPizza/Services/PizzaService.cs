using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaDbContext _context;

    public PizzaService(PizzaDbContext context)
    {
        _context = context;
    }

    /*  Pizzas 集合包含 pizzas 表中的所有行
        AsNoTracking 扩展方法指示 EF Core 禁用更改跟踪。 由于此操作是只读的，因此 AsNoTracking 可以优化性能
        所有披萨都随ToList()方法返回
    */
    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas.AsNoTracking().ToList();
    }

    /*
        Include 扩展方法采用 lambda 表达式 来指定将 Toppings 和 Sauce 导航属性包含在结果中（通过使用预先加载）。 如果不使用此表达式，EF Core 会为这些属性返回 null。
        SingleOrDefault 方法返回与 lambda 表达式匹配的披萨。
        如果没有记录匹配，则返回 null。
        如果多个记录匹配，则会引发异常。
        lambda 表达式描述 Id 属性等于 id 参数的记录
    */
    public Pizza? GetById(int id)
    {
        return _context.Pizzas.Include(p => p.Toppings).Include(p => p.Sauce).AsNoTracking().SingleOrDefault(p => p.Id == id);
    }

    /*
        假定 newPizza 为有效对象。 EF Core 不执行数据验证，因此任何验证都必须由 ASP.NET Core 运行时或用户代码处理。
        Add 方法将 newPizza 实体添加到 EF Core 的对象图中。
        SaveChanges 方法指示 EF Core 将对象更改保存到数据库。
    */
    public Pizza? Create(Pizza newPizza)
    {
        _context.Add(newPizza);
        _context.SaveChanges();
        return newPizza;
    }

    /*
        对现有 Pizza 和 Topping 对象的引用是使用 Find 创建的。 Find 是按主键查询记录的优化方法。 在查询数据库之前，Find 会先搜索本地实体图
        使用 .Add 方法将 Topping 对象添加到 Pizza.Toppings 集合中。 如果集合不存在，则创建一个新集合。
        SaveChanges 方法指示 EF Core 将对象更改保存到数据库。
    */
    public void AddTopping(int PizzaId, int ToppingId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var toppingToAdd = _context.Toppings.Find(ToppingId);
        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or Topping not found");
        }
        // if (pizzaToUpdate.Toppings is null)
        // {
        //     pizzaToUpdate.Toppings = new List<Topping>();
        // }
        pizzaToUpdate.Toppings ??= new List<Topping>();
        pizzaToUpdate.Toppings.Add(toppingToAdd);
        _context.SaveChanges();
    }

    /*
       对现有 Pizza 和 Sauce 对象的引用是使用 Find 创建的。 Find 是按主键查询记录的优化方法。 在查询数据库之前，Find 会先搜索本地实体图
       Pizza.Sauce 属性设置为 Sauce 对象. 
       Update 方法调用是不必要的，因为 EF Core 检测到你在 Pizza 上设置了 Sauce 属性。 但是，如果你不设置该属性，则 EF Core 不会更新数据库。
       SaveChanges 方法指示 EF Core 将对象更改保存到数据库。
   */
    public void UpdateSauce(int PizzaId, int SauceId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var sauceToUpdate = _context.Sauces.Find(SauceId);
        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("Pizza or Topping not found");
        }
        pizzaToUpdate.Sauce = sauceToUpdate;
        _context.SaveChanges();
    }

    /*
        Find 方法通过主键（在本例中为 Id）检索披萨。
        如果披萨存在，则将其从 EF Core 的对象图中删除。
        SaveChanges 方法指示 EF Core 将对象更改保存到数据库。
    */
    public void DeleteById(int id)
    {
        var pizzaToDelete = _context.Pizzas.Find(id);
        if (pizzaToDelete is not null)
        {
            _context.Pizzas.Remove(pizzaToDelete);
            _context.SaveChanges();
        }
    }
}