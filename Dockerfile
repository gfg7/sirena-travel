FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SirenaTravel/SirenaTravel.csproj", "SirenaTravel/"]
RUN dotnet restore "SirenaTravel/SirenaTravel.csproj"
COPY . .
WORKDIR "/src/SirenaTravel"
RUN dotnet build "SirenaTravel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SirenaTravel.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SirenaTravel.dll"]
