FROM mcr.microsoft.com/dotnet/sdk:7.0.103-alpine3.16 AS build-env
WORKDIR /app
LABEL test=true

COPY src/BikeDashboard/BikeDashboard BikeDashboard
COPY src/BikeDashboard/TestBikedashboard  TestBikedashboard
COPY src/BikeDashboard/BikeDashboard.sln BikeDashboard.sln

RUN dotnet restore

ENV CollectCoverage=true
ENV Include="[BikeDashboard*]*"
ENV CoverletOutputFormat=opencover

RUN dotnet test

RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0.3-alpine3.16
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app

COPY --from=build-env /app/publish/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "BikeDashboard.dll"]
