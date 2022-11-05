FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UrlDetailsService/UrlDetailsService.csproj", "UrlDetailsService/"]
RUN dotnet restore "UrlDetailsService/UrlDetailsService.csproj"
COPY . .
WORKDIR "/src/UrlDetailsService"
RUN dotnet build "UrlDetailsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrlDetailsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlDetailsService.dll"]
