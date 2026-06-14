# Tender parsing
FullStack проект .NET C# ASP

Есть сайт https://market.mosreg.ru это ЭТП (электронная торговая площадка), на котором ежедневно публикуются новые тендеры.
Приложение на вход принимает номер тендера, например, 1763198, затем получает с сайта все необходимые данные по этому тендеру и отображает полученные данные.

[Техническое задание к проекту](https://docs.google.com/document/d/1ytS3Nug9II2CUE6MuE5tdLdin63j5Jbs5Lb7AciKRFs/edit?usp=sharing)

# Инструкция к запуску
## Требования

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) — для запуска без Docker
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/) — для клонирования репозитория

### 1. Клонирование репозитория
```bash
git clone https://github.com/Stanislaw-P/Tender-parsing.git
cd <папка-проекта>
```
### 3. Запуск проекта
## ▶️ Запуск без Docker
 
1. Перейти в папку проекта (в названии есть пробел, поэтому путь указывается в кавычках):
```bash
cd "Tender parsing"
```
 
2. Восстановить зависимости:
```bash
dotnet restore
```
 
3. Запустить приложение:
```bash
dotnet run
```
 
4. После старта в консоли появятся адреса, по которым доступно приложение (например, `https://localhost:XXXX` и `http://localhost:YYYY` — точные порты указаны в `Tender parsing/Properties/launchSettings.json`).

## 🐳 Запуск через Docker

Dockerfile находится внутри папки проекта, поэтому собирать образ нужно из **корня репозитория**, указав путь к Dockerfile явно.
 
1. Собрать образ:
```bash
docker build -f "Tender parsing/Dockerfile" -t tender-parsing .
```
 
2. Запустить контейнер:
```bash
docker run -d -p 8080:8080 -e ASPNETCORE_URLS=http://+:8080 --name tender-parsing tender-parsing
```
 
3. Приложение будет доступно по адресу: `http://localhost:8080`
> 💡 По умолчанию в Dockerfile открыты порты **8080 (HTTP)** и **8081 (HTTPS)**. Для простого локального запуска удобнее использовать только HTTP-порт, как показано выше. Если нужен HTTPS внутри контейнера, дополнительно потребуется создать dev-сертификат (`dotnet dev-certs https`) и подключить его к контейнеру как volume.

## 🛑 Остановка контейнера
 
```bash
docker stop tender-parsing
docker rm tender-parsing
```
