FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
LABEL test=true

COPY src/BikeDashboard .
RUN dotnet restore

RUN dotnet test /p:CollectCoverage=true /p:Include="[BikeDashboard*]*" /p:CoverletOutputFormat=opencover

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app
COPY --from=build-env /app/BikeDashboard/out/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "BikeDashboard.dll"]
