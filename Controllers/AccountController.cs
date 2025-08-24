using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Repositories;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController(IAccountRepository accountRepository) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await accountRepository.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await accountRepository.LoginAccount(loginDTO);
            return Ok(response);
        }
        [HttpPost("sendResetLink")]
        [AllowAnonymous]
        public async Task<IActionResult> SendResetLinkToEmail(string email)
        {
            try
            {
                await accountRepository.SendResetLinkToEmail(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("resetUserPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetUserPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                await accountRepository.ResetPassword(resetPasswordDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
