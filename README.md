# SmartHead.Essentials
NuGet пакет с типовыми решениями для ускорения разработки. 

## SmartHead.Essentials.Abstractions
Содержит в себе базовые классы, которые необходимы при разработке по DDD + CQRS + Event Sourcing (immediate consistency). Библиотека совместима со спецификациями из open source библиотеки Force (https://github.com/hightechgroup/force). Имеет прямые зависимости к Entity Framework.
### DDD: Entity, ValueObject. 
Реализованы по принципу "is a", а не "can do".

Использование: 
```C#
public class Animal : Entity
```
```C#
public class Address : ValueObject
```

### CQRS + Event Sourcing 
Реализованы с помощью библиотеки MediatR.


```C#
public class AddProductCommand : Command
```

```C#
public class ProductAddedEvent : Event
```

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

## SmartHead.Essentials.Implementation
### InMemoryBus 
Глобальная шина для функционирования MediatR. Настроен на сохранение всех наследников `Event` в `EventStore`, кроме `DomainNotification`.

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

InMemoryBus.cs

```C#
public virtual async Task RaiseEventAsync<T>(T @event, CancellationToken ct = default) 
    where T : Event
{
    if (!@event.MessageType.Equals("DomainNotification"))
        // Записываем наследников Event и с типом не DomainNotification.
        await EventStore.SaveAsync(@event, ct);
        
    // Паблишим ивенты. Они будут доступны при реализации INotificationHandler<T>, где T = Event
    await Mediator.Publish(@event, ct);
}
```

### DomainNotificationHandler 
Обработчик, с помощью которого построена архитектура обработки ошибок через INotification библиотеки MediatR.

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
            new DomainNotification(nameof(DomainNotification), Resources.InvalidOperation), ct);
            
        // Ошибка типа DomainNotification попала в InMemoryBus и сохранилась в памяти. 
        // Теперь она доступна через DomainNotificationHandler для дальнейшей обработки.
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

### EventStore
Шина для обработки наследников `Event`, для последующиего сохранения в базу. Содержит в себе аггрегат, тип, время, и тело события в сериализованном виде. Используется в InMemoryBus.

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

### CommandHandlerBase 
Базовый класс обработчика команд, который содержит в себе базовые зависимости, необходимые для обработки доменных ошибок и взаимодействия с базой данных. Реализованные методы `Commit()` и `CommitAsync()` не позволят записать в базу, если найдутся доменные ошибки. Также умеют выбрасывать свои ошибки при наличии исключений во время записи, которые можно в будущем аггрегировать и доставить в тело Bad Request итд. Содержит зависимости `IMediatorHandler`, `DomainNotificationHandler`, `IUnitOfWork`.

### UnitOfWork 
Класс, реализующий паттерн Unit Of Work. Реализованы виртуальные методы `Commit()` и `CommitAsync()` с логикой орбаботки интерфейсов `IHasCreationTime` и `IHasModificationTime`. При необходимости можно добавить свою реализацию, наследовавшись от класса и перезаписать методы `Commit()`, `CommitAsync()`. 

## SmartHead.Essentials.Application
Набор инструментов, ускоряющих разработку `Application` слоя приложения. 
### Атрибуты
Набор атрибутов для работы с файлами в REST Api.
- AllowedExtensionsAttribute 
- MaxFileSize
- HasValidFileName

Пример.

```C#
[HasValidFileName]
[MaxFileSize(500 * 1024 * 1024)] // 500 mb
[AllowedExtensions(new[] {".jpg", ".png", ".mp4", ".jpeg"})]
public IFormFile File { get; set; }
```

- DevelopmentOnly

Атрибут, который позволяет выключать метод в не Development окружении. Пример.

```C#
[HttpDelete]
[DevelopmentOnly]
[ApiExplorerSettings(IgnoreApi = true)]
public async Task<IActionResult> Delete([FromQuery] DeleteRequest request)
```

### Response Formatter
Инструмент, необходимый для форматирования ответа приложения и приведения ответа в единую стилистику.

Положительный формат ответа: 

```JSON
{
  "content": {
   "key": "value"
  },
  "debugData": "string"
}
```

Отрицательный формат ответа:

```JSON
{
  "subStatus": "string",
  "errorContent": [
    "string"
  ],
  "debugData": "string"
}
```

Регистрация. 

Startup.cs

```C#
services
    .AddControllers()
    .SetCompatibilityVersion(CompatibilityVersion.Latest)
    .AddResponseOutputFormatter();

services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory =
        actionContext =>
            InvalidModelStateResponseFactory.CreateFrom(Resources.InvalidModel, actionContext.ModelState);
});
```

ApiControllerBase.cs

```C#
public abstract class ApiControllerBase : FormattedApiControllerBase
{
```
### Пагинация
Использование.
```C#
/// <summary>
///     Вывод списка продуктов.
/// </summary>
[HttpGet]
[SwaggerResponse(200, SwaggerResponseMessages.Ok + " Возвращается список продуктов.",
    typeof(SwaggerSuccessApiResponse<PagedResponse<ProductItemModel>>))]
public IActionResult Get([FromQuery] PagingQueryModel query)
{
    var products = _context
        .Set<Domain.Entities.Products>()
        .OrderByDescending(x => x.Rating)
        .ProjectTo<ProductItemModel>(_mapper.ConfigurationProvider)
        .Paginate(query.Page, query.Size);

    return Ok(products);
}
```

Ответ.

```JSON
{
    "pagination": {
      "itemsTotal": 0,
      "page": 0,
      "total": 0,
      "size": 0,
      "hasPrevious": true,
      "hasNext": true
    },
    "items": [
      {
        "id": 0,
        "name": "string",
        "price": 0,
        "rating": 0
      }
    ]
}
```

### Swagger Response
Упрощение разработки swagger документации. Автоматически дополняет ответ ошибкой 500, а также 401 и 403 если метод покрыт авторизацией. Для упрощения документирования различных ответов присутствует набор шаблонных сообщений `SwaggerResponseMessages`. `SwaggerErrorApiResponse` и `SwaggerSuccessApiResponse` добавлены для построения тел ответов на swagger странице при использовании в связке с Response Formatter.

Регистрация.
```C#
services.AddSwaggerGen(options =>
    {
        // Ваш конфиг
        options.OperationFilter<ResponseOperationFilter>();
    }
);
```

Использование. 500, 403, 401 ошибки добавились автоматически.

```C#
/// <summary>
///     Удаление продукта.
/// </summary>
[HttpDelete("{id}")]
[Authorize]
[SwaggerResponse(204, SwaggerResponseMessages.NoContent, typeof(void))]
[SwaggerResponse(400, SwaggerResponseMessages.BadRequest, typeof(SwaggerErrorApiResponse<IEnumerable<string>>))]
public async Task<IActionResult> Delete(long id)
```
### Seed
Методы для инициализации данных в БД.
- MigrationsInitializer - используется для автомиграции при старте.
- DataInitializerBase - базовый метод для инициализации данных в базу. Имеет фабричный метод InitializeAsync, который необходимо реализовать.

Использование.

Program.cs
```C#
public static async Task Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();
    await host.InitAsync();
    await host.RunAsync();
}
```

Startup.cs
```C#
services.AddAsyncInitializer<MigrationsInitializer>();
services.AddAsyncInitializer<AdminsInitializer>();
```

#### Примечение
В начале советуется запускать MigrationsInitializer, так как сначала должна инициализироваться актуальная схема, а потом уже все остальное.
