FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WinterExam23-24/WinterExam23-24.csproj", "WinterExam23-24/"]
RUN dotnet restore "WinterExam23-24/WinterExam23-24.csproj"
COPY . .
WORKDIR "/src/WinterExam23-24"
RUN dotnet build "WinterExam23-24.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WinterExam23-24.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WinterExam23-24.dll"]
