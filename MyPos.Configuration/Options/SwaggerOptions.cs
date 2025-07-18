using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.Configuration.Options
{
    public class SwaggerOptions
    {
        public string Title { get; set; } = "MyPos API";
        public string Version { get; set; } = "v1";
        public string JwtDescription { get; set; } = "Enter your JWT token below (without 'Bearer' prefix)";
        public string BasicDescription { get; set; } = "Basic Authentication using username and password";
    }
}
