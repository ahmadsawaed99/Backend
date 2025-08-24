using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DTOs;

namespace Backend.Repositories
{
    public interface IAccountRepository
    {
        Task<ServiceResponses.LoginResponse> CreateAccount(UserDTO userDTO);
        Task<ServiceResponses.LoginResponse> LoginAccount(LoginDTO loginDTO);
        Task SendResetLinkToEmail(string email);
        Task ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
                if (user is not null)
                    await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task SendResetLinkToEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var resetLink = string.Empty;
                if (user is not null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    resetLink = $"www.myapp.com/token={resetToken}";
                    // send mail (resetLink)
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceResponses.LoginResponse> CreateAccount(UserDTO userDTO)
        {
            // CreateUserPassword
            if (userDTO.CreateUserPassword != _config["CreateUserPassword"] && userDTO.CreateUserPassword != _config["CreateAdminPassword"])
                return new ServiceResponses.LoginResponse(false, null!, null!, "The password of create user is wrong", null!);

            if (userDTO == null)
                return new ServiceResponses.LoginResponse(false, null!, null!, "Model is empty", null!);


            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user is not null)
                return new ServiceResponses.LoginResponse(false, null!, null!, "Model registered already", null!);

            var newUser = new AppUser()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = userDTO.Password,
                UserName = userDTO.Email,
            };

            var createUser = await _userManager.CreateAsync(newUser!, userDTO.Password);

            if (!createUser.Succeeded) return new ServiceResponses.LoginResponse(false, null!, null!, "Error occured... please try again", null!);

            var checkAdmin = userDTO.CreateUserPassword == _config["CreateAdminPassword"];

            if (checkAdmin)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await _userManager.AddToRoleAsync(newUser, "Admin");

                var getUserRole = await _userManager.GetRolesAsync(newUser);
                var userSession = new UserSession(newUser.Id, newUser.Name, newUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);

                return new ServiceResponses.LoginResponse(true, token, getUserRole.First(), "Account Created", newUser.Id);
            }
            else
            {
                var checkUser = await _roleManager.FindByNameAsync("User");
                if (checkUser is null) await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                await _userManager.AddToRoleAsync(newUser, "User");

                var getUserRole = await _userManager.GetRolesAsync(newUser);
                var userSession = new UserSession(newUser.Id, newUser.Name, newUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);

                return new ServiceResponses.LoginResponse(true, token, getUserRole.First(), "Account Created", newUser.Id);
            }
        }

        public async Task<ServiceResponses.LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null) return new ServiceResponses.LoginResponse(false, null!, null!, "Login Container is empty", null!);

            var getUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null) return new ServiceResponses.LoginResponse(false, null!, null!, "User not found", null!);

            bool checkUserPassword = await _userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPassword) return new ServiceResponses.LoginResponse(false, null!, null!, "Invalid email or password", null!);

            var getUserRole = await _userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());

            string token = GenerateToken(userSession);

            return new ServiceResponses.LoginResponse(true, token, userSession.Role!, "Login Completed", getUser.Id);
        }

        private string GenerateToken(UserSession userSession)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , userSession.id!),
                new Claim(ClaimTypes.Name , userSession.Name!),
                new Claim(ClaimTypes.Email , userSession.Email!),
                new Claim(ClaimTypes.Role , userSession.Role!),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
