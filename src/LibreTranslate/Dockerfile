FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LibreTranslate/LibreTranslate.csproj", "LibreTranslate/"]
RUN dotnet restore "LibreTranslate/LibreTranslate.csproj"
COPY . .
WORKDIR "/src/LibreTranslate"
RUN dotnet build "LibreTranslate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibreTranslate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibreTranslate.dll"]
