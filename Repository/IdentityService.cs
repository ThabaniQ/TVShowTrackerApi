using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using TVShowTracker.Data;
using TVShowTracker.Domain;
using TVShowTracker.Contracts.Request;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TVShowTracker.Repository.Interface;

namespace TVShowTracker.Repository
{
    public class IdentityService : RepositoryBase<UserRegistrationDTO>, IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public IdentityService(ApplicationDbContext _applicationContext, UserManager<IdentityUser> userManager, IConfiguration configuration) : base(_applicationContext)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthenticationResult> LoginAsync(UserLoginDTO userLogin)
        {

            var _user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (_user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }
            var userValid = await _userManager.CheckPasswordAsync(_user, userLogin.Password);

            if (!userValid)
            {

                return new AuthenticationResult
                {
                    Errors = new[] { "User or Password is incorrect" }
                };
            }

            return GenerateAuthentication(_user);
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationDTO userRegistation)
        {
            var existingUser = await _userManager.FindByEmailAsync(userRegistation.Email);
            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email addresss already exist" }
                };
            }

            var user = new IdentityUser
            {
                Email = userRegistation.Email,
                UserName = userRegistation.Username
            };

            var createdUser = await _userManager.CreateAsync(user, userRegistation.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };

            }
            return GenerateAuthentication(user);
        }

        private AuthenticationResult GenerateAuthentication(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)

                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
