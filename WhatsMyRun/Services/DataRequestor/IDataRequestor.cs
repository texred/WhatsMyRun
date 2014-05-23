using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WhatsMyRun.Services.DataRequestor
{
    public interface IDataRequestor
    {
        Task<JObject> GetDataAsync(Uri uri);

        Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters);
    }
}
