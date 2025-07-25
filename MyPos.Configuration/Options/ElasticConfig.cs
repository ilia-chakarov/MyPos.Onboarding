using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.Configuration.Options
{
    public class ElasticConfig
    {
        public string Url { get; set; }
        public string IndexFormat { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
