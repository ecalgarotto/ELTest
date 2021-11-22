
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Swoop.EL.Company/Swoop.EL.Company.csproj", "Swoop.EL.Company/"]
RUN dotnet restore "Swoop.EL.Company/Swoop.EL.Company.csproj"
COPY . .
WORKDIR "/src/Swoop.EL.Company"
RUN dotnet build "Swoop.EL.Company.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Swoop.EL.Company.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Swoop.EL.Company.dll"]