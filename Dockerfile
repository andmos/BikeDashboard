FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

COPY src/BikeDashboard .
RUN dotnet restore

RUN dotnet test /p:CollectCoverage=true /p:Include="[BikeDashboard*]*"

RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app
COPY --from=build-env /app/BikeDashboard/out/ .

EXPOSE 5000
ENTRYPOINT ["dotnet", "BikeDashboard.dll"]
