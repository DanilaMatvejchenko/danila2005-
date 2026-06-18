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

## Быстрый запуск — одна команда

> Не нужно устанавливать SQL Server, не нужно настраивать БД.  
> Всё работает из коробки.

### Требования

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Запуск

```bash
git clone https://github.com/DanilaMatvejchenko/danila2005-.git
cd danila2005-
dotnet run --project src/ElectronicJournal.API
```

Открыть в браузере: **http://localhost:5000**

При первом запуске автоматически:
- Создастся файл `ElectronicJournal.db` (SQLite)
- Заполнится тестовыми данными (пользователи, оценки, расписание и т.д.)

> Swagger UI для тестирования API: **http://localhost:5000/swagger**

### Тестовые учётные записи

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
2. Установите стартовым проектом: **ElectronicJournal.API**
3. Нажмите **F5**
4. Откроется браузер на `http://localhost:5000`

---

## Мобильное приложение (.NET MAUI)

### Требования
- Visual Studio 2022/2026 с workload **.NET MAUI**
- Android SDK или macOS + Xcode

### Запуск
1. Откройте `ElectronicJournal.sln`
2. Выберите проект: **ElectronicJournal.Mobile**
3. Выберите эмулятор/устройство
4. **F5**

> Для Android-эмулятора API-адрес автоматически настроен на `10.0.2.2`.

---

## Запуск тестов

```bash
dotnet test tests/ElectronicJournal.Tests
```

---

## Переключение на SQL Server

В `src/ElectronicJournal.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ElectronicJournal;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Структура проекта

```
ElectronicJournal.sln
├── src/
│   ├── ElectronicJournal.API/      # API + хостинг Blazor WASM
│   ├── ElectronicJournal.Web/      # Blazor WebAssembly (UI)
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
- Уведомления (SignalR)
- Отчёты по успеваемости
- Тёмная и светлая тема

## Документация

[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)
