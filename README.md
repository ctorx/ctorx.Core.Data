# ctorx.Core.Data
This package provides unit of work and generic repository functionality for use in simplifying common tasks in application development for applications utilizing Entity Framework.

## Wait, Doesn't EF Already Provide Repository and Unit of Work?
Entity Framework provides its own implementation of the Unit of Work and Repository patterns and when used directly in your application, serves to provide the offerings of those patterns effectively.  This framework is for developers who want to abstract Entity Framework away from the application yet maintain control over what constitutes a unit of work and when it is committed.  

## How It Works
The application uses a UnitOfWorkFactory to begin a unit of work.  In a web application, for example, this is most likely a POST to a MVC controller. Within the unit of work, one or more calls are made to the service layer to essentially prepare changes for persistence by applying business logic, enforcing rules, etc.  Finally, the unit of work is committed by the application which ultimately triggers the SaveChanges() call on the underlying DbContext.

Consider the following example in creating a new user from a web application:

  1. A Web Form is posted
  2. The controller initiates a new unit of work
  3. The controller calls the service layer to create a new user
  4. The controller calls the service layer to add the user to a few roles
  5. The controller commits the unit of work which persists changes to the data store
  6. If the commit operation was successful, the controller then calls the service layer to send an email notification to the new user
  7. If the commit operation was NOT sucessful, the controller can log the error, notify the UI, and an email message is not sent.
  
And because all of the changes are made within the unit of work, they benefit from EF's transaction scoping.  If the add user operation succeeds but the add role operation does not, the unit of work will not persist and the application knows it should not send the email message.  

## The Code - Getting things Going

### Create your Context based Repository

The Context Repository exposes methods for fetching, adding, attaching and deleting data.  The repository is not meant to be consumed by your application directly -- it is a means for your service layer to access data.

```csharp
  public interface IYourContextRepository : IDbContextRepository<YourDbContext> { }

  public class YourContextRepository : DbContextRepository<YourDbContext>, IYourContextRepository
  {
    public YourContextRepository(YourDbContext yourDbContext ) : 
      base(yourDbContext) { }
  }
```
### Create a Service
Services are used to encapsulate business logic.  

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

*NOTE that you can use a DbContextService to perform data operations on any entity in your context, though you should aim for more entity specific services*

### Using the Unit of Work
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

### Wire Dependencies
You must wire the required dependencies manually or via the UseUoW() extension method in `Startup.ConfigureServices()`.

```csharp
  // Manually
  services.AddScoped<IUnitOfWorkFactory<YourDbContext>, DefaultUnitOfWorkFactory<YourDbContext>>();
  services.AddScoped<IDbContextRepository<YourDbContext>, YourContextRepository>();
  
  // Or...via UseUow()
  services.UseUow<YourDbContext, YourContextRepository>();
```

