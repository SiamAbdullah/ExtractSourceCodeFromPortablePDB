
namespace ExtractSourceCodeFromPortablePDB
{
    using System;
    using System.IO;
    using System.Reflection.Metadata;

    class Program
    {
        static void Main(string[] args)
        {
            if(!ParseArgument(args, out var inputDirectory, out var outputDirectory))
            {
                ShowUsage();
                return;
            }

            var pdbFiles = Directory.GetFiles(inputDirectory, "*.pdb", SearchOption.AllDirectories);
            foreach (var pdbFile in pdbFiles)
            {
                ParsePdb(pdbFile, outputDirectory);
            }

            Console.WriteLine("Extracted source from portable PDB");
        }

        private static void ParsePdb(string inputFileName, string outputDirectory)
        {
            // create the output Directory if directory is not exit.
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (var fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                var pdbMeta = MetadataReaderProvider.FromPortablePdbStream(fs);
                var reader = pdbMeta.GetMetadataReader(MetadataReaderOptions.Default);
                var documents = reader.Documents;
                foreach (var document in documents)
                {
                    var content = reader.GetDocument(document);

                    // extract the source code file name
                    var embeddedSourceFileName = reader.GetString(content.Name);

                    // get the source code 
                    var code = reader.GetEmbeddedSource(document);

                    if (!string.IsNullOrEmpty(embeddedSourceFileName))
                    {
                        string outputFileName = Path.GetFileName(embeddedSourceFileName);
                        var fullPath = Path.Combine(outputDirectory, outputFileName);
                        File.WriteAllText(fullPath, code);
                    }
                }
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("ExtractSourceCodeFromPortablePDB.exe -i <input PDB Directory> -o <folder where source code will be written>");
        }

        private static bool ParseArgument(string[] args, out string inputDir, out string outputDir)
        {
            inputDir = null;
            outputDir = null;

            int count = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-i"))
                {
                    inputDir = args[++i];
                    count++;
                }
                else if (args[i].Equals("-o"))
                {
                    outputDir = args[++i];
                    count++;
                }
            }

            return count == 2;
        }
    }
}
