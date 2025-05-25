# OP2MapJsonToolsCmd

![Screenshot](https://images.outpostuniverse.org/OP2MapJsonToolsCmd.png)

## What is it?

Console utility application to convert Outpost 2: Divided Destiny map files to/from json format.

Inspired by the original [JsonMap](https://github.com/OutpostUniverse/JsonMap) project.


Uses the [OP2MapJsonToolsLibrary](https://github.com/leviathan400/OP2MapJsonToolsLibrary) to convert to/from json.

### Useage

**Convert .map to .json:**
```bash
OP2MapJsonToolsCmd.exe eden01.map
```

**Convert .json to .map:**
```bash
OP2MapJsonToolsCmd.exe eden01.json
```

**Show help:**
```bash
OP2MapJsonToolsCmd.exe --help
```

### Command Line Options

| Option | Description |
|--------|-------------|
| `-i, --input <file>` | Input file (.map or .json) |
| `-o, --output <file>` | Output file (auto-generated if not specified) |
| `-n, --name <name>` | Map name (for JSON export) |
| `--notes <notes>` | Map notes (for JSON export) |
| `-f, --format <format>` | JSON export format (original, perrow, perrowpadded) |
| `--tojson` | Force conversion to JSON |
| `--tomap` | Force conversion to MAP |
| `-h, --help` | Show help message |
| `-v, --version` | Show version information |


### JSON Map Format

The JSON format includes the following sections:
- header: Map dimensions and metadata
- tiles: Visual layer tile mapping indices
- cellTypes: Gameplay layer (movement/passability)
- clipRect: Visible area definition (if not default)
- tileset: Tileset sources, mappings, and terrain types

## JSON Map Format Details

### 1. Header Section
Contains basic map information and metadata:

```json
{
  "header": {
    "width": 64,            // Map width in tiles
    "height": 64,           // Map height in tiles  
    "map": "eden01.map",    // Original filename
    "name": "Eden Station", // Display name
    "author": "Dynamix",    // Map creator
    "notes": "Campaign map" // Additional notes
  }
}
```

### 2. Tiles Array
Visual layer containing tile mapping indices:

```json
{
  "tiles": [
    [ 1898, 1899, 1900, 1901 ],  // Row 1
    [ 1904, 1905, 1906, 1907 ],  // Row 2  
    [ 1910, 1911, 1912, 1913 ]   // Row 3
  ]
}
```

### 3. Cell Types Array  
Gameplay layer defining movement and passability:

```json
{
  "cellTypes": [
    [ 0, 0, 1, 1 ],  // FastPassible1, FastPassible1, Impassible2, Impassible2
    [ 2, 2, 3, 3 ],  // SlowPassible1, SlowPassible1, SlowPassible2, SlowPassible2
    [ 4, 4, 5, 5 ]   // MediumPassible1, MediumPassible1, MediumPassible2, MediumPassible2
  ]
}
```

### 4. Clip Rectangle (Optional)
Defines the visible/playable area for world-wrapping maps. World maps are 512 tiles wide and wrap horizontally. The clipRect specifies which portion of the map is actually visible and playable in-game. clipRect is only included in JSON when different from default values.
 
```json
{
  "clipRect": {
    "x1": 32,   // Left boundary
    "y1": 0,    // Top boundary  
    "x2": 287,  // Right boundary
    "y2": 254   // Bottom boundary
  }
}
```

### 5. Tileset Information
Complete tileset data including sources, mappings, and terrain definitions:

```json
{
  "tileset": {
    "sources": [
      {
        "filename": "well0001.bmp",
        "numTiles": 295
      }
    ],
    "tileMappings": [
      {
        "tilesetIndex": 0,
        "tileGraphicIndex": 14,
        "animationCount": 0,
        "animationDelay": 0
      }
    ],
    "terrainTypes": [
      {
        "tileRange": { "start": 0, "end": 10 },
        "bulldozedTileMappingIndex": 120,
        "rubbleTileMappingIndex": 121,
        "tubeTileMappings": [122, 123, 124, 125, 126, 127],
        "wallTileMappingIndexes": [[128, 129, 130, 131]],
        "lavaTileMappingIndex": 140,
        "flat1": 141,
        "flat2": 142, 
        "flat3": 143,
        "scorchedTileMappingIndex": 160,
        "scorchedRange": [{ "start": 161, "end": 165 }],
        "unknown": [-1, -1, -1, -1]
      }
    ]
  }
}
```

### Complete Example

```json
{
  "header": {
    "width": 64,
    "height": 64,
    "map": "eden01.map",
    "name": "Eden Campaign 01", 
    "author": "Dynamix",
    "notes": "Original campaign map"
  },
  "tiles": [
    [ 1898, 1899, 1900, 1901, 1902, 1903 ],
    [ 1904, 1905, 1906, 1907, 1908, 1909 ],
    [ 1910, 1911, 1912, 1913, 1914, 1915 ]
  ],
  "cellTypes": [
    [ 0, 0, 1, 1, 2, 2 ],
    [ 3, 3, 4, 4, 5, 5 ],
    [ 6, 6, 7, 7, 8, 8 ]
  ],
  "clipRect": {
    "x1": 32,
    "y1": 0, 
    "x2": 287,
    "y2": 254
  },
  "tileset": {
    "sources": [
      {
        "filename": "well0001",
        "numTiles": 269
      }
    ],
    "tileMappings": [
      {
        "tilesetIndex": 0,
        "tileGraphicIndex": 14,
        "animationCount": 0,
        "animationDelay": 0
      }
    ],
    "terrainTypes": [
      {
        "tileRange": {
          "start": 0,
          "end": 10
        },
        "bulldozedTileMappingIndex": 120,
        "rubbleTileMappingIndex": 121,
        "tubeTileMappings": [122, 123, 124, 125, 126, 127],
        "wallTileMappingIndexes": [
          [128, 129, 130, 131],
          [132, 133, 134, 135]
        ],
        "lavaTileMappingIndex": 140,
        "flat1": 141,
        "flat2": 142,
        "flat3": 143,
        "tubeTileMappingIndexes": [150, 151, 152, 153, 154, 155],
        "scorchedTileMappingIndex": 160,
        "scorchedRange": [
          {
            "start": 161,
            "end": 165
          }
        ],
        "unknown": [-1, -1, -1, -1]
      }
    ]
  }
}
```

## Export Format Options

### Original Format
Flat arrays similar to the original C++ implementation:
```json
{
  "tiles": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
  "cellTypes": [0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5]
}
```

### PerRowPadded Format
Each row as a separate array for better readability with consistent formatting:
```json
{
  "tiles": [
    [   1,    2,    3,    4 ],
    [   5,    6,    7,    8 ],
    [   9,   10,   11,   12 ]
  ]
}
```
