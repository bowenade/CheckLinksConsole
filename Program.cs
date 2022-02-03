using System;
using System.Net.Http;

class Program
{
    static void Main(string[] args)
    {
        var site = "http://www.google.com.au";
        var client = new HttpClient();
        var body = client.GetStringAsync(site);
        Console.WriteLine(body.Result);
        Console.WriteLine();
        Console.WriteLine("Links");
        var links = LinkChecker.GetLinks(body.Result);
        links.ToList().ForEach(link => Console.WriteLine(link));
    }
}