using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PolyClinic.Authentication.Repository
{
    /// <summary>
    /// Repository for Jwt Token creation
    /// </summary>
    public class JwtRepository
    {
        private const int EXPIRATION_MINUTES = 2;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Creates Jwt Repository instance with configuraion builder to get Jwt options from Appsettings.json file
        /// </summary>
        public JwtRepository()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        /// <summary>
        /// Creates Jwt Repository instance with injected configuraion instance to get Jwt options from Appsettings.json file
        /// </summary>
        public JwtRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates Authentication Token with given User Name and Email
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="userEmail">User Email Address</param>
        /// <param name="expiration">Expiration date/time</param>
        /// <returns>Authentication Token string</returns>
        public string CreateToken(string userName, string userEmail, out DateTime expiration)
        {
            expiration = DateTime.Now.AddMinutes(EXPIRATION_MINUTES);

            var token = CreateJwtToken(
                CreateClaims(userName, userEmail),
                CreateSigningCredentials(),
                expiration
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Initializes a JwtSecurityToken instance
        /// </summary>
        /// <param name="claims">Array of Claim instances</param>
        /// <param name="credentials">SigningCredentials instance</param>
        /// <param name="expiration">Expiration date/time</param>
        /// <returns>JwtSecurityToken</returns>
        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration) =>
            new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        /// <summary>
        /// Creates Claim instances for generating JwtSecurityToken
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="userEmail">User Email Address</param>
        /// <returns>Array of Claim instances</returns>
        private Claim[] CreateClaims(string userName, string userEmail) =>
            new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userName),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userEmail)
            };

        /// <summary>
        /// Initializes a SigningCredentials instance with encoded security key
        /// </summary>
        /// <returns>SigningCredentials with encoded security key</returns>
        private SigningCredentials CreateSigningCredentials() =>
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),
                SecurityAlgorithms.HmacSha256
            );
    }
}
