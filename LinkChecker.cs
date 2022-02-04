using HtmlAgilityPack;
public class LinkChecker
{
    public static IEnumerable<String> GetLinks(String page)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(page);
        var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]")
            .Select(n => n.GetAttributeValue("href", string.Empty))
            .Where(l => !string.IsNullOrEmpty(l))
            .Where(l => l.StartsWith("http"));
        return links;
    }

    public static IEnumerable<LinkCheckResult> CheckLinks(IEnumerable<string> links)
    {
        var all = Task.WhenAll(links.Select(CheckLinks));
        return all.Result;
    }

    public static async Task<LinkCheckResult> CheckLinks(string link)
    {
        var result = new LinkCheckResult();
        result.Link = link;
        using (var client = new HttpClient())
        {
            var reqeust = new HttpRequestMessage(HttpMethod.Head, link);
            try
            {
                var response = await client.SendAsync(reqeust); 
                result.Problem = response.IsSuccessStatusCode
                    ? null
                    : response.StatusCode.ToString();
                return result;
            }   
            catch (HttpRequestException exception)
            {
                result.Problem = exception.Message;
                return result;
            }
        }
            return result;

    }

}

public class LinkCheckResult
{
    public bool Exists => String.IsNullOrWhiteSpace(Problem);
    public string Problem { get; set; }
    public string Link { get; set; }
    public bool IsMissing => !Exists;
}