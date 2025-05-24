using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OP2MapJsonTools;
using static OP2MapJsonTools.OP2MapJsonTools;

// OP2MapJsonToolsCmd
// https://github.com/leviathan400/OP2MapJsonToolsCmd
// 
// Console app to:
// - Process .map into .json
// - Process .json into .map
//
// .NET 8 /  C# / Multi-platform Console Application
//
// Improvement over the original project 'JsonMap'. Meant to be a similar (console app) improved replacement.
// https://github.com/OutpostUniverse/JsonMap
//
//
// Outpost 2: Divided Destiny is a real-time strategy video game released in 1997.

namespace OP2MapJsonToolsCmd
{
    class OP2MapJsonToolsCmd
    {
        private static void Main(string[] args)
        {
            // Get the version of the OP2MapJsonTools library
            var assembly = typeof(OP2MapJsonTools.OP2MapJsonTools).Assembly;
            var version = assembly.GetName().Version;
            var versionStringLibrary = version != null ? $"v{version.Major}.{version.Minor}.{version.Build}.{version.MinorRevision}" : "version unknown";

            // Get the current version 
            var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = assemblyVersion != null ? $"v{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}" : "v0.6.5";

            Console.WriteLine($"OP2MapJsonToolsCmd {versionString}");
            Console.WriteLine($"Using OP2MapJsonTools library {versionStringLibrary}");
            Console.WriteLine("Converts Outpost 2 map files to/from JSON format");
            Console.WriteLine("===============================================");
            Console.WriteLine();

            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }

            try
            {
                ProcessArguments(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                Environment.Exit(1);
            }

            //Console.WriteLine();
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }

        private static void ProcessArguments(string[] args)
        {
            // Process arguments
            string inputFile = "";
            string outputFile = "";
            string mapName = "";
            string notes = "";
            JsonExportFormat format = JsonExportFormat.PerRowPadded; // Default format
            bool toJson = true; // Default operation

            // Parse command line arguments
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-i":
                    case "--input":
                        if (i + 1 < args.Length)
                            inputFile = args[++i];
                        break;

                    case "-o":
                    case "--output":
                        if (i + 1 < args.Length)
                            outputFile = args[++i];
                        break;

                    case "-n":
                    case "--name":
                        if (i + 1 < args.Length)
                            mapName = args[++i];
                        break;

                    case "--notes":
                        if (i + 1 < args.Length)
                            notes = args[++i];
                        break;

                    case "-f":
                    case "--format":
                        if (i + 1 < args.Length)
                        {
                            string formatStr = args[++i].ToLower();
                            switch (formatStr)
                            {
                                case "original":
                                    format = JsonExportFormat.Original;
                                    break;
                                case "perrow":
                                    format = JsonExportFormat.PerRow;
                                    break;
                                case "perrowpadded":
                                    format = JsonExportFormat.PerRowPadded;
                                    break;
                                default:
                                    throw new ArgumentException($"Invalid format: {formatStr}. Valid formats are: original, perrow, perrowpadded");
                            }
                        }
                        break;

                    case "--tojson":
                        toJson = true;
                        break;

                    case "--tomap":
                        toJson = false;
                        break;

                    case "-h":
                    case "--help":
                        ShowUsage();
                        return;

                    case "-v":
                    case "--version":
                        ShowVersion();
                        return;

                    default:
                        // If it doesn't start with a dash, treat it as input file if not set
                        if (!args[i].StartsWith("-") && string.IsNullOrEmpty(inputFile))
                        {
                            inputFile = args[i];
                        }
                        break;
                }
            }

            // Validate required arguments
            if (string.IsNullOrEmpty(inputFile))
            {
                throw new ArgumentException("Input file is required. Use -i or --input to specify the input file.");
            }

            if (!File.Exists(inputFile))
            {
                throw new FileNotFoundException($"Input file not found: {inputFile}");
            }

            // Auto-detect operation if not explicitly specified
            if (args.All(arg => !arg.Equals("--tojson", StringComparison.OrdinalIgnoreCase) &&
                               !arg.Equals("--tomap", StringComparison.OrdinalIgnoreCase)))
            {
                string extension = Path.GetExtension(inputFile).ToLower();
                toJson = extension == ".map";
            }

            // Generate output file if not specified
            if (string.IsNullOrEmpty(outputFile))
            {
                if (toJson)
                {
                    outputFile = Path.ChangeExtension(inputFile, ".json");
                }
                else
                {
                    outputFile = Path.ChangeExtension(inputFile, ".map");
                }
            }

            Console.WriteLine($"Input file: {inputFile}");
            Console.WriteLine($"Output file: {outputFile}");
            Console.WriteLine($"Operation: {(toJson ? "Map to JSON" : "JSON to Map")}");

            if (toJson)
            {
                Console.WriteLine($"Format: {format}");
                if (!string.IsNullOrEmpty(mapName))
                    Console.WriteLine($"Map name: {mapName}");
                if (!string.IsNullOrEmpty(notes))
                    Console.WriteLine($"Notes: {notes}");
            }

            Console.WriteLine();

            // Perform the conversion
            if (toJson)
            {
                ConvertMapToJson(inputFile, outputFile, format, mapName, notes);
            }
            else
            {
                ConvertJsonToMap(inputFile, outputFile);
            }
        }

        private static void ConvertMapToJson(string mapFile, string jsonFile, JsonExportFormat format,
                                           string mapName, string notes)
        {
            Console.WriteLine("Converting map to JSON...");

            // Use the OP2MapJsonTools library to export (passing empty string for author parameter)
            OP2MapJsonTools.OP2MapJsonTools.ExportMapToJsonFile(mapFile, jsonFile, format, mapName, "", notes);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully converted to JSON file");
            Console.ResetColor();

            ShowJsonFormatInfo(format);
        }

        private static void ConvertJsonToMap(string jsonFile, string mapFile)
        {
            Console.WriteLine("Converting JSON to map...");

            // Use the OP2MapJsonTools library to import
            OP2MapJsonTools.OP2MapJsonTools.ExportMapFile(jsonFile, mapFile);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully converted to MAP file");
            Console.ResetColor();
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  OP2JsonMapConsole [options] <input-file>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -i, --input <file>     Input file (.map or .json)");
            Console.WriteLine("  -o, --output <file>    Output file (auto-generated if not specified)");
            Console.WriteLine("  -n, --name <name>      Map name (for JSON export)");
            Console.WriteLine("  --notes <notes>        Map notes (for JSON export)");
            Console.WriteLine("  -f, --format <format>  JSON export format (for map->json conversion)");
            Console.WriteLine("                         Options: original, perrow, perrowpadded (default)");
            Console.WriteLine("  --tojson               Force conversion to JSON");
            Console.WriteLine("  --tomap                Force conversion to MAP");
            Console.WriteLine("  -h, --help             Show this help message");
            Console.WriteLine("  -v, --version          Show version information");
            Console.WriteLine();
            Console.WriteLine("JSON Export Formats:");
            Console.WriteLine("  original      - Flat arrays (like C++ implementation)");
            Console.WriteLine("  perrow        - Each map row as separate array on one line");
            Console.WriteLine("  perrowpadded  - Same as perrow but with number padding for alignment");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  # Convert map to JSON with default settings");
            Console.WriteLine("  OP2JsonMapConsole mymap.map");
            Console.WriteLine();
            Console.WriteLine("  # Convert map to JSON with metadata and specific format");
            Console.WriteLine("  OP2JsonMapConsole -i mymap.map -o output.json -n \"My Map\" -f perrowpadded");
            Console.WriteLine();
            Console.WriteLine("  # Convert JSON back to map");
            Console.WriteLine("  OP2JsonMapConsole mymap.json");
            Console.WriteLine();
            Console.WriteLine("  # Specify explicit conversion direction");
            Console.WriteLine("  OP2JsonMapConsole --tojson mymap.map");
            Console.WriteLine("  OP2JsonMapConsole --tomap mymap.json");
        }

        private static void ShowVersion()
        {
            // Get the current version 
            var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = assemblyVersion != null ? $"v{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}" : "v0.6.5";

            Console.WriteLine($"OP2MapJsonToolsCmd {versionString}");
            Console.WriteLine("Built with OP2MapJsonTools library");
            Console.WriteLine();
        }

        private static void ShowJsonFormatInfo(JsonExportFormat format)
        {
            //Console.WriteLine();
            //Console.WriteLine("JSON Format Details:");
            //switch (format)
            //{
            //    case JsonExportFormat.Original:
            //        Console.WriteLine("- Original: Flat arrays similar to C++ implementation");
            //        Console.WriteLine("- All tiles and cell types stored in continuous arrays");
            //        break;
            //    case JsonExportFormat.PerRow:
            //        Console.WriteLine("- PerRow: Each map row stored as separate array on single line");
            //        Console.WriteLine("- Easier to correlate JSON data with actual map layout");
            //        break;
            //    case JsonExportFormat.PerRowPadded:
            //        Console.WriteLine("- PerRowPadded: Same as PerRow with number padding for alignment");
            //        Console.WriteLine("- Most readable format when viewed in text editor");
            //        break;
            //}
            //Console.WriteLine();
            //Console.WriteLine("The JSON includes the following sections:");
            //Console.WriteLine("• header - Map dimensions and metadata");
            //Console.WriteLine("• tiles - Visual layer tile mapping indices");
            //Console.WriteLine("• cellTypes - Gameplay layer (movement/passability)");
            //Console.WriteLine("• clipRect - Visible area definition (if not default)");
            //Console.WriteLine("• tileset - Tileset sources, mappings, and terrain types");
        }

    }
}
