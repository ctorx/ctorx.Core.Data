# ctorx.Core.Data
This package provides unit of work and repository pattern functionality for use in simplifying common tasks in application development for applications utilizing Entity Framework.  Applications control unit of work persistence while service layers are used to encapsulate business logic and provide a generic interface to DbContext functionality.

##How to Use It

###Create your Context based Repository

The Context Repository exposes methods for fetching, adding, attaching and deleting data.

```csharp
  public interface IYourContextRepository : IDbContextRepository<YourDbContext> { }

  public class YourContextRepository : DbContextRepository<YourDbContext>, IYourContextRepository
  {
    public YourContextRepository(YourDbContext yourDbContext ) : 
      base(yourDbContext) { }
  }
```
###Wire Dependencies
You must wire the required dependencies manually or via the UseUoW() extension method in `Startup.ConfigureServices()`.

```csharp
  // Manually
  services.AddScoped<IUnitOfWorkFactory<YourDbContext>, DefaultUnitOfWorkFactory<YourDbContext>>();
  services.AddScoped<IDbContextRepository<YourDbContext>, YourContextRepository>();
  
  // Or...via UseUow()
  services.UseUow<YourDbContext, YourContextRepository>();
```

###Create a Service
Leverage services to encapsulate business logic and handle data operations.

```csharp
  public interface IYourEntityService : IDbContextService
  {
    /// <summary>
    /// Sample method to fetch a list of entities
    /// </summary>
    public Task<IList<YourEntity>> GetRecentAddedEntitiesAsync();
  }
  
  public class YourEntityService : DbContextService<YourDbContext>, IYourEntityService
  {
    /// <summary>
    /// Sample method to fetch a list of entities
    /// </summary>
    public async Task<IList<YourEntity>> GetRecentAddedEntitiesAsync()
    {
      return await this.GetSet<YourEntity>()
        .Where(x => x.SomeProperty)
        .ToListAsync();
    }
  }
```

*NOTE that you can use a DbContextService to perform data operation on any entity in your context, though you should aim for more entity specific services*

###Using the Unit of Work
The unit of work is the wrapper around the SaveChanges method that you'll need to utilize in your applications when adding or updating data.  You don't need a UoW for retrieving data.

Here's an example from an ASP.NET Core MVC Controller

```csharp
  public class YourController : Controller
  {
    readonly IYourEntityService YourEntityService;
    IUnitOfWorkFactory<YourDbContext> UnitOfWorkFactory;
    
    public YourController(IYourEntityService yourEntityService, IUnitOfWorkFactory<YourDbContext> unitOfWorkFactory)
    {
      this.YourEntityService = yourEntityService;
      this.UnitOfWorkFactory = unitOfWorkFactory;
    }
    
    /// <summary>
    /// Sample action to update an entity name
    /// </summary>
    public async Task<IActionResult> UpdateEntityName(int entityId, string name)
    {
      using(var uow = this.UnitOfWorkFactory.NewUnitOfWork())
      {
        var entity = await this.YourEntityService.GetEntityByIdAsync(entityId);
        if(entity == null) 
        {
          throw new InvalidOperationException("Entity does not exist");
        }
        
        try
        {
          entity.Name = name;
          await uow.CommitAsync();
        }
        catch(Exception ex)
        {
          // Log as needed
        }
      }
    }
  }
```
