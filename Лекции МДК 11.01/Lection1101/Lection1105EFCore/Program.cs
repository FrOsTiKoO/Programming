using Lection1105EFCore;
using Lection1105EFCore.Data;
using Lection1105EFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

Console.WriteLine("EF Core");

GameStoreContext context = new();

var result = 

context.Categories.Add(new Category { Name = "123" });

// Выборка из таблицы
var price = 1000;
var games = context.Games
    .FromSql($"SELECT * FROM Game WHERE price <= {price}");
Console.WriteLine(games.ToQueryString());

games = context.Games
    .FromSqlRaw($"SELECT * FROM Game WHERE price <= {0}", price);

    games = context.Games
    .FromSqlRaw($"SELECT * FROM Game WHERE price <= " + price.ToString());
Console.WriteLine(games.ToQueryString());

foreach ( var game in games )
    Console.WriteLine($"{game.Name} - {game.Price}");

// Получение значения скалярного типа
var titles = context.Database
    .SqlQuery<string>($"SELECT Name FROM Game")
    .ToList();

var count = context.Database.ExecuteSql($"UPDATE Games SET Price+=1");

// Вызов хранимой процедуры
var category = "12345";
context.Database.ExecuteSql($"EXECUTE AddCategory2 {category}");
context.Database.ExecuteSql($"EXECUTE AddCategory2 @name={category}");

//SqlParameter id = new SqlParameter { ParameterName = "@id" };

SqlParameter id = new()
{
    ParameterName = "@id",
    SqlDbType = System.Data.SqlDbType.Int,
    Direction = System.Data.ParameterDirection.Output
};
context.Database.ExecuteSqlRaw($"EXECUTE AddCategory2 @name={category}, @id OUTPUT");
Console.WriteLine(id.Value);

games = context.Games
    .FromSql($"GetGamesByPrice {price}");

Console.WriteLine();
foreach (var game in games.ToList())
    Console.WriteLine($"{game.Name} - {game.Price}");

// Вызов стандартной функции
games = context.Games
    .Where(g => EF.Functions.Like (g.Name, "[m-z]%"));

Console.WriteLine();
foreach (var game in games.ToList())
    Console.WriteLine($"{game.Name} - {game.Price}");

var avgPrice = context.Games.Average(g => g.Price);
var countByPrice = context.Games.Count(g => g.Price < 1000);

// Вызов скалярной функции
avgPrice = context.Database
    .SqlQuery<decimal>($"SELECT dbo.GetAvgPrice(2) AS value")
    .FirstOrDefault();

// Вызов табличной функции
var categoryName = "шутер";
games = context.Games
    .FromSql($"SELECT * FROM dbo.GetGamesByCategory({categoryName})");

Console.WriteLine();
foreach (var game in games.ToList())
    Console.WriteLine($"{game.Name} - {game.Price}");

// Маппинг (указывается в OnModelCreating())
    









// group.Students.Aggregate("", (s1, s2) => s1 + s2.Name + "; ") - позволяет вывести все фамилии в один столбец

////context.Categories.Where(c => c.CategoryId > 10).ExecuteDelete();

////context.Games.ExecuteUpdate(g => g
////.SetProperty(g => g.Price, g => g.Price + 1)
////.SetProperty(g => g.IsDeleted, g => false)
////);

////context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

////var categories = context.Categories;



//// select - проекция
//// where - фильтрация
//// group by - груперовка (позволяет сгрупировать данные и работать с агригатными функциями)
//// limit - пагинация
//// order by - сортировка
//// from+join - объединение
//// отслеживание изменений(по умолчанию данные кэшируются)
//// способы отключения отслеживания изменений

////var games = context.Games.AsNoTracking();

////var categories = context.Categories.AsNoTracking();

////var addingCategory = new Category { Name = "qwerty3" };
////context.Categories.Add(addingCategory);

////var deletingCategory = context.Categories.Find(3);
////context.Categories.Remove(deletingCategory);

////var updatingCategory = new Category { CategoryId = 7, Name = "qwerty5" };

////context.Categories.Update(updatingCategory);
////context.Entry(updatingCategory).State = EntityState.Modified;

////context.SaveChanges();

////var games = context.Games.Include(g => g.Category).AsQueryable();

////var gamesDto = games.Select(g => new GameDto
////{
////    Id = g.GameId,
////    Name = g.Name,
////    Price = g.Price,
////    CategoryName = g.Category.Name,

////});

////Console.WriteLine(gamesDto.ToQueryString());

//// Получение связанных данных(работает с навигационными свойствами) 

////Include - загружает данные связанные с текущим набором 
////ThenInclude - загружает данные связанные с загруженными Include

////Пример 1

////var games = context.Games.Include(g => g.Category).AsQueryable();
////Пример 2 (фильтрация данных) чтобы применять фильтры в зависемости от условий данные должны быть типа IQueryable
////GameFilter filter = new() { Category = "рпг", MaxPrice = 1200 };
////if(если указана категория)
////if (!String.IsNullOrEmpty(filter.Category))
////    games = games.Where(g => g.Category.Name == filter.Category);// или "РПГ"

////if(если указана цена)
////if (filter.MaxPrice.HasValue)
////    games = games.Where(g => g.Price < 1000);

////Сортировка данных (OrderBy, OrderByDescending-указывает 1 стобец сортировки,
////ThenBy(по возрастанию), ThenByDesending(по убыванию) - 1..2..3 столбцы сортировки)
////string orderColumn = "Price";
////bool isSortAsc = false;
////games = (isSortAsc)
////    ? games = games.OrderBy(g => EF.Property<object>(g, orderColumn))
////    : games = games.OrderByDescending(g => EF.Property<object>(g, orderColumn));

////пагинация или построчный вывод
////int pageSize = 3;
////int pageIndex = 1;
////games = games.Skip(pageSize * (pageIndex - 1)).Take(pageSize);




//foreach (var game in games.ToList())
//    Console.WriteLine($"{game.GameId} {game.Name} {game.Category.Name}");

////select(проекция используемая для оптимезации, позволяет вернуть только требуемые данные)

//var gamesData = games.
//    Select(g => new
//    {
//        GameName = g.Name
//    });

//Console.WriteLine(games.ToQueryString());
//Console.WriteLine(gamesData.ToQueryString());

////Пример1.1

////var categories = context.Categories.Include(g => g.Games);
////foreach (var category in categories.ToList())
////    Console.WriteLine($"{category.CategoryId} {category.Name} {category.Games.Count}");

///*foreach (var category in categories.ToList())
//{
//    Console.WriteLine($"{category.CategoryId} {category.Name}");
//    foreach (var game in category.Games)
//        Console.WriteLine($"-{game.Name}");

//}*/

////join(позволяет объеденить не связанные таблицы)

//var joinExample = context.Games
//    .Join(context.Categories,
//    g => g.CategoryId,
//    c => c.CategoryId,
//    (g, c) => new
//    {
//        g.Name,
//        CategoryName = c.Name
//    });
//Console.WriteLine(joinExample.ToQueryString());

//// group by - груперовка (позволяет сгрупировать данные и работать с агригатными функциями)

//var groupbyExample = context.Games
//    .GroupBy(g => g.CategoryId)
//    .Select(gr => new
//    {
//        Id = gr.Key,
//        GameCount = gr.Count(),
//        AvgPrise = gr.Average(g => g.Price)
//    });
//Console.WriteLine(groupbyExample.ToQueryString());







