using System.Diagnostics;

namespace lab_1_useinov_ip31
{
    public class Program
    {
        public static void Main()
        {
            List<string> actions = ["sort", "modsort"];
            List<string> generateOptions = ["yes", "no"];
            string choice;
            string needToGenerate;
            string path;

            do
            {
                Console.WriteLine("Generate file randomly (yes or no)?");
                needToGenerate = Console.ReadLine() ?? "yes";
                if (!generateOptions.Contains(needToGenerate))
                    Console.WriteLine("Incorrect entering. Choose yes or no");
            } while (!generateOptions.Contains(needToGenerate));

            if (needToGenerate == "no")
            {
                Console.WriteLine("Enter your file path: ");
                path = Console.ReadLine() ?? Environment.CurrentDirectory + "\\A.bin";
            }
            else
            {
                path = Environment.CurrentDirectory + "\\A.bin";
            }

            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists && fileInfo.Length == 0 && needToGenerate == "no")
            {
                Console.WriteLine("Create the file to start working");
                Environment.Exit(1);
            }

            if (needToGenerate == "no" && !fileInfo.Exists)
            {
                Console.WriteLine("Create the file to start working");
                Environment.Exit(1);
            }

            do
            {
                Console.WriteLine("Enter kind of sorting (sort or modsort): ");
                choice = Console.ReadLine() ?? "modsort";
                if (!actions.Contains(choice))
                    Console.WriteLine("Incorrect entering. Choose sort or modsort");
            } while (!actions.Contains(choice));

            Console.WriteLine("Start of the main action...");
            Stopwatch sw = new();
            sw.Start();

            long fileSize;

            if (choice == "sort")
            {
                if (needToGenerate == "yes")
                {
                    fileSize = 10 * 1024 * 1024;
                    FileCreator.CreateFile(path, fileSize);
                }
                ExternalSorting.StartSorting(path);
                // BinaryFileGetter.GetData(path);
            }
            else if (choice == "modsort")
            {
                if (needToGenerate == "yes")
                {
                    fileSize = 1024 * 1024 * 1024;
                    FileCreator.CreateFile(path, fileSize);
                }
                long chunkSize = 128 * 1024 * 1024;
                ModExternalSorting.StartSorting(path, chunkSize);
                // BinaryFileGetter.GetData(path);
            }

            sw.Stop();
            Console.WriteLine("End of working...");

            Console.WriteLine($"Results:\n" +
                $"Time of working: " +
                $"{sw.ElapsedMilliseconds} ms; " +
                $"{sw.ElapsedMilliseconds / 1000.0} s; " +
                $"{sw.ElapsedMilliseconds / 1000.0 / 60.0} m\n");

            Console.ReadLine();
        }
    }
}