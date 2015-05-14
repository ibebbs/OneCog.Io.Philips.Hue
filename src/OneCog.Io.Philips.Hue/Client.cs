using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface IClient
    {
        Task<string> Get(Uri resource);
        Task<string> Get(Uri resource, IReadOnlyDictionary<string, string> urlSegments);

        Task<bool> Post(Uri resource, string content);
    }

    public class Client : IClient
    {
        private readonly RestSharp.RestClient _client;

        public Client(Uri baseUri, string userName)
        {
            _client = new RestSharp.RestClient(baseUri);
        }

        public Task<string> Get(Uri resource)
        {
            return Get(resource, new Dictionary<string, string>());
        }

        public async Task<string> Get(Uri resource, IReadOnlyDictionary<string, string> urlSegments)
        {
            var request = new RestRequest(resource, Method.GET);

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
            var request = new RestRequest(resource, Method.PUT);
            request.AddBody(content);

            var response = await _client.ExecutePostTaskAsync(request);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Put(Uri resource, string content)
        {
            var request = new RestRequest(resource, Method.PUT);
            request.AddBody(content);

            var response = await _client.ExecuteTaskAsync(request);

            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw response.ErrorException;
            }
        }
    }
}
