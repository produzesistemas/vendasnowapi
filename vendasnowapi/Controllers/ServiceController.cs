using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models;
using Repositorys;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using UnitOfWork;

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private IServiceRepository _serviceRepository;
        private IConfiguration configuration;
        private IWebHostEnvironment _hostEnvironment;
        public ServiceController(
            IServiceRepository serviceRepository,
            IWebHostEnvironment environment,
            IConfiguration Configuration
            )
        {
            _serviceRepository = serviceRepository;
            _hostEnvironment = environment;
            this.configuration = Configuration;
        }


        [HttpGet()]
        [Route("getAll")]
        [Authorize()]
        public IActionResult GetAll()
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                var establishmentId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(z => z.Type.Contains("sid")).Value);
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                if (establishmentId == decimal.Zero)
                {
                    return BadRequest("Usuário sem Estabelecimento cadastrado.");
                }

                Expression<Func<Service, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Service>();
                ps1 = p => p.ApplicationUserId.Equals(id);
                pred = pred.And(ps1);
                ps2 = p => p.EstablishmentId == establishmentId;
                pred = pred.And(ps2);
                return new JsonResult(_serviceRepository.Where(pred).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos Serviços: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save([FromBody] Service service)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                var establishmentId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(z => z.Type.Contains("sid")).Value);
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                if (establishmentId == decimal.Zero)
                {
                    return BadRequest("Usuário sem Estabelecimento cadastrado.");
                }
                var pathToSave = string.Concat(_hostEnvironment.ContentRootPath, configuration["pathFileService"]);
                var fileDelete = pathToSave;
                if (service.Id > decimal.Zero)
                {
                    if (service.Base64 != "")
                    {
                        byte[] imageBytes = Convert.FromBase64String(service.Base64);
                        var fileName = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                        service.ImageName = fileName;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                ms.CopyTo(stream);
                            }
                        }

                    }
                    service.UpdateAspNetUsersId = id;
                    _serviceRepository.Update(service, pathToSave);


                } else
                {
                    if (service.Base64 != "")
                    {
                        byte[] imageBytes = Convert.FromBase64String(service.Base64);
                        var fileName = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                        service.ImageName = fileName;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                ms.CopyTo(stream);
                            }
                        }
                    }
                    service.ApplicationUserId = id;
                    service.EstablishmentId = establishmentId;
                    service.CreateDate = DateTime.Now;
                    service.Active = true;
                    _serviceRepository.Insert(service);
                }

                
                
                return new OkResult();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    return BadRequest("Já existe um serviço com essa descrição!");
                }
                return BadRequest(string.Concat("Falha no cadastro do serviço: ", ex.InnerException.Message));
            }
        }

        [HttpPost()]
        [Route("delete")]
        [Authorize()]
        public IActionResult Delete([FromBody] Service service)
        {
            try
            {
                _serviceRepository.Delete(service.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return BadRequest("O Serviço não pode ser excluído. Está relacionado com um profissional. Considere desativar!");
                }
                return BadRequest(string.Concat("Falha na exclusão do serviço: ", ex.Message));

            }
        }

        [HttpPost()]
        [Route("active")]
        [Authorize()]
        public IActionResult Active(Service service)
        {
            try
            {
                _serviceRepository.Active(service.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}
