#### Задание

Реализовать приложение забирающее 50 актуальных вакансии с 
сайта https://rabota.yandex.ru/search?job_industry=275 (или любого 
другого аналогичного ресурса). Приложение должно сохранять вакансии в 
базе данных (одной, на выбор соискателя: MS Access, MS SQL Server, 
Postgres). Приложение должно поддерживать работу как в присоединенном к 
сайту режиме, так и только с сохраненными в БД вакансиями.

Для разработки рекомендуется использовать .Net Core. Использовать EF и подход CodeFirst. 
Преимуществом будет обладать 
задание, запускаемое в Docker контейнере. Должно быть краткое readme с 
описанием функционала и требований по запуску программы. 

### Архитектура front

* front вынесен в отдельную часть (src-front)
* Основной SPA фрейворк - Vue/Vuex/Vue-CLI
* Dev-сборка и запуск сервера фронта через "npm i || npm run serve"
* После сборки (Vue-CLI aka Webpack) представляет собой набор статических файлов

### Архитектура back

* Приложение выполнено в виде одного web-сервиса - монолит
* Основной фреймворк - Asp .Net Core
* Точки доступа (методы контроллеров) - REST API
* БД - подход Code First, переключение провайдера в зависимости от конфигурации сервиса (либо в памяти, либо по MSSQL-connectionString)
* Бизнес-логика реализована через Command-pattern - IGetCommand, IExecuteCommand (фабрика команд не используется - не так много зависимостей)
* Загрузка данных - паттерн инверсии зависимостей (aka Adapter-pattern) - IWebSourceLoader, ISourceParser (отказ от фабрики парсеров - архитектурно не выгодно)
* Инфраструктура зависимостей - в DiExtensions.cs
* Основной проект для сборки,запуска,публикации - Web.Host.Service

### Простой запуск приложения

1. Запустить проект Web.Host.Service (localhost:5000)
2. Запустить dev-сервер фронта(в каталоге src-front) - "npm i || npm run serve" (localhost:8080)

### Запуск инфраструктуры контейнеров

* Запустить скрипт **scripts\win\build-and-run-docker-infrastructure.bat**

Основные этапы сборки и запуска

1. Сборка Docker Image для сервиса Web.Host.Service
2. Сборка Docker Image для прокси сервера Nginx и статических файлов (front)
    * Локация "/" - index.html (основная точка доступа для загрузки фронта);
    * Локация "/api" - прокси на хост:порт сервиса Web.Host.Service
3. Описание оркестрации (параметры запуска образов - порты, переменные, тома) образов - 
в файле docker-compose.yml
4. Запуск инфраструктуры приложения - docker-compose up
5. Запуск chrome на адрес http://$(doker-machine ip)/

Описание скриптов:

* *build-and-run-docker-infrastructure.bat* <br>
Сборка всех необходимых файлов для запуска docker-контейнеров и запуск контейнеров
* *build-di-proxy-front.bat*  <br>
Сборка Vue-App и обертка в docker-образ на основе образа nginx <br>
 `scripts\win\app-front\` - собранные файлы <br>
 `scripts\win\images\client-host-image.tar` - переносимый docker-образ (см. docker load)
* *build-di-webhost-service.bat* <br>
 Публикация проекта Web.Host.Service в каталог  `scripts\win\app-webhost\` и обертка в docker-образ на основе образа mcr.microsoft.com/dotnet/core/aspnet:2.2 <br>
 `scripts\win\images\web-host-image.tar` - переносимый docker-образ (см. docker load)
* *build-images.bat* <br>
Сборка всех необходимых docker-образов
* *run-docker-compose.bat* <br>
Запуск инфраструктуры контейнеров <br>
`docker-compose.yml` - описание запускаемых контейнеров
`app-proxy-front-compose.Dockerfile` обертка над образом фронта, при использовании локальных образов<br>
`app-webhost-compose.Dockerfile` обертка над образом webhost, при использовании локальных образов <br>
`nginx.conf` конфигурация веб-сервера и конфигурация прокси-взаимодействия


Замечание:

Docker был развернут на Win7 средствами Docker Toolbox for Windows. 
В следствии чего создается виртуальная docker-машина (VirtualBox boot2docker), на которой 
выполняются все docker-команды и запускаются контейнеры. 

В виду чего, при использовании других решений по установке doсker, 
возможны ошибки в выполнении скрипта. 

Например 
* при ошибках в подключении томов необходимо сделать конфигурации общих папок (папок решения) и их монтирование в boot2docker
```
sudo mkdir --parents /c
sudo mount -t vboxsf C_DRIVE /c
# C_DRIVE - shared folder "C:\" in VirtualBox settings
```

* возможны сбои в следствии работы антивируса (в частности удаленная отладка контейнера) <br>
Помогло отключение антивируса