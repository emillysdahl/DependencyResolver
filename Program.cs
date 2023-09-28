using test2;

for (int i = 0; i < 10; i++)
{
    var inputFile = $"C:\\Users\\el\\Desktop\\test2\\testdata\\input00{i}.txt";

    if (!File.Exists(inputFile))
    {
        Console.WriteLine("Input file not found.");
        return;
    }

    string inputFileContent = File.ReadAllText(inputFile.Trim().Replace("\n", "").Replace("\r", ""));

    string output = PackageInstaller.IsInstallationValid(inputFileContent) ? "PASS" : "FAIL";

    var outputFile = $"C:\\Users\\el\\Desktop\\test2\\testdata\\output00{i}.txt";

    string outputFileContent = File.ReadAllText(outputFile).Trim().Replace("\n", "").Replace("\r", "");

    Console.WriteLine($"input00{i}.txt     -->   {output}   (validation)");    
    Console.WriteLine($"output00{i}.txt:   -->   {outputFileContent}   (answer)");
    Console.WriteLine();
}
