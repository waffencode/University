# University 🎓

<img width="1920" height="1080" src="https://github.com/user-attachments/assets/f8819358-e78b-413f-8aca-b2df0f9aeaa7" />

University — информационная система для студентов и преподавателей университета с открытым исходным кодом. 

Состоит из бэкенда на ASP.NET (этот репозиторий) и фронтенда на React (https://github.com/waffencode/university-frontend)

`🚀` Демонстрационный клиент: https://waffencode.ru/

### ✨ Особенности

- **`📅` Просмотр расписания** на день и неделю
- **`💬` Система обмена сообщениями**
- **`📚` Учёт данных РПД** (рабочих программ дисциплин)
- **`🕒` Формирование расписания** на основе данных из рабочих программ дисциплин
- **`🏫` Учёт аудиторного фонда**, дисциплин и направлений подготовки

### Развёртывание

Ниже описывается типовой случай развёртывания системы на выделенном сервере, для которого вам понадобится:
- PostgreSQL
- Nginx
- Docker
- .NET SDK и инструментарий EF Core (`dotnet tool install --global dotnet-ef`)

Для выполнения развёртывания необходимо:

Клонировать репозитории бэкенда и фронтенда:
```bash
mkdir ~/deployments
cd ~/deployments
git clone https://github.com/waffencode/University.git
git clone https://github.com/waffencode/university-frontend.git
```

#### Backend

Перейти в директорию репозитория бэкенд-части:
```bash
cd ~/deployments/University
```

Подготовить бэкенд-часть — вписать данные для подключения к базе в файл `appsettings.json` (если будет выполняться запуск Debug-конфигурации, то `appsettings.Development.json`):
```json
"ConnectionStrings": {
    "Database": "Host=localhost;Port=5432;Database=university;Username=postgres;Password=;"
},
```

Установить переменную `BUILD_CONFIGURATION` и `ASPNETCORE_ENVIRONMENT`, обозначив тот тип билда, который вы хотите получить (релизный или отладочный):
```bash
export BUILD_CONFIGURATION=Release
```

Соответственно установить в файле `compose.yaml` нужный тип окружения:
```yaml
...
environment:
  - ASPNETCORE_ENVIRONMENT=Production
...
```

Запустить сборку:
```bash
docker compose up -d --build
```

Создать миграцию EF Core, которая создаст нужную структуру базы данных, и применить её к базе:
```bash
dotnet ef migrations add Initial --project University.API
dotnet ef database update --project University.API -- --environment Production
```

Перезапустить приложение после успешного применения миграции:
```bash
docker compose down
docker compose up -d
```

#### Frontend

Подготовить сборку фронтенд-части:
```bash
cd ~/deployments/university-frontend
npm install
npx @chakra-ui/cli snippet add
npx @chakra-ui/cli snippet add password-input prose
```

Создать в директории файл `.env`, установив нужные значения переменных окружения:
```bash
HTTPS=true
VITE_DEMO_MODE_BANNER=true
```

В файле `src/config.json` указать адрес, по которому доступен развёрнутый бэкенд:
```json
{
  "serverUrl": "https://api.waffencode.ru/api"
}
```

Собрать фронтенд и разместить его по пути доступных сайтов Nginx:
```bash
npm run build
cp build/ /var/www/university-frontend/ -r
```

В самом Nginx необходимо указать путь, по которому размещена директория с файлами собранного приложения, например `root /var/www/university-frontend/build;`
Подробнее о настройке Nginx см. соответствующие инструкции.

Для использования HTTPS можно сгенерировать сертификаты с помощью [Certbot](https://certbot.eff.org/).
