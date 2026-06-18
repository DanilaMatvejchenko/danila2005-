# Электронный журнал для техникума

Полнофункциональное приложение для учёта успеваемости студентов, посещаемости и взаимодействия между преподавателями, студентами и администрацией.

## Технологии

| Компонент | Технология |
|-----------|-----------|
| Backend | ASP.NET Core 8.0 Web API |
| Web Frontend | Blazor WebAssembly |
| Mobile | .NET MAUI (Android / iOS) |
| База данных | SQL Server |
| Реальное время | SignalR |
| Аутентификация | JWT Bearer |
| Тестирование | xUnit + FluentAssertions |

## Быстрый старт

### Требования
- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022/2026

### Настройка БД
```sql
-- Выполните скрипт создания базы данных
sqlcmd -S localhost -i sql/001_CreateDatabase.sql
```

### Запуск API
```bash
cd src/ElectronicJournal.API
dotnet run
```
API будет доступен по адресу `https://localhost:7001`  
Swagger: `https://localhost:7001/swagger`

### Запуск Web-приложения
```bash
cd src/ElectronicJournal.Web
dotnet run
```

### Сборка мобильного приложения
Откройте `ElectronicJournal.sln` в Visual Studio и выберите проект `ElectronicJournal.Mobile`.

## Структура проекта

```
├── src/
│   ├── ElectronicJournal.API/      # REST API + SignalR
│   ├── ElectronicJournal.Web/      # Blazor WebAssembly (ПК)
│   └── ElectronicJournal.Mobile/   # .NET MAUI (Android/iOS)
├── tests/
│   └── ElectronicJournal.Tests/    # Unit-тесты
├── sql/                            # SQL скрипты
└── docs/                           # Документация
```

## Функциональность

- Авторизация с ролями (Администратор, Преподаватель, Студент, Родитель)
- Электронный журнал оценок
- Учёт посещаемости
- Расписание занятий
- Чат между пользователями
- Push-уведомления (SignalR)
- Отчёты по успеваемости
- Тёмная и светлая тема
- Аудит-лог действий

## Учётные данные по умолчанию

| Роль | Email | Пароль |
|------|-------|--------|
| Администратор | admin@technikum.ru | Admin123! |

## Документация

Подробная архитектура и описание проекта: [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)
