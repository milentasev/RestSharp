using RestSharp;
using RestSharp.Authenticators;


var client = new RestClient("https://api.github.com");
client.Authenticator = new HttpBasicAuthenticator("milentasev", "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");


var request = new RestRequest("repos/{user}/{repo}/issues", Method.Post);
request.AddUrlSegment("user", "milentasev");
request.AddUrlSegment("repo", "Postman");



request.AddHeader("Content-Type", "application/json");
request.AddBody(new { title = "New issue from RestSharp", body = "I have a problem with this", labels =new string[] {"bug", "importance: high"}});

var response = await client.ExecuteAsync(request);


