using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
    public static class JTokenExtensions
    {
        /// <summary>
        /// Provides a fallback value if the node was not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="valueToUseIfNotFound"></param>
        /// <returns></returns>
        public static T ValueWithDefault<T>(this JToken token, T valueToUseIfNotFound = default(T))
        {
            if (token != null)
            {
                return token.Value<T>();
            }
            else
            {
                return valueToUseIfNotFound;
            }
        }
    }
}
