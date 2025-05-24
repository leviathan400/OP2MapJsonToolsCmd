# OP2MapJsonToolsCmd

![Screenshot](https://images.outpostuniverse.org/OP2MapJsonToolsCmd.png)

## What is it?

Console app to convert Outpost 2 MAP files to/from JSON format.

Uses the OP2MapJsonToolsLibrary

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