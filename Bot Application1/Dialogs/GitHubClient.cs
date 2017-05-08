using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Octokit;

namespace Bot_Application1.Dialogs
{
    public class GitHubClient
    {
        private readonly Lazy<Octokit.GitHubClient> _client;

        public GitHubClient(string appName = @"MyBotApp")
        {
            _client = new Lazy<Octokit.GitHubClient>(() => new Octokit.GitHubClient(new Octokit.ProductHeaderValue(appName)));
        }

        public Task<Octokit.User> LoadProfile(string userName) => _client.Value.User.Get(userName);

        public Task<Octokit.SearchUsersResult> ExecuteSearch(string query) => 
            _client.Value.Search.SearchUsers(new SearchUsersRequest(query));
    }
}