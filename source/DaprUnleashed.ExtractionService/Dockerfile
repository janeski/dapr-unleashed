#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["source/DaprUnleashed.ExtractionService/DaprUnleashed.ExtractionService.csproj", "source/DaprUnleashed.ExtractionService/"]
COPY ["source/DaprUnleashed.DomainModel/DaprUnleashed.DomainModel.csproj", "source/DaprUnleashed.DomainModel/"]
RUN dotnet restore "source/DaprUnleashed.ExtractionService/DaprUnleashed.ExtractionService.csproj"
COPY . .
WORKDIR "/src/source/DaprUnleashed.ExtractionService"
RUN dotnet build "DaprUnleashed.ExtractionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprUnleashed.ExtractionService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprUnleashed.ExtractionService.dll"]