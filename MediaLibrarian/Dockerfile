#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update && apt-get install -y ffmpeg

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MediaLibrarian/MediaLibrarian.csproj", "MediaLibrarian/"]
RUN dotnet restore "MediaLibrarian/MediaLibrarian.csproj"
COPY . .
WORKDIR "/src/MediaLibrarian"
RUN dotnet build "MediaLibrarian.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediaLibrarian.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediaLibrarian.dll"]