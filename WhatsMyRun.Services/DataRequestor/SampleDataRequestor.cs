using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WhatsMyRun.Services.DataRequestor
{
    public class SampleDataRequestor : IDataRequestor
    {
        public async Task<string> GetDataAsync(Uri uri)
        {
            if (uri.AbsolutePath.Contains("workout"))
            {
                uri = new Uri("https://dl.dropboxusercontent.com/u/27461593/WhatsMyRunSampleData/Workouts.json");
            }

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(uri))
            {
                if (response == null) return null;
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        public Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
