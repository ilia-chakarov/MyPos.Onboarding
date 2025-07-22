using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyPos.TCPWokrers.XmlModel
{
    [XmlRoot]
    public class ChatMessage
    {
        [XmlElement]
        public string Message { get; set; } = null!;
        [XmlElement]
        public DateTime Timestamp { get; set; }
    }
}
