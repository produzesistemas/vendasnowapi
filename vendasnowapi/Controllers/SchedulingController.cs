using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Filters;
using Repositorys;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using UnitOfWork;

namespace scheduling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulingController : ControllerBase
    {
        private ISchedulingRepository _SchedulingRepository;
        private ISchedulingTrackRepository _SchedulingTrackRepository;
        private ISchedulingEmailRepository _SchedulingEmailRepository;
        public SchedulingController(
            ISchedulingRepository SchedulingRepository,
            ISchedulingTrackRepository SchedulingTrackRepository,
            ISchedulingEmailRepository SchedulingEmailRepository
            )
        {
            _SchedulingRepository = SchedulingRepository;
            _SchedulingTrackRepository = SchedulingTrackRepository;
            _SchedulingEmailRepository = SchedulingEmailRepository;
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save([FromBody] Scheduling scheduling)
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
                scheduling.ApplicationUserId = id;
                scheduling.CreateDate = DateTime.Now;
                _SchedulingRepository.Insert(scheduling);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no agendamento: ", ex.InnerException.Message));
            }
        }

        [HttpPost()]
        [Route("getByUser")]
        [Authorize()]
        public IActionResult GetByUser(FilterDefault filter)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                filter.ApplicationUserId= id;
                return new JsonResult(_SchedulingRepository.GetPagination(filter).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos agendamentos ", ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Authorize()]
        public IActionResult Get(int id)
        {
            try
            {
                return new JsonResult(_SchedulingRepository.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest("Agendamento não encontrado!" + ex.Message);
            }
        }

        [HttpPost()]
        [Route("cancelByClient")]
        [Authorize()]
        public IActionResult CancelByClient(FilterDefault filter)
        {
            try
            {
                if ((filter != null) && (filter.Id > decimal.Zero))
                {
                    _SchedulingRepository.CancelByClient(filter.Id);
                    return new OkResult();
                } else
                {
                    return BadRequest("Identificação do agendamento não encontrado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Falha no cancelamento do agendamento. Entre em contato com o administrador do sistema. " + ex);
            }
        }

        [HttpPost()]
        [Route("cancelByPartner")]
        [Authorize()]
        public IActionResult CancelByPartner(FilterDefault filter)
        {
            try
            {
                if ((filter != null) && (filter.Id > decimal.Zero))
                {
                    _SchedulingRepository.CancelByPartner(filter.Id);
                    return new OkResult();
                }
                else
                {
                    return BadRequest("Identificação do agendamento não encontrado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Falha no cancelamento do agendamento. Entre em contato com o administrador do sistema. " + ex);
            }
        }

        [HttpPost()]
        [Route("confirm")]
        [Authorize()]
        public IActionResult Confirm(FilterDefault filter)
        {
            try
            {
                if ((filter != null) && (filter.Id > decimal.Zero))
                {
                    _SchedulingRepository.Confirm(filter.Id);
                    return new OkResult();
                }
                else
                {
                    return BadRequest("Identificação do agendamento não encontrado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Falha na confirmação do agendamento. Entre em contato com o administrador do sistema. " + ex);
            }
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

                Expression<Func<Scheduling, bool>> ps2;
                var pred = PredicateBuilder.New<Scheduling>();
                ps2 = p => p.EstablishmentId == establishmentId;
                pred = pred.And(ps2);
                return new JsonResult(_SchedulingRepository.Where(pred).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos Serviços: ", ex.Message));
            }
        }

    }
}
