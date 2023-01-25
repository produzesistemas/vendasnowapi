using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class OpeningHoursController : ControllerBase
    {
        private IOpeningHoursRepository _OpeningHoursRepository;
        public OpeningHoursController(
   IOpeningHoursRepository OpeningHoursRepository
    )
        {
            _OpeningHoursRepository = OpeningHoursRepository;
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

                    Expression<Func<OpeningHours, bool>> ps1, ps2;
                    var pred = PredicateBuilder.New<OpeningHours>();
                    ps2 = p => p.EstablishmentId == establishmentId;
                    pred = pred.And(ps2);
                    return new JsonResult(_OpeningHoursRepository.Where(pred).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(string.Concat("Falha no carregamento dos Horários: ", ex.Message));
                }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save([FromBody] OpeningHours openingHours)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }

                if (openingHours.Id > decimal.Zero)
                {
                    _OpeningHoursRepository.Update(openingHours);
                }
                else
                {
                    openingHours.ApplicationUserId = id;
                    _OpeningHoursRepository.Insert(openingHours);
                }
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no cadastro do horário: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("delete")]
        [Authorize()]
        public IActionResult Delete([FromBody] OpeningHours openingHours)
        {
            try
            {
                _OpeningHoursRepository.Delete(openingHours.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na exclusão do produto: ", ex.Message));

            }
        }

        [HttpPost()]
        [Route("active")]
        [Authorize()]
        public IActionResult Active(OpeningHours openingHours)
        {
            try
            {
                _OpeningHoursRepository.Active(openingHours.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}
