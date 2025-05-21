# Базовый образ для запуска приложения (ASP.NET)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

# Образ для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["SaveWiseNew/SaveWiseNew.csproj", "SaveWiseNew/"]
RUN dotnet restore "SaveWiseNew/SaveWiseNew.csproj"

# Копируем все остальное и собираем
COPY . .
WORKDIR "/src/SaveWiseNew"
RUN dotnet build "SaveWiseNew.csproj" -c Release -o /app/build

# Публикуем сборку
FROM build AS publish
RUN dotnet publish "SaveWiseNew.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaveWiseNew.dll"]
