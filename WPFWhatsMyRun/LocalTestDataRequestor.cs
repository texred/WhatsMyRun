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
            var solutionRoot = Directory.GetParent(Directory.GetParent(Directory.GetParent(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName;
            var testingDataFullPath = Path.Combine(solutionRoot, @"Testing\WhatsMyRun.Services.Tests", "SampleRunDataFromJune.json");

            using (var fs = new FileStream(testingDataFullPath, FileMode.Open))
            using (var sr = new StreamReader(fs))
            {
                //var d = sr.ReadToEnd();
                var t = new Task<string>(() => sr.ReadToEnd());
                return await t;
                //.net bug w/ this? It doesn't ever return the results.
                //return await sr.ReadToEndAsync();
            }
        }

        public Task<T> PostDataAsync<T>(Uri uri, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
