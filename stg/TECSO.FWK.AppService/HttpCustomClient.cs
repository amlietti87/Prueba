using Microsoft.AspNetCore.Http;
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

namespace TECSO.FWK.AppService
{
    /// <summary>
    /// This class handles all the Network call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpCustomClient<T> : TECSO.FWK.Domain.Services.HttpCustomClient
    {

        public HttpCustomClient(string _baseUrl, Func<string> _access_tokenFunc)
       : base(_baseUrl, _access_tokenFunc)
        {

        }


        /// <summary>
        /// Method for GET request.
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public async Task<T> GetRequest(string api)
        {
            return await this.GetRequest<T>(api);
        }


        /// <summary>
        /// Method for the POST request.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostRequest(string api, T data)
        {
            return await base.PostRequest<T>(api, data);
        }


        public virtual async Task<T> PostRequest(string api, List<KeyValuePair<string, string>> FormData)
        {
            return await base.PostRequest<T>(api, FormData);
        }

    }
}
