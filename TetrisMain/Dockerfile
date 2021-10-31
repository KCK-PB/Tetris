FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Tetris-main/Tetris-main.csproj", "Tetris-main/"]
RUN dotnet restore "Tetris-main/Tetris-main.csproj"
COPY . .
WORKDIR "/src/Tetris-main"
RUN dotnet build "Tetris-main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tetris-main.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tetris-main.dll"]
