FROM mcr.microsoft.com/dotnet/sdk:5.0.401-alpine3.13 AS build-env
WORKDIR /app
LABEL test=true

COPY src/BikeDashboard/BikeDashboard BikeDashboard
COPY src/BikeDashboard/TestBikedashboard  TestBikedashboard
COPY src/BikeDashboard/BikeDashboard.sln BikeDashboard.sln

RUN dotnet restore

RUN dotnet test /p:CollectCoverage=true /p:Include="[BikeDashboard*]*" /p:CoverletOutputFormat=opencover

RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0.10-alpine3.13
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app

COPY --from=build-env /app/publish/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "BikeDashboard.dll"]
