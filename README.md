# GoldenCrown

### 💳 Banking REST API built with ASP.NET Core

Современный Web API для управления пользователями, банковскими счетами и
финансовыми операциями.

------------------------------------------------------------------------

## ✨ Features

-   🔐 Регистрация и авторизация пользователей;
-   👤 Управление пользовательскими сессиями;
-   💰 Создание и обслуживание банковских счетов;
-   📈 Просмотр текущего баланса;
-   ➕ Пополнение счёта;
-   🔄 Переводы между пользователями;
-   📜 История финансовых операций;
-   📚 Автоматическая документация API через Swagger;
-   ⚙️ Фоновая обработка и очистка неактуальных данных.

------------------------------------------------------------------------

## 🏗 Architecture

``` text
Client
   │
   ▼
Controllers
   │
   ▼
Services
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server Database
```

Проект построен по принципу разделения ответственности:

  Слой                 Назначение
  -------------------- ----------------------------------
  Controllers          Обработка HTTP-запросов
  Services             Бизнес-логика
  DTOs                 Передача данных между слоями
  Models               Доменные сущности
  Database             Работа с базой данных
  Middleware           Авторизация и обработка запросов
  BackgroundServices   Выполнение фоновых задач

------------------------------------------------------------------------

## 📂 Project Structure

``` text
GoldenCrown/
├── Attributes/
├── BackgroundServices/
├── Controllers/
├── DTOs/
├── Database/
├── Middleware/
├── Migrations/
├── Models/
├── Services/
├── Program.cs
└── GoldenCrown.csproj
```

------------------------------------------------------------------------

## 🚀 Getting Started

### Prerequisites

Убедитесь, что у вас установлены:

-   .NET SDK 10.0;
-   SQL Server;
-   Entity Framework CLI.

Установка EF CLI:

``` bash
dotnet tool install --global dotnet-ef
```

------------------------------------------------------------------------

### Installation

``` bash
git clone https://github.com/Saharo202/GoldenCrown.git
cd GoldenCrown/GoldenCrown
```

Настройте строку подключения в `appsettings.json`.

Примените миграции:

``` bash
dotnet ef database update
```

Запустите приложение:

``` bash
dotnet run
```

------------------------------------------------------------------------

## 📖 API Documentation

Swagger будет доступен по адресу:

``` text
https://localhost:<PORT>/swagger
```

------------------------------------------------------------------------

## 🔐 Authentication

``` http
Authorization: <token>
```

------------------------------------------------------------------------

## 📌 Endpoints

### Authentication

  Method   Endpoint           Description
  -------- ------------------ --------------------------
  POST     `/User/register`   Регистрация пользователя
  POST     `/User/login`      Авторизация пользователя

### Finance

  Method   Endpoint              Description
  -------- --------------------- ---------------------------
  GET      `/Finance/balance`    Получить баланс
  POST     `/Finance/deposit`    Пополнить счёт
  POST     `/Finance/transfer`   Выполнить перевод
  GET      `/Finance/history`    Получить историю операций

------------------------------------------------------------------------

## 🛠 Development Roadmap

-   [ ] JWT Authentication;
-   [ ] Refresh Tokens;
-   [ ] Unit Tests;
-   [ ] Integration Tests;
-   [ ] Docker Support;
-   [ ] CI/CD Pipeline;
-   [ ] Rate Limiting;
-   [ ] Audit Logging;
-   [ ] Role-Based Authorization.

------------------------------------------------------------------------

## 📄 License

Если лицензия отсутствует, все права сохраняются за автором проекта.

------------------------------------------------------------------------

**GoldenCrown** --- учебный проект банковского Web API.
