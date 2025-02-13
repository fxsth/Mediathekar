#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
RUN apt-get update && apt-get install -y ffmpeg

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Mediathekar/Mediathekar.csproj", "Mediathekar/"]
RUN dotnet restore "Mediathekar/Mediathekar.csproj"
COPY . .
WORKDIR "/src/Mediathekar"
RUN dotnet build "Mediathekar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mediathekar.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mediathekar.dll", "--server.urls", "http://+5000"]