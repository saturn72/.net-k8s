#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Datafeed.Rest/Datafeed.Rest.csproj", "Services/Datafeed.Rest/"]
RUN dotnet restore "Services/Datafeed.Rest/Datafeed.Rest.csproj"
COPY . .
WORKDIR "/src/Services/Datafeed.Rest"
RUN dotnet build "Datafeed.Rest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Datafeed.Rest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Datafeed.Rest.dll"]