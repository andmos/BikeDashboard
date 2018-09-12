package main

import (
    "bufio"
    "context"
    "database/sql"
    "encoding/json"
    "flag"
    "fmt"
    "io/ioutil"
    "log"
    "net/http"
    "os"
    "strings"
    "sync"
    "time"

    "googlemaps.github.io/maps"

    _ "github.com/lib/pq"
)

var db *sql.DB

var (
    apiKey        string
    dsn           string
    locationsFile string
)

type HourlyForecast struct {
    Type       string
    Properties Properties
}

type Properties struct {
    Periods []Period
}

type Period struct {
    Temperature int
}

func main() {
    flag.StringVar(&apiKey, "api-key", "", "Google Maps API key")
    flag.StringVar(&dsn, "dsn", "", "The database connection string")
    flag.StringVar(&locationsFile, "locations", "locations.txt", "The locations file")
    flag.Parse()

    if apiKey == "" {
        apiKey = os.Getenv("GOOGLE_MAPS_API_KEY")
    }

    if apiKey == "" {
        log.Fatal("The --api-key flag or GOOGLE_MAPS_API_KEY env var must be set and non-empty")
    }

    apiKey = strings.TrimSpace(apiKey)

    if dsn == "" {
        dsn = os.Getenv("DSN")
    }

    var err error
    db, err = sql.Open("postgres", dsn)
    if err != nil {
        log.Fatal(err)
    }

    for {
        err := db.Ping()
        if err != nil {
            log.Println(err.Error())
            time.Sleep(3 * time.Second)
            continue
        }
        break
    }

    data, err := os.Open(locationsFile)
    if err != nil {
        log.Fatal(err)
    }

    var locations []string

    scanner := bufio.NewScanner(data)
    for scanner.Scan() {
        locations = append(locations, scanner.Text())

    }

    var wg sync.WaitGroup

    for _, location := range locations {
        wg.Add(1)
        location := location

        go func() {
            defer wg.Done()

            mc, err := maps.NewClient(maps.WithAPIKey(apiKey))
            if err != nil {
                log.Fatal(err)
            }

            r := maps.FindPlaceFromTextRequest{
                Input:     location,
                InputType: maps.FindPlaceFromTextInputTypeTextQuery,
            }

            response, err := mc.FindPlaceFromText(context.Background(), &r)
            if err != nil {
                log.Fatal(err)
            }

            pdr := maps.PlaceDetailsRequest{
                PlaceID: response.Candidates[0].PlaceID,
            }
            log.Printf("retrieving geo coordinates for %s", location)
            pdResponse, err := mc.PlaceDetails(context.Background(), &pdr)
            if err != nil {
                log.Fatal(err)
            }

            lat := pdResponse.Geometry.Location.Lat
            lng := pdResponse.Geometry.Location.Lng
            u := fmt.Sprintf("https://api.weather.gov/points/%.4f,%.4f/forecast/hourly", lat, lng)
            log.Printf("retrieving weather data for %s (%.4f,%.4f)", location, lat, lng)

            request, err := http.NewRequest("GET", u, nil)
            if err != nil {
                log.Fatal(err)
            }
            request.Header.Add("User-Agent", "Hightower Weather 1.0")
            request.Header.Add("Accept", "application/geo+json")

            weatherResponse, err := http.DefaultClient.Do(request)
            if err != nil {
                log.Fatal(err)
            }

            data, err := ioutil.ReadAll(weatherResponse.Body)
            if err != nil {
                log.Fatal(err)
            }

            weatherResponse.Body.Close()

            var forecast HourlyForecast
            if err := json.Unmarshal(data, &forecast); err != nil {
                log.Fatal(err)
            }

            log.Printf("setting temperature for %s to %d", location, forecast.Properties.Periods[0].Temperature)
            _, err = db.Exec(query, location, forecast.Properties.Periods[0].Temperature)
            if err != nil {
                log.Fatal(err)
            }
        }()
    }

    wg.Wait()
}

var query = `INSERT INTO weather (location, temperature)
VALUES ($1, $2)
ON CONFLICT (location)
DO UPDATE SET temperature = EXCLUDED.temperature;`