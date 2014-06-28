using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyRun.Services.DataRequestor
{
    public class LocalTestDataRequestor : IDataRequestor
    {
        public async Task<string> GetDataAsync(Uri uri)
        {
            //var slnRoot = Directory.GetParent(Directory.GetParent(
            //    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            string dataLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "..\\Testing\\SampleRunDataFromJune.json";

            using(var sr = new StreamReader( new FileStream(dataLocation, FileMode.Open)))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
