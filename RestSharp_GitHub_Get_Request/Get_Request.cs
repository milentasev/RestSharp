using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;

var client = new RestClient("https://api.github.com");

var request = new RestRequest("/repos/{user}/{repo}/issues", Method.Get);

request.AddUrlSegment("user", "milentasev");
request.AddUrlSegment("repo", "Postman");
request.AddUrlSegment("id", "16");

var response = await client.ExecuteAsync(request);

var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

foreach (var issue in issues)
{
	Console.WriteLine("ISSUE NUMBER: " + issue.number);
	Console.WriteLine("ISSUE ID: " + issue.id);
	Console.WriteLine("HTML_URL: " + issue.html_url);
	Console.WriteLine("*********");
}

Console.WriteLine("ISSUES COUNT: " + issues.Count);

Console.WriteLine("STATUS CODE: " + response.StatusCode);
Console.WriteLine("BODY: " + response.Content);



