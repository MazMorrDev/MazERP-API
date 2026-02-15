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

# EXPOSE can use the ENV set above (this is a build-time annotation).
EXPOSE $PORT

# Use a shell entrypoint so ASPNETCORE_URLS is set from the runtime PORT (if provided)
ENTRYPOINT ["sh", "-c", "export ASPNETCORE_URLS=\"http://+:${PORT:-5001}\"; exec dotnet MazErpBack.dll"]