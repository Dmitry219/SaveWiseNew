# The basic image for launching the application (ASP.NET )
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

# Project build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore the dependencies
COPY ["SaveWiseNew/SaveWiseNew.csproj", "SaveWiseNew/"]
RUN dotnet restore "SaveWiseNew/SaveWiseNew.csproj"

# We copy everything else and assemble it.
COPY . .
WORKDIR "/src/SaveWiseNew"
RUN dotnet build "SaveWiseNew.csproj" -c Release -o /app/build

# Publishing the build
FROM build AS publish
RUN dotnet publish "SaveWiseNew.csproj" -c Release -o /app/publish /p:UseAppHost=false

# The final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaveWiseNew.dll"]
