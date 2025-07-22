FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
WORKDIR /app
EXPOSE 8080

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

RUN apt-get update \
    && apt-get install -y locales \
    && locale-gen en_US.UTF-8 pt_BR.UTF-8 \
    && rm -rf /var/lib/apt/lists/*

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY Directory.Packages.props ./
COPY CapitalNerd.Laranjinhai.sln ./
COPY src/ ./src/

RUN dotnet nuget locals all --clear
RUN dotnet clean "CapitalNerd.Laranjinhai.sln"
RUN dotnet restore "CapitalNerd.Laranjinhai.sln"
RUN dotnet publish "CapitalNerd.Laranjinhai.sln" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CapitalNerd.Laranjinhai.API.dll"]
