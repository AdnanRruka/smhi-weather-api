# SMHI Weather API

A REST API that fetches and merges weather observations (temperature and gust wind) from [SMHI Open Data](https://opendata.smhi.se/) and exposes them through a single endpoint.

Built with ASP.NET Core .NET 10 Minimal API.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Getting started

```bash
git clone <repository-url>
cd SMHIWeatherAPI/SMHIWeatherAPI
dotnet run
```

The API starts on `https://localhost:7173` by default.

Swagger UI is available at `https://localhost:7173/swagger` (development only).

## Authentication

The API uses JWT Bearer authentication. Obtain a token from the `/token` endpoint:

```bash
curl -X POST https://localhost:7173/token \
  -H "Content-Type: application/json" \
  -d '{"password": "demo"}'
```

Use the returned token as a Bearer token on all subsequent requests:

```
Authorization: Bearer <token>
```

In Swagger UI, click **Authorize** and paste the token.

## Endpoints

### GET /measurements

Returns merged temperature and gust wind observations per station.

| Query parameter | Type    | Required | Description                              |
|-----------------|---------|----------|------------------------------------------|
| `stationId`     | integer | No       | Filter by a specific SMHI station ID     |
| `period`        | string  | No       | `latest-hour` (default) or `latest-day` |

**Examples**

All stations, latest hour (default):
```
GET /measurements
```

Specific station, latest day:
```
GET /measurements?stationId=188790&period=latest-day
```

**Response**

```json
[
  {
    "stationId": "188790",
    "stationName": "Abisko Aut",
    "latitude": 68.3555,
    "longitude": 18.8211,
    "temperature": 11.4,
    "gustWind": 3.2,
    "timestamp": "2024-06-28T10:00:00+00:00"
  }
]
```

`temperature` and `gustWind` are nullable — a station may have data for one but not both.

## Running tests

```bash
cd SMHIWeatherAPI
dotnet test .\SMHIWeatherAPI.Tests\
```

## Known shortcuts

See [SHORTCUTS.md](SHORTCUTS.md) for a list of shortcuts and the reasoning behind them.
