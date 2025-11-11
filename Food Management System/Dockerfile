# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Food Management System/Food Management System.csproj", "Food Management System/"]
COPY ["FOOD.DATA/FOOD.DATA.csproj", "FOOD.DATA/"]
COPY ["FOOD.SERVICES/FOOD.SERVICES.csproj", "FOOD.SERVICES/"]
COPY ["FOOD.MODEL/FOOD.MODEL.csproj", "FOOD.MODEL/"]
RUN dotnet restore "./Food Management System/Food Management System.csproj"
COPY . .
WORKDIR "/src/Food Management System"
RUN dotnet build "./Food Management System.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Food Management System.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Food Management System.dll"]