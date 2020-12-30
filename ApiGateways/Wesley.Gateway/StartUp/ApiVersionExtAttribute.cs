using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeslesyCore.Web.Startup.Swagger
{
    /// <summary>
    /// 自定义Api版本特性，目前只用Swagger版本区分，若后期要把API访问实现多版本，则要引入Microsoft.AspNetCore.Mvc.Versioning
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiVersionExtAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public string Version { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="version"></param>
        public ApiVersionExtAttribute(string version)
        {
            Version = version;
        }
    }
}