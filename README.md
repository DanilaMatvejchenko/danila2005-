# Электронный журнал для техникума

Полнофункциональное приложение для учёта успеваемости, посещаемости и взаимодействия преподавателей, студентов и администрации.

## Технологии

| Компонент | Технология |
|-----------|-----------|
| Backend | ASP.NET Core 8.0 Web API |
| Web Frontend | Blazor WebAssembly |
| Mobile | .NET MAUI (Android / iOS) |
| База данных | SQLite (тест) / SQL Server (прод) |
| Реальное время | SignalR |
| Аутентификация | JWT Bearer |
| Тестирование | xUnit + FluentAssertions |

---

## Быстрый запуск (тестовая версия)

> Не нужно устанавливать SQL Server — при первом запуске автоматически создаётся SQLite база с тестовыми данными.

### Требования

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### 1. Клонировать репозиторий

```bash
git clone https://github.com/DanilaMatvejchenko/danila2005-.git
cd danila2005-
```

### 2. Запустить API-сервер

```bash
cd src/ElectronicJournal.API
dotnet run
```

При первом запуске автоматически:
- Создастся файл `ElectronicJournal.db` (SQLite)
- Заполнится тестовыми данными (пользователи, оценки, расписание и т.д.)

API доступен: **http://localhost:5000**
Swagger UI: **http://localhost:5000/swagger**

### 3. Запустить Web-приложение (в отдельном терминале)

```bash
cd src/ElectronicJournal.Web
dotnet run
```

Web-приложение: **http://localhost:5010**

### 4. Войти в систему

| Роль | Email | Пароль |
|------|-------|--------|
| Администратор | `admin@technikum.ru` | `Admin123!` |
| Преподаватель | `smirnov@technikum.ru` | `Teacher123!` |
| Преподаватель | `kozlova@technikum.ru` | `Teacher123!` |
| Студент | `ivanov@technikum.ru` | `Student123!` |
| Студент | `petrova@technikum.ru` | `Student123!` |
| Студент | `sidorov@technikum.ru` | `Student123!` |
| Родитель | `parent@technikum.ru` | `Parent123!` |

---

## Запуск через Visual Studio

1. Откройте `ElectronicJournal.sln`
2. Правый клик на Solution → **Configure Startup Projects**
3. Выберите **Multiple startup projects**
4. Установите `Start` для `ElectronicJournal.API` и `ElectronicJournal.Web`
5. Нажмите **F5**

---

## Мобильное приложение (.NET MAUI)

### Требования
- Visual Studio 2022/2026 с workload **.NET MAUI**
- Android SDK (для Android) или macOS с Xcode (для iOS)

### Запуск
1. Откройте `ElectronicJournal.sln` в Visual Studio
2. Выберите целевой проект: **ElectronicJournal.Mobile**
3. Выберите эмулятор/устройство
4. Нажмите **F5**

> Для Android-эмулятора API-адрес автоматически настроен на `10.0.2.2:5001`.

---

## Запуск тестов

```bash
dotnet test tests/ElectronicJournal.Tests
```

---

## Переключение на SQL Server (продакшн)

В `src/ElectronicJournal.API/appsettings.json` измените строку подключения:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ElectronicJournal;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Или выполните SQL-скрипт: `sql/001_CreateDatabase.sql`

---

## Структура проекта

```
├── src/
│   ├── ElectronicJournal.API/      # REST API + SignalR + SeedData
│   ├── ElectronicJournal.Web/      # Blazor WebAssembly (ПК)
│   └── ElectronicJournal.Mobile/   # .NET MAUI (Android/iOS)
├── tests/
│   └── ElectronicJournal.Tests/    # Unit-тесты
├── sql/                            # SQL скрипты
└── docs/                           # Документация
```

## Функциональность

- 4 роли: Администратор, Преподаватель, Студент, Родитель
- Журнал оценок (текущие, промежуточные, итоговые)
- Учёт посещаемости
- Расписание занятий
- Чат между пользователями
- Уведомления в реальном времени (SignalR)
- Отчёты по успеваемости
- Тёмная и светлая тема
- Адаптивный интерфейс

## Документация

Подробная архитектура: [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)
