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
                        Console.WriteLine("-help     Show help");
                        Console.WriteLine("-version  Show version");
                        return;
                    case "V":
                    case "v":
                    case "version":
                        var assemblyName = Assembly.GetExecutingAssembly().GetName();
                        Console.WriteLine("{0} {1}", assemblyName.Name, assemblyName.Version.ToString(2));
                        return;
                    default:
                        Console.WriteLine(args[i] + ": unknown option");
                        Environment.Exit(1);
                        break;
                }
            }
            try
            {
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
