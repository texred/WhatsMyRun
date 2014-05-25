using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhatsMyRun.Services.DataRequestor
{
    public interface IDataRequestor
    {
        Task<T> GetDataAsync<T>(Uri uri);

        Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters);
    }
}
