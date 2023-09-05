using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;

namespace TECSO.FWK.Domain.Services
{
    public class HttpCustomClient
    {

        private string baseUrl;

        private Func<string> getAccess_token;

        protected NetworkCredential credential;

        public HttpCustomClient(string _baseUrl, Func<string> _access_tokenFunc)
        {
            baseUrl = _baseUrl;

            getAccess_token = _access_tokenFunc;
        }


        public HttpCustomClient(string _baseUrl, string user, string password)
        {
            baseUrl = _baseUrl;
            this.credential = new NetworkCredential(user, password);
        }




        /// <summary>
        /// Method for GET request.
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public async Task<RT> GetRequest<RT>(string api, IDictionary<string,string> parameters=null, TimeSpan? timeOut = null)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var access_token = getAccess_token?.Invoke();

                    if (timeOut.HasValue)
                    {
                        client.Timeout = timeOut.Value;
                    }


                    if (!string.IsNullOrEmpty(access_token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
                    }

                    if (parameters==null)
                    {
                        parameters = new Dictionary<string, string>();
                    }

                    var requestUri = QueryHelpers.AddQueryString(api, parameters);

                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync(requestUri);

                    this.ManageErrorsClient(response);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<RT>(responseString);
                        return responseObject;

                    }
                    return default(RT);
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }


        public async Task<RT> GetRequestResponseModel<RT>(string api)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var access_token = getAccess_token?.Invoke();
                    if (!string.IsNullOrEmpty(access_token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
                    }

                    

                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync(api);

                    this.ManageErrorsResponseModelClient<RT>(response);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<ResponseModel<RT>>(responseString);
                        return responseObject.DataObject;

                    }
                    return default(RT);
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }


        private HttpClient BuildHttpClient()
        {
            HttpClient client;

            if (this.credential != null)
            {
                client = new HttpClient(new HttpClientHandler() { Credentials = credential });
            }
            else
            {
                client = new HttpClient();
            }

            client.BaseAddress = new Uri(this.baseUrl);
            return client;
        }

        /// <summary>
        /// Method for the POST request.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostRequest<T>(string api, T data)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var access_token = getAccess_token?.Invoke();

                    if (!string.IsNullOrEmpty(access_token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
                    }

                    var jsonRequest = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                    // HTTP POST
                    var response = await client.PostAsync(api, content);

                    this.ManageErrorsClient(response);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<T>(responseString);
                        return responseObject;

                    }
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }

        public async Task<T> PostRequestResponseModel<T, F>(string api, F data)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var access_token = getAccess_token?.Invoke();

                    if (!string.IsNullOrEmpty(access_token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
                    }

                    var jsonRequest = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                    // HTTP POST
                    var response = await client.PostAsync(api, content);

                    this.ManageErrorsResponseModelClient<T>(response);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<ResponseModel<T>>(responseString);
                        return responseObject.DataObject;

                    }
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }


        /// <summary>
        ///  Method for the POST request.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="jsonInput"></param>
        /// <returns></returns>
        public async Task<RT> PostRequest<RT>(string api, string jsonInput = "{}", TimeSpan? timeOut=null)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {

                    if (timeOut.HasValue)
                    {
                        client.Timeout = timeOut.Value;
                    }
                    
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var access_token = getAccess_token?.Invoke();

                    if (!string.IsNullOrEmpty(access_token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
                    }

                    var content = new StringContent(jsonInput, Encoding.UTF8, "text/json");
                    var response = await client.PostAsync(api, content);

                    this.ManageErrorsClient(response);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        var responseObject = JsonConvert.DeserializeObject<RT>(responseString);
                        return responseObject;
                    }
                    return default(RT);
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }

        public virtual async Task<T> PostRequest<T>(string api, List<KeyValuePair<string, string>> FormData)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new FormUrlEncodedContent(FormData.ToArray());

                    // HTTP POST
                    HttpResponseMessage response = await client.PostAsync(api, content);

                    this.ManageErrorsClient(response);

                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        T responseObject = JsonConvert.DeserializeObject<T>(responseString);
                        return responseObject;
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }




        /// <summary>
        /// Method for the PUT request.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="jsonInput"></param>
        /// <returns></returns>
        public async Task<string> PutRequest(string api, string jsonInput)
        {
            try
            {
                using (var client = this.BuildHttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var access_token = getAccess_token?.Invoke();

                    if (!string.IsNullOrEmpty(access_token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
                    }

                    var content = new StringContent(jsonInput, Encoding.UTF8, "text/json");
                    var res = await client.PutAsync(api, content);

                    this.ManageErrorsClient(res);

                    if (res.IsSuccessStatusCode)
                    {
                        var responseString = res.Content.ReadAsStringAsync().Result;
                        return responseString;

                    }

                    return "";
                }
            }
            catch (Exception ex)
            {
                //await Task.Run(() => App.LogError(ex));
                throw ex;
            }
        }

        /// <summary>
        /// Method for Deserialize the JSON response.
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(string responseString)
        {
            return JsonConvert.DeserializeObject<T>(responseString);
        }


        protected virtual void ManageErrorsResponseModelClient<RT>(HttpResponseMessage response)
        {            
            if (response != null && response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new System.UnauthorizedAccessException(responseString);
                }


                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    if (!responseString.Contains("DataObject"))
                    {
                        throw new ValidationException(string.Join(",", responseString));
                    }
                }

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (!responseString.Contains("DataObject"))
                    {
                        if (!string.IsNullOrEmpty(responseString))
                        {
                            throw new ValidationException(string.Join(",", responseString));
                        }
                        throw new ValidationException(string.Join(",", response.RequestMessage.ToString()));
                    }
                }

                var responseObject = JsonConvert.DeserializeObject<ResponseModel<RT>>(responseString);
                if (responseObject.Status != "Ok")
                {
                    throw new ValidationException(string.Join(",", responseObject.Messages));
                }

                throw new Exception(responseString);
            }
        }



        protected virtual void ManageErrorsClient(HttpResponseMessage response)
        {
            //TODO: manage the response
            if (response != null && response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;

                //if (responseString.Contains(SharedTokens.RegistrarUsuario))
                //{
                //    throw new Exceptions.AuthenticationException(SharedTokens.RegistrarUsuario);
                //}
                //if (responseString.Contains(SharedTokens.RequierdToken))
                //{
                //    throw new Exceptions.RequierdTokenException(SharedTokens.RequierdToken);
                //}
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new System.UnauthorizedAccessException(responseString);
                }

                if (string.IsNullOrEmpty(responseString))
                {
                    throw new Exception(response.ToString());
                }                        

                throw new Exception(responseString);


            }
        }
    }

    public class ResponseModel<T> : AbstractModel
    {
        public ResponseModel()
        {
            this.Messages = new List<string>();
        }

        public T DataObject { get; set; }
    }

    public abstract class AbstractModel
    {
        public String Status { get; set; }

        public List<String> Messages { get; set; }
    }
}
