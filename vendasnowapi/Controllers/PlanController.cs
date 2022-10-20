using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Filters;
using System;
using System.Linq.Expressions;
using UnitOfWork;

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private IPlanRepository PlanRepository;
        public PlanController(
   IPlanRepository PlanRepository

    )
        {
            this.PlanRepository = PlanRepository;
        }

        [HttpGet()]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                Expression<Func<Plan, bool>> p1;
                var predicate = PredicateBuilder.New<Plan>();
                p1 = p => p.Active == true;
                predicate = predicate.And(p1);
                return new JsonResult(PlanRepository.Where(predicate));
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos planos de pagamento: ", ex.Message));
            }
        }

    }
}
