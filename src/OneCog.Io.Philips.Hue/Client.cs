using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface IClient
    {
        Task<string> Get(Uri resource);
        Task<string> Get(Uri resource, IReadOnlyDictionary<string, string> urlSegments);

        Task<bool> Post(Uri resource, string content);

        Task<string> Put(Uri resource, string content);
        Task<string> Put(Uri resource, IReadOnlyDictionary<string, string> urlSegments, string content);
    }

    public class Client : IClient
    {
        private readonly RestSharp.RestClient _client;

        public Client(Uri baseUri)
        {
            _client = new RestSharp.RestClient(baseUri);
        }

        private RestRequest BuildRequest(Uri resource, Method method, IReadOnlyDictionary<string, string> uriSegments)
        {
            RestRequest request = new RestRequest(resource, method);

            foreach (KeyValuePair<string, string> kvp in uriSegments)
            {
                request.AddUrlSegment(kvp.Key, kvp.Value);
            }

            return request;
        }

        public Task<string> Get(Uri resource)
        {
            return Get(resource, new Dictionary<string, string>());
        }

        public async Task<string> Get(Uri resource, IReadOnlyDictionary<string, string> urlSegments)
        {
            var request = BuildRequest(resource, Method.GET, urlSegments);

            var response = await _client.ExecuteGetTaskAsync(request);

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public async Task<bool> Post(Uri resource, string content)
        {
            var request = BuildRequest(resource, Method.POST, new Dictionary<string, string>());
            request.AddBody(content);

            var response = await _client.ExecutePostTaskAsync(request);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<string> Put(Uri resource, IReadOnlyDictionary<string, string> urlSegments, string content)
        {
            var request = BuildRequest(resource, Method.PUT, urlSegments);
            request.AddBody(content);

            var response = await _client.ExecuteTaskAsync(request);

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            else
            {
                throw response.ErrorException;
            }
        }

        public Task<string> Put(Uri resource, string content)
        {
            return Put(resource, new Dictionary<string, string>(), content);
        }
    }
}
