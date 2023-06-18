using Microsoft.AspNetCore.Identity;
using PolyClinic.Authentication.Models;

namespace PolyClinic.Authentication.Repository
{
    public class AuthenticationRepository
    {
        private readonly AuthenticationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationRepository(AuthenticationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Creates new User as an asynchronorous operation
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="email">User Email</param>
        /// <param name="password">Password</param>
        /// <param name="firstName">Password</param>
        /// <param name="lastName">Password</param>
        /// <returns>Empty string if user created successfully. Otherwise, error message</returns>
        public async Task<string> CreateUserAsync(string userName, string email, string password, string firstName, string lastName)
        {
            var result = await _userManager.CreateAsync(
                new ApplicationUser()
                {
                    UserName = userName,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                },
                password: password);

            if (!result.Succeeded)
            {
                var errors =  result.Errors.ToList().ConvertAll(err => err.Description);
                return string.Join(",\n", errors);
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks if User exists with given User Name and Password, as an asynchronorous operation
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>User Email if User exists for given username and password. Otherwise, null</returns>
        public async Task<string> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;

            var isValid = await _userManager.CheckPasswordAsync(user, password);

            if(isValid)
            {
                return user.Email;
            }

            return null;
        }

    }
}