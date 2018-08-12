# BikeDashboard
Simple website to track my closest Bikeshare station.

To override default station with env variables:

```
docker run -it -p 5000:5000 -e GBFSAddress="http://gbfs.urbansharing.com/trondheim/" -e StationName="Lerkendal" andmos/bikedashboard
```
