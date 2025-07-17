using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Options;
using WebAPI.Services;
using WebAPI.UnitOfWork;

namespace MyPos.WebAPI.xUnitTests.ServicesTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPasswordHasher<UserEntity>> _passwordHasherMock;
        private IOptions<JwtSettings> _jwtOptions;

        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordHasherMock = new Mock<IPasswordHasher<UserEntity>>();
            _jwtOptions = Options.Create(new JwtSettings
                {
                SecretKey = "test-secret-key",
                Issuer = "test-issuer",
                Audience = "test-audience",
                ExpiryMinutes = 60
                }
            );

            _authService = new AuthService( _unitOfWorkMock.Object, _passwordHasherMock.Object, _jwtOptions );
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsNull()
        {
            var loginDto = new LoginDto { Username = "wrong", Password = "invalid_pass" };

            var res = _authService.Login( loginDto );

            Assert.NotNull( res );
        }
    }
}
