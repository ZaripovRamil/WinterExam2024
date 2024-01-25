FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MainApi/MainApi.csproj", "MainApi/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Utils/Utils.csproj", "Utils/"]
COPY ["DatabaseServices/DatabaseServices.csproj", "DatabaseServices/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "MainApi/MainApi.csproj"
COPY . .
WORKDIR "/src/MainApi"
RUN dotnet build "MainApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainApi.dll"]
