FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["UsuariosCRUD/UsuariosCRUD.csproj", "UsuariosCRUD/"]
RUN dotnet restore "UsuariosCRUD/UsuariosCRUD.csproj"
COPY . .
WORKDIR "/src/UsuariosCRUD"
RUN dotnet build "UsuariosCRUD.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsuariosCRUD.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsuariosCRUD.dll"]