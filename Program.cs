using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvFlatten
{
    class Program
    {
        static bool skipHeader;

        static IEnumerable<string[]> ReadCsv(string file)
        {
            // For some obscure historical reason, the CSV parser is in the Visual Basic library
            // So to make this work in a new project, right click project, Add Reference, Microsoft.VisualBasic
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                if (skipHeader)
                    parser.ReadFields();
                while (!parser.EndOfData)
                    yield return parser.ReadFields();
            }
        }

        static void Main(string[] args)
        {
            string file = null;
            for (int i = 0; i < args.Length; i++)
            {
                var s = args[i];

                // On Windows, options can start with /
                if (Path.DirectorySeparatorChar == '\\' && s.StartsWith("/"))
                    s = "-" + s.Substring(1);

                // Not an option
                if (!s.StartsWith("-"))
                {
                    if (file != null)
                    {
                        Console.WriteLine(args[i] + ": expected one file");
                        Environment.Exit(1);
                    }
                    file = s;
                    continue;
                }

                // Option
                s = s.TrimStart('-');
                switch (s)
                {
                    case "?":
                    case "h":
                    case "help":
                        Console.WriteLine("Options:");
                        Console.WriteLine();
                        Console.WriteLine("-h  Show help");
                        Console.WriteLine("-v  Show version");
                        Console.WriteLine();
                        Console.WriteLine("-s  Skip header line");
                        return;
                    case "V":
                    case "v":
                    case "version":
                        var assemblyName = Assembly.GetExecutingAssembly().GetName();
                        Console.WriteLine("{0} {1}", assemblyName.Name, assemblyName.Version.ToString(2));
                        return;
                    case "s":
                    case "skip":
                    case "skip-header":
                        skipHeader = true;
                        break;
                    default:
                        Console.WriteLine(args[i] + ": unknown option");
                        Environment.Exit(1);
                        break;
                }
            }
            if (file == null)
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName();
                Console.WriteLine(string.Format("Usage: {0} <file>", assemblyName.Name));
                Environment.Exit(1);
            }
            try
            {
                foreach (var line in ReadCsv(file))
                {
                    foreach (var cell in line)
                    {
                        Console.WriteLine(cell);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
