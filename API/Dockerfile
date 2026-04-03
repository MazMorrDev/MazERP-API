# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["MazErpBack.csproj", "./"]
RUN dotnet restore "MazErpBack.csproj"

COPY . .
RUN dotnet publish "MazErpBack.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Use a fixed container port; map host/platform port at run time.
EXPOSE 8080

# Better compatibility behind reverse proxies and platforms.
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

ENTRYPOINT ["dotnet", "MazErpBack.dll"]