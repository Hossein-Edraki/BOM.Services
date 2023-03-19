



FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BOM.Services.Api/BOM.Services.Api.csproj", "BOM.Services.Api/"]
RUN dotnet restore "BOM.Services.Api/BOM.Services.Api.csproj"
COPY . .
WORKDIR "/src/BOM.Services.Api"
RUN dotnet build "BOM.Services.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BOM.Services.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BOM.Services.Api.dll"]



