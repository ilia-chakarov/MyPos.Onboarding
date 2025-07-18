using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.Services.DTOs.Log
{
    public class HttpLogEntry
    {
        // Request
        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string RequestBody { get; set; }

        // Response
        public int StatusCode { get; set; }
        public string ResponseBody { get; set; }

        // Meta
        public long ElapsedMilliseconds { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
