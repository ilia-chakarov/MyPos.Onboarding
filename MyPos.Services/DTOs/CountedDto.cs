using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DTOs;

namespace MyPos.Services.DTOs
{
    public class CountedDto<T>
    {
        public int? PageNumber { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public int? TotalCount { get; set; } = null;
        public IEnumerable<T> Items { get; set; } = [];
    }
}
