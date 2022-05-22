using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestGitHub_API_RestSharp
{
	public class GitHub_Tests
	{
		private RestClient client;
		private RestRequest request;
		private string baseUrl = "https://api.github.com";
		private string allIssuesURL = "/repos/{user}/{repo}/issues";
		private string singleIssueURL = "/repos/{user}/{repo}/issues/{issueNumber}";
		private string user = "ENTER_YOUR_USERNAME_HERE";
		private string token = "ENTER_YOUR_GITHUB_TOKEN_HERE";
		

		[SetUp]
		public void Setup()
		{
			this.client = new RestClient(baseUrl);
			
		}

		
		[Test]
		public async Task Test_Get_APIRequest_GitHub()
		{
			this.request = new RestRequest(allIssuesURL);
			//enter your Github username
			this.request.AddUrlSegment("user", "milentasev");
			//enter your Github repo
			this.request.AddUrlSegment("repo", "RestSharp");
			
			var response = await this.client.ExecuteAsync(this.request, Method.Get);

			Assert.IsNotNull(response.Content);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		}

		[Test]
		public async Task Test_GitHub_Get_AllIssues()
		{
			this.request = new RestRequest(allIssuesURL);
			//enter your Github username
			this.request.AddUrlSegment("user", "milentasev");
			//enter your Github repo
			this.request.AddUrlSegment("repo", "RestSharp");
			
			var response = await this.client.ExecuteAsync(request, Method.Get);

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

			var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

			foreach (var issue in issues)
			{
				Assert.Greater(issue.id, 0);
				Assert.Greater(issue.number, 0);
				Assert.IsNotEmpty(issue.title);

			}

		}

		[Test]
		public async Task Test_If_Issues_Are_Multiple()
		{
			this.request = new RestRequest(allIssuesURL);
			//enter your Github username
			this.request.AddUrlSegment("user", "milentasev");
			//enter your Github repo
			this.request.AddUrlSegment("repo", "RestSharp");
			
			var response = await this.client.ExecuteAsync(request, Method.Get);

			var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

			Assert.That(issues.Count > 1);

		}

		[Test]
		public async Task Test_CreateIssue()
		{
			this.request = new RestRequest(allIssuesURL);
			//enter your Github username
			this.request.AddUrlSegment("user", "milentasev");
			//enter your Github repo
			this.request.AddUrlSegment("repo", "RestSharp");

			client.Authenticator = new HttpBasicAuthenticator(user, token);


			this.request.AddJsonBody(new { title = "New issue from RestSharp", body = "I have a problem with this one.", labels = new string[] {"bug", "importance: high" }});
			var response = await this.client.ExecuteAsync(request, Method.Post);

			var issue = JsonSerializer.Deserialize<Issue>(response.Content);

			Assert.Greater(issue.id, 0);
			Assert.Greater(issue.number, 0);
			Assert.IsNotEmpty(issue.title);

		}

		[Test]
		public async Task Test_Get_Issue_By_Valid_Number()
		{
			this.request = new RestRequest(singleIssueURL);
			//enter your Github username
			this.request.AddUrlSegment("user", "milentasev");
			//enter your Github repo
			this.request.AddUrlSegment("repo", "RestSharp");
			//enter issue number 
			request.AddUrlSegment("issueNumber", 1);

			var response = await this.client.ExecuteAsync(this.request, Method.Get);

			var issue = JsonSerializer.Deserialize<Issue>(response.Content);

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.AreEqual(1, issue.number);
			Assert.That(issue.id, Is.GreaterThan(1));
			Assert.IsNotNull(issue.title);

		}

		[Test]
		public async Task Test_Get_Issue_By_Invalid_Number()
		{
			this.request = new RestRequest(singleIssueURL);
			//enter your Github username
			this.request.AddUrlSegment("user", "milentasev");
			//enter your Github repo
			this.request.AddUrlSegment("repo", "RestSharp");
			//enter issue number 
			request.AddUrlSegment("issueNumber", 233344);

			var response = await this.client.ExecuteAsync(this.request, Method.Get);

			var issue = JsonSerializer.Deserialize<Issue>(response.Content);

			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
			
		}
	}
}