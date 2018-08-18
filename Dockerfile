FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY src/BikeDashboard .
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app
COPY --from=build-env /app/BikeDashboard/out/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "BikeDashboard.dll"]

