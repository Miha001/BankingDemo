FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BankingDemo.API/BankingDemo.API.csproj", "BankingDemo.API/"]
COPY ["BankingDemo.Application/BankingDemo.Application.csproj", "BankingDemo.Application/"]
COPY ["BankingDemo.Domain/BankingDemo.Domain.csproj", "BankingDemo.Domain/"]
COPY ["BankingDemo.Infrastructure/BankingDemo.Infrastructure.csproj", "BankingDemo.Infrastructure/"]

RUN dotnet restore "BankingDemo.API/BankingDemo.API.csproj"

COPY . .

WORKDIR "/src/BankingDemo.API"
RUN dotnet publish "BankingDemo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BankingDemo.API.dll"]