using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork uow, IPasswordHasher<UserEntity> passwordHasher, IConfiguration configuration)
        {
            _unitOfWork = uow;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }
        public async Task<string> Login([FromBody] LoginDto dto)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(query => 
            query.Where(u => u.Username == dto.Username));

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
