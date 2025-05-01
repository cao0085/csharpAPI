
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RestApiPractice.csproj", "./"]
RUN dotnet restore "RestApiPractice.csproj"

COPY . .
RUN dotnet publish "RestApiPractice.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RestApiPractice.dll"]