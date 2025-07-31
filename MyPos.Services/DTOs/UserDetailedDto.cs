using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DTOs;

namespace MyPos.Services.DTOs
{
    public class UserDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public RegistrantDto? Registrant { get; set; }
    }
}
