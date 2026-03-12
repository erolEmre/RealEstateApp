FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY RealEstateApp/RealEstateApp.WebUI.csproj RealEstateApp/
COPY RealEstate.Application/RealEstate.Application.csproj RealEstate.Application/
COPY RealEstate.Core/RealEstate.Core.csproj RealEstate.Core/
COPY RealEstate.Infrastructure/RealEstate.Infrastructure.csproj RealEstate.Infrastructure/

RUN dotnet restore RealEstateApp/RealEstateApp.WebUI.csproj

COPY . .
RUN dotnet publish RealEstateApp/RealEstateApp.WebUI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8081

ENTRYPOINT ["dotnet", "RealEstateApp.WebUI.dll"]