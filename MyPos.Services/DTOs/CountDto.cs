using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.Services.DTOs
{
    public class CountDto
    {
        public int? PageNumber { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public int? TotalCount { get; set; } = null;
    }
}
