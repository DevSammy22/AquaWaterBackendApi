#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AquaWater.Api/AquaWater.Api.csproj", "AquaWater.Api/"]
COPY ["AquaWater.Data/AquaWater.Data.csproj", "AquaWater.Data/"]
COPY ["AquaWater.BusinessLogic/AquaWater.BusinessLogic.csproj", "AquaWater.BusinessLogic/"]
COPY ["AquaWater.Dto/AquaWater.Dto.csproj", "AquaWater.Dto/"]
COPY ["AquaWater.Domain/AquaWater.Domain.csproj", "AquaWater.Domain/"]
RUN dotnet restore "AquaWater.Api/AquaWater.Api.csproj"

COPY . .
WORKDIR /src/AquaWater.Api
RUN dotnet build
FROM build AS publish
WORKDIR /src/AquaWater.Api
RUN dotnet publish  -c Release -o /src/publish
FROM base AS final
WORKDIR /app

COPY --from=publish /src/publish .
COPY --from=publish /src/AquaWater.Api/Json/Admin.json ./
COPY --from=publish /src/AquaWater.Api/Json/CompanyManager.json ./
COPY --from=publish /src/AquaWater.Api/Json/Customer.json ./
COPY --from=publish /src/AquaWater.Api/Json/ProductGallery.json ./
COPY --from=publish /src/AquaWater.Api/Json/Products.json ./
COPY --from=publish /src/AquaWater.Api/Json/Rating.json ./
COPY --from=publish /src/AquaWater.Api/Json/Review.json ./
COPY --from=publish /src/AquaWater.Api/Json/Roles.json ./
COPY --from=publish /src/AquaWater.Api/Json/User.json ./
COPY --from=publish /src/AquaWater.Api/HtmlTemplate/EmailTemplate.html ./
COPY --from=publish /src/publish .
#ENTRYPOINT ["dotnet", "AquaWater.Api.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet AquaWater.Api.dll



