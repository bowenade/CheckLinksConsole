using System;
using System.Net.Http;
using System.IO;
class Program
{
    static void Main(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var outputFolder = "resports";
        var outputFile = "report.txt";
        var outputPath = Path.Combine(currentDirectory, outputFolder, outputFile);
        var directory = Path.GetDirectoryName(outputPath);
        Directory.CreateDirectory(directory);
        Console.WriteLine($"Saving report to {outputPath}");

        var site = "http://www.google.com.au";
        var client = new HttpClient();
        var body = client.GetStringAsync(site);
        Console.WriteLine(body.Result);
        Console.WriteLine();
        Console.WriteLine("Links");
        var links = LinkChecker.GetLinks(body.Result);
        links.ToList().ForEach(link => Console.WriteLine(link));
        // write out links
        // File.WriteAllLines(outputPath, links);
        var checkedLinks = LinkChecker.CheckLinks(links);
        using (var file = File.CreateText(outputPath))
        {
            foreach (var link in checkedLinks)
            {
                var status = link.IsMissing ? "missing" : "OK";
                file.WriteLine($"{status} - {link.Link}");
            }
        }
    }
}