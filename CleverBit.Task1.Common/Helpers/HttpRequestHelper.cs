using CleverBit.Task1.Common.Enums;
using CleverBit.Task1.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CleverBit.Task1.Common.Helpers
{
    public interface IHttpRequestHelper
    {
        IHttpRequestHelper Configure(IntegrationModel integrationModel);

        Task<Result<T>> Post<K, T>(string endpoint, ContentTypes contentType, K content)
            where K : class, new()
            where T : class;

        Task<Result<T>> Get<T>(string endpoint, string queryString = null)
            where T : class;
    }

    public class HttpRequestHelper : IHttpRequestHelper
    {
        private string BaseUrl { get; set; }
        private Dictionary<string, string> Headers { get; set; }

        public IHttpRequestHelper Configure(IntegrationModel integrationModel)
        {
            this.Headers = new Dictionary<string, string>();
            this.BaseUrl = integrationModel.Url;

            // Refresh Token

            switch (integrationModel.AuthenticationType)
            {
                case AuthenticationTypes.Bearer:
                    Headers.Add("Authentication", $"Bearer {integrationModel.Token}");
                    break;
                case AuthenticationTypes.Basic:
                    var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(integrationModel.Username + ":" + integrationModel.Password));
                    Headers.Add("Authorization", $"Basic {auth}");
                    break;
            }

            return this;
        }

        public async Task<Result<T>> Post<K, T>(string endpoint, ContentTypes contentType, K content)
            where K : class, new()
            where T : class
        {
            try
            {
                var requestUrl = this.BaseUrl + endpoint;
                var requestContentParameters = await SetRequestContent<K>(contentType, content);

                var handler = new HttpClientHandler { DefaultProxyCredentials = CredentialCache.DefaultCredentials };
                using var client = new HttpClient(handler);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(requestUrl),
                    Content = new StringContent(requestContentParameters.Value, Encoding.UTF8, requestContentParameters.Key),
                    Method = HttpMethod.Post
                };

                if (Headers != null && Headers.Any())
                    foreach (var item in Headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = await SetResponseContent<T>(response.Content.Headers.ContentType.MediaType, responseContent);

                return new Result<T>("Success", true, responseData);
            }
            catch (Exception ex)
            {
                return new Result<T>(ex, default(T));
            }
        }

        public async Task<Result<T>> Get<T>(string endpoint, string queryString = null)
            where T : class
        {
            try
            {
                var requestUrl = this.BaseUrl + endpoint;
                var handler = new HttpClientHandler { DefaultProxyCredentials = CredentialCache.DefaultCredentials };
                using var client = new HttpClient(handler);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{requestUrl}/{queryString}"),
                    Method = HttpMethod.Get
                };

                if (Headers != null && Headers.Any())
                {
                    foreach (var item in Headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                T result = default;

                try
                {
                    result = JsonConvert.DeserializeObject<T>(responseContent);
                }
                catch (Exception ex)
                {
                    return new Result<T>(ex, null);
                }

                return new Result<T>("Success", true, result);
            }
            catch (Exception ex)
            {
                return new Result<T>(ex, null);
            }
        }

        private Task<T> SetResponseContent<T>(string contentType, string content)
            where T : class
        {
            T result = default;

            try
            {
                switch (contentType)
                {
                    case "application/json":
                        result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        break;
                    case "application/xml":
                        var xmlSerializer = new XmlSerializer(typeof(T));
                        using (TextReader reader = new StringReader(content))
                        {
                            result = (T)xmlSerializer.Deserialize(reader);
                        }
                        break;
                    case "application/x-www-form-urlencoded":
                        result = content as T;
                        break;
                    case "text/html":
                        result = content as T;
                        break;
                    case "text/plain":
                        result = content as T;
                        break;
                }

                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(default(T));
            }
        }

        private Task<KeyValuePair<string, string>> SetRequestContent<K>(ContentTypes contentType, K content)
            where K : class
        {
            var requestContent = string.Empty;
            var mediaType = string.Empty;
            switch (contentType)
            {
                case ContentTypes.JSON:
                    mediaType = "application/json";
                    requestContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    break;
                case ContentTypes.XML:
                    mediaType = "application/xml";
                    var xmlSerializer = new XmlSerializer(typeof(K));
                    using (var stringWriter = new StringWriter())
                    {
                        using XmlWriter writer = XmlWriter.Create(stringWriter);
                        xmlSerializer.Serialize(writer, content);
                        requestContent = stringWriter.ToString();
                    }
                    break;
                case ContentTypes.FORM:
                    mediaType = "application/x-www-form-urlencoded";
                    var formObj = content as IEnumerable<KeyValuePair<string, string>>;
                    requestContent = new FormUrlEncodedContent(formObj).ReadAsStringAsync().Result;
                    break;
                case ContentTypes.HTML:
                    mediaType = "text/html";
                    requestContent = content.ToString();
                    break;
                case ContentTypes.PlainText:
                    mediaType = "text/plain";
                    requestContent = content.ToString();
                    break;
            }

            return Task.FromResult(new KeyValuePair<string, string>(mediaType, requestContent));
        }
    }
}
