# BikeDashboard

Simple website to track my closest Bikeshare station.

To override default station with env variables:

```bash
docker run -it -p 5000:5000 -e GBFSAddress="http://gbfs.urbansharing.com/trondheim/gbfs.json" -e StationName="Lerkendal" ghcr.io/andmos/bikedashboard
```

Local weather services from [OpenWeatherMap](https://openweathermap.org) can be added to the BikeDashboard by providing an API-key:

```bash
docker run -it -p 5000:5000 -e GBFSAddress="http://gbfs.urbansharing.com/trondheim/gbfs.json" -e StationName="Skansen" -e WeatherServiceAPIKey="" ghcr.io/andmos/bikedashboard
```

All compatible GBFS systems can be found [here](https://github.com/NABSA/gbfs/blob/master/systems.csv).

[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=andmos/BikeDashboard)](https://dependabot.com)

[![CI / CD](https://github.com/andmos/BikeDashboard/actions/workflows/ci.yaml/badge.svg)](https://github.com/andmos/BikeDashboard/actions/workflows/ci.yaml)
