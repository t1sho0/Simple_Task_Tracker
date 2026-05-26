# Task Tracker API

Минималистичный и быстрый трекер задач (Task Tracker), построенный на современном стеке .NET 10 и PostgreSQL. Проект полностью контейнеризирован с помощью Docker, настроен локальный разворот через docker-compose, а также реализован полноценный CI/CD пайплайн через GitHub Actions с автоматическим деплоем в облако Render.Интерфейс фронтенда выполнен в лаконичном стиле Notion Dark Mode для комфортной работы в темное время суток.

## Технологический стекBackend: 
  .NET 10 (C#), 
  ASP.NET Core Web 
  APIDatabase: PostgreSQL
  ORM: Entity Framework Core (EF Core) + Npgsql
  Frontend: HTML5, CSS3 (Notion Dark Theme), JavaScript (Vanilla JS / Fetch API)
  DevOps & CI/CD: 
    Docker, Docker Compose, 
    GitHub Actions, 
    Render.comAPI 
  Testing: Scalar / Swagger (в среде Development)

# Архитектура и БезопасностьПолитика секретов (.env):

1. Все чувствительные данные (пароли БД, строки подключения) полностью вынесены из кода проекта и конфигурационных файлов (appsettings.json). 
Локально они хранятся в файле .env, который находится в .gitignore.

2. Контейнеризация: Приложение разделено на два независимых контейнера — веб-сервер с API и изолированная база данных, общающиеся внутри внутренней сети Докера.

3. Автоматические миграции: При старте приложения бэкенд автоматически проверяет состояние базы данных и накатывает недостающие миграции EF Core (Database.MigrateAsync()), что исключает ручную настройку БД при деплое.

# Локальный запуск (Разработка)Для запуска проекта на локальной машине вам понадобятся установленный Docker Desktop и Git.
1. Клонирование репозитория:
    `git clone https://github.com/t1sho0/Simple_Task_Tracker.git`
    `cd Simple_Task_Tracker`
    
2. Настройка окруженияСоздайте в корне проекта файл .env и заполните его своими данными:
   `PlaintextDB_PASSWORD=your_secure_password
    DB_NAME=task_tracker_db
    DB_USER=postgres`
   
3. Запуск через *Docker Compose* Выполните команду для сборки образов и поднятия контейнеров:
   `docker compose up --build`
   
После успешного запуска: 
  Фронтенд и API будут доступны по адресу: `http://localhost:5103`
  Интерфейс документации Scalar (если запущен в режиме Development): `http://localhost:5103/scalar/v1` 

🛰️ Схема CI/CD (GitHub Actions & Render)

В проекте настроен автоматический жизненный цикл изменений (.github/workflows/ci.yml):

  [ Код на ПК ] ──( git push )──> [ GitHub Репозиторий ]
                                        │
                               ( Срабатывает CI )
                                        ▼
                        [ GitHub Actions Runner ]
                        ├── Сборка проекта (.NET 10)
                        └── Прогон тестов (dotnet test)
                                        │
                               ( Успешно? Пингуем CD )
                                        ▼
                        [ Облако Render.com ]
                        ├── Скачивание свежего кода
                        ├── Сборка Docker-образа
                        └── Авто-миграция и деплой базы
Production API URL: https://simple-task-tracker-ul1b.onrender.com📜 Спецификация API (Эндпоинты)Бэкенд реализует классический RESTful API для управления задачами:МетодЭндпоинтОписаниеТело запроса (JSON)GET/api/TasksПолучить список всех задач—POST/api/TasksСоздать новую задачу{"title": "Строка"}PATCH/api/Tasks/{id}Переключить статус (Выполнено/Нет)—PUT/api/Tasks/{id}Изменить текст/название задачи{"id": int, "title": "Новая строка"}DELETE/api/Tasks/permanently/{id}Безвозвратное удаление задачи—
