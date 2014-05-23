using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WhatsMyRun.Services.DataRequestor
{
    public class SampleDataRequestor : IDataRequestor
    {
        public async Task<T> GetDataAsync<T>(Uri uri)
        {
            if (uri.AbsolutePath.Contains("workout"))
            {
                uri = new Uri("https://dl.dropboxusercontent.com/u/27461593/WhatsMyRunSampleData/Workouts.json");
            }

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(uri))
            {
                if (response == null) return default(T);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Deserialize
            }

            return default(T);
        }

        public Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
