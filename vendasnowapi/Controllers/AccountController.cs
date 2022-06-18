using System;
using System.Threading.Tasks;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using System.Linq;
using System.Security.Claims;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private IConfiguration configuration;
        private IWebHostEnvironment _hostEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration Configuration, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = Configuration;
            _hostEnvironment = environment;
        }

        [HttpPost()]
        [Route("recoverPasswordVendasNow")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPasswordVendasNow(LoginUser loginUser)
        {
            try
            {
                if ((loginUser == null) || (loginUser.Email == null))
                {
                    return BadRequest("E-mail inválido.");
                }
                var user = await userManager.FindByEmailAsync(loginUser.Email);
                if (user == null)
                {
                    return BadRequest("E-mail não encontrado.");
                }

                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var valid = await passwordValidator.ValidateAsync(userManager, user, loginUser.Secret);

                if (valid.Succeeded)
                {
                    user.PasswordHash = userManager.PasswordHasher.HashPassword(user, loginUser.Secret);
                }
                else
                {
                    if (valid.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                    return BadRequest("Não foi possível recuperar a senha.");
                }
                user.EmailConfirmed = false;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var code = Guid.NewGuid().ToString();
                    await userManager.AddClaimAsync(user, new Claim("CodeConfirmation", code));
                    sendEmailConfirmUser(loginUser.Email, code, user.Id);
                } else
                {
                    if (result.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                    if (result.Errors.FirstOrDefault().Code.Equals("InvalidUserName") || result.Errors.FirstOrDefault().Code.Equals("InvalidEmail")) { return BadRequest("E-mail inválido!"); }
                    return BadRequest("Não foi possível recuperar a senha.");
                }

                return Ok("Senha recuperada com sucesso! Verifique sua caixa de email e confirme o cadastro.");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }

        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("registerVendasNow")]
        public async Task<IActionResult> RegisterVendasNow(LoginUser loginUser)
        {
            try
            {
                var applicationUser = this.userManager.FindByEmailAsync(loginUser.Email);
                if (applicationUser.Result != null)
                {
                    if (applicationUser.Result.EmailConfirmed)
                    {
                        return BadRequest("Usuário já registrado e confirmado!");
                    }
                    else
                    {
                        var code = Guid.NewGuid().ToString();
                        await userManager.AddClaimAsync(applicationUser.Result, new Claim("CodeConfirmation", code));
                        sendEmailConfirmUser(applicationUser.Result.Email, code, applicationUser.Result.Id);
                        return BadRequest("Usuário já registrado. Verifique sua caixa de email e confirme o cadastro.");
                    }

                }

                var user = new ApplicationUser()
                {
                    UserName = loginUser.Email,
                    Email = loginUser.Email,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = Convert.ToInt32(decimal.Zero)
                };

                IdentityResult addUserResult = await userManager.CreateAsync(user, loginUser.Secret);

                if (addUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "VendasNow");
                    var code = Guid.NewGuid().ToString();
                    await userManager.AddClaimAsync(user, new Claim("CodeConfirmation", code));
                    sendEmailConfirmUser(loginUser.Email, code, user.Id);
                }
                else
                {
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("InvalidUserName") || addUserResult.Errors.FirstOrDefault().Code.Equals("InvalidEmail")) { return BadRequest("E-mail inválido!"); }
                    return BadRequest(addUserResult.Errors.FirstOrDefault().ToString());
                }

                return Ok("Usuário registrado com sucesso! Verifique sua caixa de email e confirme o cadastro.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }

        }

        public void sendEmailConfirmUser(string Email, string code, string userId)
        {
            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(configuration["FromEmail"].ToString());
                mail.To.Add(Email);
                mail.Subject = "O aplicativo VendasNow Pro precisa validar seu email.";
                mail.Body = "<div style='padding-top: 15px;padding-bottom: 15px;'><img src='" + string.Concat(configuration["Dominio"].ToString(), "/assets/logo_arredondado_app.png") + "' width='100'></div>" +
                    "<div style='padding-top: 15px;'>Clique no link abaixo para validar seu email no aplicativo.</div>" +
                    "<div><a href=" + configuration["Dominio"].ToString() + "/user/" + userId + "/" + code + ">Clique para validar</a>" +
                "<div></div>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(configuration["STMPEmail"].ToString(), Convert.ToInt32(configuration["PortEmail"].ToString()));
                smtp.Credentials = new System.Net.NetworkCredential(configuration["UserEmail"].ToString(), configuration["PassEmail"].ToString());
                smtp.Send(mail);

            }
            catch (SmtpFailedRecipientException ex)
            {
                var message = ex.Message;


            }
            catch (SmtpException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("loginVendasNow")]
        public async Task<IActionResult> LoginVendasNow(LoginUser loginUser)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Secret, false, false);
                if (!result.Succeeded)
                {
                    return BadRequest("Acesso negado! Login inválido!");
                }
                var user = await userManager.FindByEmailAsync(loginUser.Email);
                if (!user.EmailConfirmed)
                {
                    return BadRequest("Acesso negado! Confirme sua conta pelo email!");
                }

                var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                var claims = claimsPrincipal.Claims.ToList();
                var permission = claims.Where(c => c.Type.Contains("role")).Select(c => c.Value).FirstOrDefault();
                if (!permission.Equals("VendasNow"))
                {
                    return BadRequest("Acesso negado! Usuário não é usuário do VendasNow Pro!");
                }
                var applicationUser = new ApplicationUser();
                applicationUser.Id = user.Id;
                var applicationUserDTO = new ApplicationUserDTO();
                applicationUserDTO.Token = TokenService.GenerateToken(applicationUser, configuration, permission);
                applicationUserDTO.Email = user.Email;
                applicationUserDTO.UserName = user.UserName;
                applicationUserDTO.Role = permission;
                return new JsonResult(applicationUserDTO);
            }
            catch (Exception ex)
            {
                return BadRequest("Falha no login! " + ex.Message);
            }

        }

        [HttpPost()]
        [Route("changePasswordVendasNow")]
        [Authorize()]
        public async Task<IActionResult> ChangePasswordVendasNow(LoginUser loginUser)
        {
            ClaimsPrincipal currentUser = this.User;
            var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
            if (id == null)
            {
                return BadRequest("Identificação do usuário não encontrada.");
            }
            var user = await userManager.FindByIdAsync(id);
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, loginUser.Secret);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Não foi possível alterar a senha.");
            }
            return Ok();
        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("disableVendasNow")]
        public async Task<IActionResult> DisableVendasNow(LoginUser loginUser)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginUser.Email);
                if (user == null)
                {
                    return BadRequest("Usuário não encontrado.");
                }
                user.LockoutEnd = DateTime.Now.AddYears(50);
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Não foi possível bloquear o usuário. Entre em contato com o administrador do sistema.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Falha no login! " + ex.Message);
            }

        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("enableVendasNow")]
        public async Task<IActionResult> EnableVendasNow(LoginUser loginUser)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginUser.Email);
                if (user == null)
                {
                    return BadRequest("Usuário não encontrado.");
                }
                user.LockoutEnd = DateTime.Now;
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Não foi possível desbloquear o usuário. Entre em contato com o administrador do sistema.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Falha no login! " + ex.Message);
            }

        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("confirmVendasNow")]
        public async Task<IActionResult> ConfirmVendasNow(LoginUser loginUser)
        {
            try
            {
                var applicationUser = await this.userManager.FindByIdAsync(loginUser.ApplicationUserId);
                if (applicationUser == null)
                {
                    return BadRequest("Usuário não encontrado.");
                }

                var claims = await this.userManager.GetClaimsAsync(applicationUser);

                if (!claims.Any(c => c.Value == loginUser.Code))
                {
                    return BadRequest("Código não encontrado.");
                }

                if ((claims.Any(c => c.Value == loginUser.Code)) && (applicationUser.EmailConfirmed))
                {
                    return BadRequest("Email já confirmado. Efetue o login");
                }

                applicationUser.EmailConfirmed = true;
                await this.userManager.UpdateAsync(applicationUser);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na validação do usuário", ex.Message));
            }

        }

    }
}
