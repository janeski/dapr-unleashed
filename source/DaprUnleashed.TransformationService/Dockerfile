#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["source/DaprUnleashed.TransformationService/DaprUnleashed.TransformationService.csproj", "source/DaprUnleashed.TransformationService/"]
RUN dotnet restore "source/DaprUnleashed.TransformationService/DaprUnleashed.TransformationService.csproj"
COPY . .
WORKDIR "/src/source/DaprUnleashed.TransformationService"
RUN dotnet build "DaprUnleashed.TransformationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprUnleashed.TransformationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprUnleashed.TransformationService.dll"]