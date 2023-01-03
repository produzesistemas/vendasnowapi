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
using UnitOfWork;
using System.Linq.Expressions;
using LinqKit;
using System.ComponentModel.Design;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

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
        private ISubscriptionRepository _subscriptionRepository;
        private IEstablishmentRepository _establishmentRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment,
            ISubscriptionRepository subscriptionRepository,
            IEstablishmentRepository establishmentRepository,
            IConfiguration Configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _hostEnvironment = environment;
            this.configuration = Configuration;
            this._subscriptionRepository = subscriptionRepository;
            this._establishmentRepository= establishmentRepository;
        }

        [HttpPost()]
        [Route("recoverPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword(LoginUser loginUser)
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

                if (user.EmailConfirmed)
                {
                    user.EmailConfirmed = false;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var code = Guid.NewGuid().ToString();
                        await userManager.AddClaimAsync(user, new Claim("CodeConfirmation", code));
                        sendEmailConfirmUserForgot(loginUser.Email, code, user.Id, loginUser.AppName);
                    }
                    else
                    {
                        if (result.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                        if (result.Errors.FirstOrDefault().Code.Equals("InvalidEmail")) { return BadRequest("E-mail inválido!"); }
                        if (result.Errors.FirstOrDefault().Code.Equals("InvalidUserName")) { return BadRequest("Nome do usuário inválido. Use apenas letras e números."); }
                        return BadRequest(result.Errors.FirstOrDefault().ToString());
                    }
                } else
                {
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var code = Guid.NewGuid().ToString();
                        await userManager.AddClaimAsync(user, new Claim("CodeConfirmation", code));
                        sendEmailConfirmUser(loginUser.Email, code, user.Id, loginUser.AppName);
                    }
                    else
                    {
                        if (result.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                        if (result.Errors.FirstOrDefault().Code.Equals("InvalidEmail")) { return BadRequest("E-mail inválido!"); }
                        if (result.Errors.FirstOrDefault().Code.Equals("InvalidUserName")) { return BadRequest("Nome do usuário inválido. Use apenas letras e números."); }
                        return BadRequest(result.Errors.FirstOrDefault().ToString());
                    }
                }
                

                return new JsonResult("Senha recuperada com sucesso! Verifique sua caixa de email e confirme o cadastro.");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }

        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(LoginUser loginUser)
        {
            try
            {
                var applicationUser = this.userManager.FindByEmailAsync(loginUser.Email);
                if (applicationUser.Result != null)
                {
                        return BadRequest("Usuário já registrado. Verifique sua caixa de email e confirme o cadastro ou recupere sua senha.");
                }

                var user = new ApplicationUser()
                {
                    UserName = loginUser.Email.Split("@").FirstOrDefault(),
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
                    await userManager.AddToRoleAsync(user, loginUser.AppName);
                    var code = Guid.NewGuid().ToString();
                    await userManager.AddClaimAsync(user, new Claim("CodeConfirmation", code));
                    sendEmailConfirmUser(loginUser.Email, code, user.Id, loginUser.AppName);
                }
                else
                {
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("InvalidEmail")) { return BadRequest("E-mail inválido!"); }
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("InvalidUserName")) { return BadRequest("Nome do usuário inválido. Use apenas letras e números."); }
                    return BadRequest(addUserResult.Errors.FirstOrDefault().ToString());
                }

                return new JsonResult("Usuário registrado com sucesso! Verifique sua caixa de email e confirme o cadastro.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }

        }

        public void sendEmailConfirmUser(string Email, string code, string userId, string appName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(configuration["FromEmail"].ToString());
                mail.To.Add(Email);
                mail.Subject = string.Concat("O aplicativo ", appName, " precisa validar seu email.");
                switch(appName)
                {
                    case "VendasNow":
                        mail.Body = "<div style='padding-top: 15px;padding-bottom: 15px;'><img src='" + string.Concat(configuration["Dominio"].ToString(), "/assets/logo_arredondado_app.png") + "' width='100'></div>" +
                        "<div style='padding-top: 15px;'>Clique no link abaixo para validar seu email no aplicativo.</div>" +
                        "<div><a href=" + configuration["Dominio"].ToString() + "/user/confirm/" + userId + "/" + code + ">Clique para validar</a>" +
                        "<div></div>";
                        break;
                    case "AppBeauty":
                        mail.Body = "<div style='padding-top: 15px;padding-bottom: 15px;'><img src='" + string.Concat(configuration["Dominio"].ToString(), "/assets/logo_arredondado_app.png") + "' width='100'></div>" +
                        "<div style='padding-top: 15px;'>Clique no link abaixo para validar seu email no aplicativo.</div>" +
                        "<div><a href=" + configuration["Dominio"].ToString() + "/user/confirm/" + userId + "/" + code + ">Clique para validar</a>" +
                        "<div></div>";
                        break;
                    default:
                        Console.WriteLine("Default case");
                    break;
                }
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

        public void sendEmailConfirmUserForgot(string Email, string code, string userId, string appName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(configuration["FromEmail"].ToString());
                mail.To.Add(Email);
                mail.Subject = string.Concat("O aplicativo ", appName, " precisa validar seu email.");
                switch (appName)
                {
                    case "VendasNow":
                        mail.Body = "<div style='padding-top: 15px;padding-bottom: 15px;'><img src='" + string.Concat(configuration["Dominio"].ToString(), "/assets/logo_arredondado_app.png") + "' width='100'></div>" +
                        "<div style='padding-top: 15px;'>Clique no link abaixo para validar seu email no aplicativo.</div>" +
                        "<div><a href=" + configuration["Dominio"].ToString() + "/user/confirmForgot/" + userId + "/" + code + ">Clique para validar</a>" +
                        "<div></div>";
                        break;
                    case "AppBeauty":
                        mail.Body = "<div style='padding-top: 15px;padding-bottom: 15px;'><img src='" + string.Concat(configuration["Dominio"].ToString(), "/assets/logo_arredondado_app.png") + "' width='100'></div>" +
                        "<div style='padding-top: 15px;'>Clique no link abaixo para validar seu email no aplicativo.</div>" +
                        "<div><a href=" + configuration["Dominio"].ToString() + "/user/confirmForgot/" + userId + "/" + code + ">Clique para validar</a>" +
                        "<div></div>";
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
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
                var result = await signInManager.PasswordSignInAsync(loginUser.Email.Split("@").FirstOrDefault(), loginUser.Secret, false, false);
                if (!result.Succeeded)
                {
                    return BadRequest("Acesso negado! Login inválido ou conta não confirmada!");
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
                    return BadRequest("Acesso negado! Usuário não tem conta no VendasNow Pro!");
                }

                Expression<Func<Subscription, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Subscription>();
                ps1 = p => p.AspNetUsersId.Equals(user.Id);
                pred = pred.And(ps1);
                ps2 = p => p.Active == true;
                pred = pred.And(ps2);
                var subscription = _subscriptionRepository.GetCurrent(pred);
                if (subscription == null)
                {
                    return BadRequest("Acesso negado! Usuário sem inscrição!");
                }
                
                var applicationUser = new ApplicationUser();
                applicationUser.Id = user.Id;
                var applicationUserDTO = new ApplicationUserDTO();
                applicationUserDTO.Token = TokenService.GenerateToken(applicationUser, configuration, permission);
                applicationUserDTO.Email = user.Email;
                applicationUserDTO.UserName = user.UserName;
                applicationUserDTO.Role = permission;
                applicationUserDTO.SubscriptionDate = subscription.SubscriptionDate.AddDays(subscription.Plan.Days).Date;
                return new JsonResult(applicationUserDTO);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Sequence contains no elements") {
                    return BadRequest("Acesso negado! Usuário sem inscrição!");
                };
                return BadRequest("Falha no login! " + ex.Message);
            }

        }

        [HttpPost()]
        [Route("changePassword")]
        [Authorize()]
        public async Task<IActionResult> ChangePassword(LoginUser loginUser)
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
            return new JsonResult("Senha alterada com sucesso!");
        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("confirm")]
        public async Task<IActionResult> Confirm(LoginUser loginUser)
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

                _subscriptionRepository.Insert(new Subscription()
                {
                    Active = true,
                    AspNetUsersId = applicationUser.Id,
                    PlanId = 1,
                    SubscriptionDate = DateTime.Now,
                    Value = 0
                });
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na validação do usuário", ex.Message));
            }

        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("confirmForgot")]
        public async Task<IActionResult> ConfirmForgot(LoginUser loginUser)
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


        [HttpPost()]
        [AllowAnonymous]
        [Route("loginBeauty")]
        public async Task<IActionResult> LoginBeauty(LoginUser loginUser)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(loginUser.Email.Split("@").FirstOrDefault(), loginUser.Secret, false, false);
                if (!result.Succeeded)
                {
                    return BadRequest("Acesso negado! Login inválido ou conta não confirmada!");
                }
                var user = await userManager.FindByEmailAsync(loginUser.Email);
                if (!user.EmailConfirmed)
                {
                    return BadRequest("Acesso negado! Confirme sua conta pelo email!");
                }

                var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                var claims = claimsPrincipal.Claims.ToList();
                var permission = claims.Where(c => c.Type.Contains("role")).Select(c => c.Value).FirstOrDefault();
                if (!permission.Equals("AppBeauty"))
                {
                    return BadRequest("Acesso negado! Usuário não tem conta no App de Salão de Beleza!");
                }

                Expression<Func<Subscription, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Subscription>();
                ps1 = p => p.AspNetUsersId.Equals(user.Id);
                pred = pred.And(ps1);
                ps2 = p => p.Active == true;
                pred = pred.And(ps2);
                var subscription = _subscriptionRepository.GetCurrent(pred);
                if (subscription == null)
                {
                    return BadRequest("Acesso negado! Usuário sem inscrição!");
                }

                var applicationUser = new ApplicationUser();
                applicationUser.Id = user.Id;
                var applicationUserDTO = new ApplicationUserDTO();
                applicationUserDTO.Token = TokenService.GenerateToken(applicationUser, configuration, permission);
                applicationUserDTO.Email = user.Email;
                applicationUserDTO.UserName = user.UserName;
                applicationUserDTO.Role = permission;
                applicationUserDTO.SubscriptionDate = subscription.SubscriptionDate.AddDays(subscription.Plan.Days).Date;
                return new JsonResult(applicationUserDTO);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Sequence contains no elements")
                {
                    return BadRequest("Acesso negado! Usuário sem inscrição!");
                };
                return BadRequest("Falha no login! " + ex.Message);
            }

        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("registerBeauty")]
        public async Task<IActionResult> RegisterBeauty(RegisterBeauty registerBeauty)
        {
            try
            {
                var applicationUser = this.userManager.FindByEmailAsync(registerBeauty.Email);
                if (applicationUser.Result != null)
                {
                    return BadRequest("Usuário já registrado. Verifique sua caixa de email e confirme o cadastro ou recupere sua senha.");
                }

                var user = new ApplicationUser()
                {
                    UserName = registerBeauty.Email.Split("@").FirstOrDefault(),
                    Email = registerBeauty.Email,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = Convert.ToInt32(decimal.Zero),
                     PhoneNumber = registerBeauty.PhoneNumber
                };

                IdentityResult addUserResult = await userManager.CreateAsync(user, registerBeauty.Secret);

                if (addUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, registerBeauty.AppName);
                    var code = Guid.NewGuid().ToString();
                    await userManager.AddClaimAsync(user, new Claim("CodeConfirmation", code));

                    var establishment = new Establishment()
                    {
                        Active = true,
                        Address = registerBeauty.Address,
                        Cnpj = registerBeauty.Cnpj,
                        Description = registerBeauty.Description,
                        Name = registerBeauty.Name,
                        Responsible = registerBeauty.Responsible,
                        TypeId = registerBeauty.TypeId,
                        AspNetUsersId = user.Id
                    };

                    _establishmentRepository.Insert(establishment);

                    var pathToSave = string.Concat(_hostEnvironment.ContentRootPath, configuration["pathFileEstablishment"]);
                    var fileName = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                    byte[] imageBytes = Convert.FromBase64String(registerBeauty.Base64);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            ms.CopyTo(stream);
                        }
                    }

                    sendEmailConfirmUser(registerBeauty.Email, code, user.Id, registerBeauty.AppName);
                }
                else
                {
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("PasswordTooShort")) { return BadRequest("A senha deve ter no mínimo 6 caracteres"); }
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("InvalidEmail")) { return BadRequest("E-mail inválido!"); }
                    if (addUserResult.Errors.FirstOrDefault().Code.Equals("InvalidUserName")) { return BadRequest("Nome do usuário inválido. Use apenas letras e números."); }
                    return BadRequest(addUserResult.Errors.FirstOrDefault().ToString());
                }

                return new JsonResult("Usuário registrado com sucesso! Verifique sua caixa de email e confirme o cadastro.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }

        }
    }
}
