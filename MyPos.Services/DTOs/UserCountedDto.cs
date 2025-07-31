using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.Services.DTOs
{
    public class UserCountedDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public int RegistrantId { get; set; }
        public string RegistrantName { get; set; } = null!;
        public CountDto CountDto { get; set; } = null!;
    }
}
