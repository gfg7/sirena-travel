# sirena-travel-test-task
Имеются два поставщика, которые с помощью HTTP API предоставляют методы для осуществления поиска маршрутов.
Каждый маршрут характеризуется следующими базовыми параметрами:

Точка старта
Точка прибытия
Дата\время старта
Дата\время прибытия
Цена маршрута
TimeToLive для маршрута с такой ценой

Поставщики имеют различные контракты для поиска маршрутов:

ProviderOneSearchRequest \ ProviderOneSearchResponse (HTTP POST http://provider-one/api/v1/search)
ProviderTwoSearchRequest \ ProviderTwoSearchResponse (HTTP POST http://provider-two/api/v1/search)

Поставщики так же имеют метод для проверки их работоспособности на данные момент (поставщик может быть недоступен в момент выполнения поиска).
Пусть интерфейсы методов будут одинаковыми:
HTTP GET http://provider-one/api/v1/ping
HTTP GET http://provider-two/api/v1/ping

HTTP 200 if provider is ready
HTTP 500 if provider is down

Необходимо реализовать HTTP API, которое позволит выполнять аггрегированый поиск с фильтрацией, с помощью данных поставщиков (ISearchService):

Request\response для API соответственно SearchRequest\SearchResponse.
API должно позволять проверить свою текущую доступность (аналогично каждому из поставщиков).

Так же:

API должно иметь свой кэш для дальнейшей работы с маршрутами по Route->Guid.
API должно уметь производить поиск только в рамках закэшированных данных (SearchRequest -> Filters -> OnlyCached).



### Для тестирования решения с использованием заглушки провайдеров:

``docker compose -f docker-compose.yml -f docker-compose.stub.yml up -d ``

Для проверки кейсов, помимо happy path - изменять нужные эдпоинты в .test


Переменные среды:

- REDIS_CONNECTION=строка подключения Redis
- ProviderOne=хост провайдера 1
- ProviderOnePing=пинг провайдера 1
- ProviderOneService=сервис провайдера 1
- ProviderTwo=хост провайдера 2
- ProviderTwoPing=пинг провайдера 2
- ProviderTwoService=пинг провайдера 2


Эндпоинты эмулятора:

- *ping-fail - 500
- *ping-ok - 200
- *search-nomatch - пустой массив Route
- provider-*-/search - динамический соответсвующий ответ провайдера
