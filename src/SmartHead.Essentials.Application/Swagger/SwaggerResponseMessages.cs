namespace SmartHead.Essentials.Application.Swagger
{
    public class SwaggerResponseMessages
    {
        public const string Forbidden =
            "\"Запрещено\". У клиента нет прав доступа к содержимому, поэтому сервер отказывается дать надлежащий ответ.";

        public const string Unauthorized =
            "\"Неавторизовано\". Для получения запрашиваемого ответа нужна аутентификация.";

        public const string Ok =
            "\"Успешно\". Запрос успешно обработан.";

        public const string NoContent =
            "\"Нет содержимого\". Нет содержимого для ответа на запрос.";

        public const string Created =
            "\"Создано\". Запрос успешно выполнен и в результате был создан ресурс.";

        public const string BadRequest =
            "\"Плохой запрос\". Этот ответ означает, что сервер не понимает запрос из-за неверного синтаксиса.";

        public const string ServerError =
            "\"Внутренняя ошибка сервера\". Сервер столкнулся с ситуацией, которую он не знает как обработать.";
    }
}