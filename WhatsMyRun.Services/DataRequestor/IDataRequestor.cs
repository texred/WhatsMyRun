using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WhatsMyRun.Services.DataRequestor
{
    public interface IDataRequestor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>JSON string</returns>
        Task<string> GetDataAsync(Uri uri);

        Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters);
    }
}
