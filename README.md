# SmartHead.Essentials
Нугет пакет с типовыми решениями для ускорения разработки. 

# SmartHead.Essentials.Abstractions
Содержит в себе базовые классы, которые необходимы при разработке по DDD + CQRS + Event Sourcing (immediate consistency). Библиотека совместима со спецификациями из open source библиотеки Force (https://github.com/hightechgroup/force). Имеет прямые зависимости к Entity Framework.
- DDD классы: Entity, ValueObject. Реализованы по принципу "is a", а не "can do".

Использование: 
```C#
public class Animal : Entity
```
```C#
public class Address : ValueObject
```

- CQRS + Event Sourcing реализованы с помощью библиотеки MediatR.
Классы: 
```C#
public class AddProductCommand : Command
```
```C#
public class ProductAddedEvent : Event
```
Использование: 

Startup.cs

```C#
services.AddMediatR(typeof(Startup));

// Хранилище Events
services.AddScoped<IEventStore, EventStore>();

// Шина обработки событий
services.AddScoped<IMediatorHandler, InMemoryBus>();

// Domain - Events
services.AddScoped<INotificationHandler<ProductAddedEvent>, ProductAddedEventHandler>();

// Domain - Commands
services.AddScoped<IRequestHandler<AddProductCommand, bool>, ProductCommandHandler>();
```

ProductCommandHandler.cs

```C#
public class ProductCommandHandler : CommandHandlerBase, IRequestHandler<ProductAddCommand, bool>

...

public async Task<bool> Handle(ProductAddCommand command, CancellationToken ct)
{
    bool isValidOperation;
    
    // Валидация

    if (!isValidOperation)
    {
        await Mediator.RaiseEventAsync(
            new DomainNotification(nameof(DomainNotification), Resources.NotValidOperation), ct);
        return false;
    }
    
    // Бизнес логика

    if (!await CommitAsync())
        return false;

    await _mediator.RaiseEventAsync(
        new ProductAddedEvent(
            product.Id,
            product.Price,
            // Другие поля
            ), ct);

    return true;
```

ProductsController.cs

```C#
var command = _mapper.Map<ProductAddCommand>(model);
await Mediator.SendCommandAsync(command);

if (!IsValidOperation())
    return BadRequest(Errors);

return Ok();
```

# SmartHead.Essentials.Abstractions
- InMemoryBus - глобальная шина для функционирования MediatR. Настроен на сохранение всех наследников `Event` в `EventStore`, кроме `DomainNotification`.

Startup.cs

```C#
services.AddScoped<IMediatorHandler, InMemoryBus>();
```

ProductsController.cs

```C#
public class ProductsController : FormattedApiControllerBase
{
    private readonly IMediatorHandler _mediator;
    public ProductsController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }
    
    ...
    [HttpGet]
    public IActionResult Post(AddProduct model)
    {
      ...
      await _mediator.SendCommandAsync(command);
```

- DomainNotificationHandler - проброс доменных ошибок.

Startup.cs

```C#
services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
```

ProductCommandHandler.cs

```C#
public class ProductCommandHandler : CommandHandlerBase, IRequestHandler<ProductAddCommand, bool>

...

public async Task<bool> Handle(ProductAddCommand command, CancellationToken ct)
{
    bool isValidOperation;
    
    // Валидация

    if (!isValidOperation)
    {
        await Mediator.RaiseEventAsync(
            new DomainNotification(nameof(DomainNotification), Resources.NotValidOperation), ct);
        return false;
    }
```

ApiControllerBase.cs

```C#
public abstract class ApiControllerBase : FormattedApiControllerBase
{
    private readonly DomainNotificationHandler _notifications;
    protected readonly IMediatorHandler Mediator;
    protected ApiControllerBase(IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications)
    {
        Mediator = mediator;
        _notifications = (DomainNotificationHandler)notifications;
    }

    protected IEnumerable<DomainNotification> Notifications
        => _notifications.Notifications();

    protected IEnumerable<string> Errors
        => Notifications.Select(x => x.Value);

    protected bool IsValidOperation()
        => !_notifications.HasNotifications();
}
```

- EventStore - шина для обработки наследников `Event`, для последующиего сохранения в базу. Содержит в себе аггрегат, тип, время, и тело события в сериализованном виде. Используется в InMemoryBus.

Startup.cs

```C#
services.AddScoped<IEventStore, EventStore>();
```

InMemoryBus.cs

```C#
public class InMemoryBus : IMediatorHandler
{
    protected readonly IMediator Mediator;
    protected readonly IEventStore EventStore;

    ...
    
    public virtual async Task RaiseEventAsync<T>(T @event, CancellationToken ct = default) 
        where T : Event
    {
        if (!@event.MessageType.Equals("DomainNotification"))
            await EventStore.SaveAsync(@event, ct);

        await Mediator.Publish(@event, ct);
    }
```

- CommandHandlerBase - базовый класс обработчика команд, который содержит в себе базвоые зависимости, необходимые для обработки доменных ошибок и взаимодействия с базой данных. Реализованные методы `Commit()` и `CommitAsync()` не позволят записать в базу, если найдутся доменные ошибки. Также умеют выбрасывать свои ошибки при наличии исключений во время записи, которые можно в будущем аггрегировать и доставить в тело Bad Request итд. 

- UnitOfWork - класс, реализующий паттерн Unit Of Work. Реализованы виртуальные методы `Commit()` и `CommitAsync()` с логикой орбаботки интерфейсов `IHasCreationTime` и `IHasModificationTime`. При необходимости можно добавить свою реализацию, наследовавшись от класса и перезаписать методы `Commit()`, `CommitAsync()`. 
