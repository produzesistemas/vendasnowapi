using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
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
    public class ProfessionalController : ControllerBase
    {
        private IProfessionalRepository _professionalRepository;
        private IConfiguration configuration;
        private IWebHostEnvironment _hostEnvironment;
        public ProfessionalController(
            IProfessionalRepository professionalRepository,
            IWebHostEnvironment environment,
            IConfiguration Configuration
            )
        {
            _professionalRepository = professionalRepository;
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

                Expression<Func<Professional, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Professional>();
                ps1 = p => p.ApplicationUserId.Equals(id);
                pred = pred.And(ps1);
                ps2 = p => p.EstablishmentId == establishmentId;
                pred = pred.And(ps2);
                return new JsonResult(_professionalRepository.Where(pred).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos Profissionais: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save([FromBody] Professional professional)
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
                var pathToSave = string.Concat(_hostEnvironment.ContentRootPath, configuration["pathFileProfessional"]);
                var fileDelete = pathToSave;
                if (professional.Id > decimal.Zero)
                {
                    if (professional.Base64 != "")
                    {
                        byte[] imageBytes = Convert.FromBase64String(professional.Base64);
                        var fileName = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                        professional.ImageName = fileName;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                ms.CopyTo(stream);
                            }
                        }

                    }
                    professional.UpdateAspNetUsersId = id;
                    _professionalRepository.Update(professional, pathToSave);


                } else
                {
                    if (professional.Base64 != "")
                    {
                        byte[] imageBytes = Convert.FromBase64String(professional.Base64);
                        var fileName = string.Concat(Guid.NewGuid().ToString(), ".jpg");
                        professional.ImageName = fileName;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                ms.CopyTo(stream);
                            }
                        }
                    }
                    professional.ApplicationUserId = id;
                    professional.EstablishmentId = establishmentId;
                    professional.CreateDate = DateTime.Now;
                    professional.Active = true;
                    _professionalRepository.Insert(professional);
                }

                
                
                return new OkResult();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    return BadRequest("Já existe um profissional com esse nome!");
                }
                return BadRequest(string.Concat("Falha no cadastro do profissional: ", ex.InnerException.Message));
            }
        }

        [HttpPost()]
        [Route("delete")]
        [Authorize()]
        public IActionResult Delete([FromBody] Professional professional)
        {
            try
            {
                var pathToSave = string.Concat(_hostEnvironment.ContentRootPath, configuration["pathFileService"]);
                var fileDelete = pathToSave;
                _professionalRepository.Delete(professional.Id, fileDelete);
                return new OkResult();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return BadRequest("O Profissional não pode ser excluído. Está relacionado com um agendamento existente. Considere desativar!");
                }
                return BadRequest(string.Concat("Falha na exclusão do profissional: ", ex.Message));

            }
        }

        [HttpPost()]
        [Route("active")]
        [Authorize()]
        public IActionResult Active(Professional professional)
        {
            try
            {
                _professionalRepository.Active(professional.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}
