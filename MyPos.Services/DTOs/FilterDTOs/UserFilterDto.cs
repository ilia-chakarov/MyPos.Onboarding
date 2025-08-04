using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.Services.DTOs.FilterDTOs
{
    public class UserFilterDto
    {
        public string? Username { get; set; }
        public string? RegistrantName { get; set; }
        public int? UserId { get; set; }
    }
}
