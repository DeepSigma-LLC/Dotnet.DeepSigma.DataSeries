# DeepSigma.DataSeries

A C# framework for building, combining, and transforming ordered data sets and data series.

`DeepSigma.DataSeries` provides strongly typed models and containers for working with categorical and time-indexed data, including support for common analytical transforms such as returns, drawdowns, wealth indexing, moving averages, standard deviation, scaling, and lagging.

## Features

- Strongly typed data models such as:
  - `Observation`
  - `BarObservation`
  - `BarObservationWithVolume`
  - `BidAskSpreadObservation`
  - `TradeObservation`
- Ordered data storage using `SortedDictionary`
- Generic `DataSet<TKey, TValue>` support for keyed collections
- Base series types for:
  - time series
  - categorical series
  - scalar / keyed data series
- Composite “series of series” support for combining multiple sub-series
- Mathematical combination of sub-series
  - add
  - subtract
  - multiply
  - divide
- Built-in transformation pipeline with support for:
  - scaling
  - lagging
  - returns
  - wealth indexing
  - reverse wealth
  - drawdown amount
  - drawdown percentage
  - averages
  - standard deviation
  - custom point/reference/vector transforms
- Time-series downsampling support
- XML documentation generation enabled in the package project

## Project structure

```text
DeepSigma.DataSeries/
├── Accumulators/
├── DataModels/
├── Enums/
├── Interfaces/
├── Models/
├── Series/
├── SeriesOfSeries/
├── Transformations/
└── Utilities/

DeepSigma.DataSeries.Tests/
├── Model/
├── Tests/
│   ├── DataSets/
│   └── Series/
└── TestScript.cs
```

## Target framework

This library currently targets:

- `net10.0`

## Package metadata

- Package ID / title: `DeepSigma.DataSeries`
- Version: `1.8.0.0`
- Author: `DeepSigma LLC`

## Dependency

The library references:

- `DeepSigma.General` `2.8.0`

## Core concepts

### 1. Data models

Data points are represented by typed models that implement the library’s data model interfaces.

Examples include:

- `Observation` for a single numeric value
- `BarObservation` for OHLC-style values
- `BarObservationWithVolume`
- `BidAskSpreadObservation`
- `TradeObservation`

These models also expose accumulator behavior used by the library’s transformation and aggregation pipeline.

### 2. Data sets

`DataSet<TKey, TValue>` stores ordered keyed data using `SortedDictionary<TKey, TValue>`.

Use it when you want a typed collection of observations and later extract a single property as its own series.

### 3. Base series

The library includes base series containers for ordered data, including:

- `TimeSeriesBase<TDateKey, TValueDataType>`
- `CategoricalSeriesBase<TKeyDataType, TValueDataType>`
- `DataSeriesBase<TKeyDataType, TValueDataType>`

These wrap sorted keyed data and provide unscaled and transformed views.

### 4. Composite series

Higher-level types such as `TimeSeries<TDate, TValueDataType>` and `CategoricalSeries<TValueDataType>` let you combine multiple sub-series into one logical series.

This is useful for cases like:

- combining multiple factors
- constructing spread series
- comparing multiple categorized inputs
- creating arithmetic combinations of independent time series

### 5. Transformations

Transformations are configured through:

- `SeriesTransformation<TData>`
- `TimeSeriesTransformation<TData>`

Supported configuration includes:

- `Scalar`
- `ObservationWindowCount`
- `Transformation`
- `ObservationLag`
- `DaySelectionTypeForLag`
- custom transformation delegates

## Installation

### Option 1: Project reference

```xml
<ProjectReference Include="..\DeepSigma.DataSeries\DeepSigma.DataSeries.csproj" />
```

### Option 2: Build from source

```bash
git clone https://github.com/DeepSigma-LLC/Dotnet.DeepSigma.DataSeries.git
cd Dotnet.DeepSigma.DataSeries
dotnet build
```

## Quick start

### Create a simple data set

```csharp
using DeepSigma.DataSeries.Models;
using DeepSigma.DataSeries.DataModels;

DataSet<int, Observation> dataSet = new();

dataSet.Add(1, new Observation(11m));
dataSet.Add(2, new Observation(12m));
dataSet.Add(3, new Observation(13m));
```

### Create a base time series

```csharp
using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Series;

var data = new SortedDictionary<DateTime, Observation>
{
    { new DateTime(2024, 1, 1), new Observation(100m) },
    { new DateTime(2024, 1, 2), new Observation(105m) },
    { new DateTime(2024, 1, 3), new Observation(110m) }
};

TimeSeriesBase<DateTime, Observation> baseSeries = new(data);
```

### Use implicit conversion to build a time series base

```csharp
TimeSeriesBase<DateTime, Observation> baseSeries = data;
```

### Combine multiple time sub-series

```csharp
using DeepSigma.DataSeries.DataModels;
using DeepSigma.DataSeries.Series;
using DeepSigma.General.Enums;

var left = new SortedDictionary<DateTime, Observation>
{
    { new DateTime(2024, 1, 1), new Observation(1m) },
    { new DateTime(2024, 1, 2), new Observation(2m) },
    { new DateTime(2024, 1, 3), new Observation(3m) }
};

var right = new SortedDictionary<DateTime, Observation>
{
    { new DateTime(2024, 1, 1), new Observation(3m) },
    { new DateTime(2024, 1, 2), new Observation(4m) },
    { new DateTime(2024, 1, 3), new Observation(5m) }
};

TimeSeriesBase<DateTime, Observation> leftBase = new(left);
TimeSeriesBase<DateTime, Observation> rightBase = new(right);

TimeSeries<DateTime, Observation> series = new()
{
    SeriesName = "Example Series"
};

series.Add(leftBase);
series.Add(rightBase, MathematicalOperation.Add);

var combined = series.GetSeriesDataTransformed();
```

### Apply a transformation

```csharp
using DeepSigma.DataSeries.Enums;

series.Transformation.Scalar = 2m;
series.Transformation.Transformation = Transformation.Return;
series.Transformation.ObservationWindowCount = 2;

var transformed = series.GetSeriesDataTransformed();
```

### Configure lag behavior on a time series

```csharp
using DeepSigma.General.Enums;

series.Transformation.ObservationLag = 1;
series.Transformation.DaySelectionTypeForLag = DaySelectionType.Any;

var lagged = series.GetSeriesDataTransformed();
```

## Example use cases

- Financial time series analytics
- Derived series construction
- Portfolio / benchmark comparison
- Spread and ratio calculations
- Categorical business metrics
- Ordered observation pipelines with reusable transforms

## Tested behavior

The current test suite validates behavior such as:

- adding multiple values to a `DataSet`
- duplicate-key rejection in `DataSet`
- categorical series initialization
- combining multiple time sub-series with arithmetic operations
- divide-by-zero handling
- series scaling
- lagging by observation / day rules
- return calculations
- wealth and reverse-wealth transforms
- drawdown amount and drawdown percentage
- moving averages
- standard deviation
- minimum and maximum key detection across multiple sub-series

## Notes

- The library uses ordered dictionaries heavily, so key ordering is part of the design.
- `DataSet<TKey, TValue>.Add(...)` throws on duplicate keys because it relies on `SortedDictionary`.

## Running tests

```bash
dotnet test
```

## Repository

GitHub repository:

`https://github.com/DeepSigma-LLC/Dotnet.DeepSigma.DataSeries`

## Author

DeepSigma LLC
