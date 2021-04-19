FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
LABEL test=true

COPY src/BikeDashboard .
RUN dotnet restore

RUN dotnet test /p:CollectCoverage=true /p:Include="[BikeDashboard*]*" /p:CoverletOutputFormat=opencover

RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0.5-alpine3.12
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app

COPY --from=build-env /app/publish/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "BikeDashboard.dll"]
