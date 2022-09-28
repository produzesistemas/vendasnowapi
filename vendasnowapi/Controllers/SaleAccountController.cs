using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using UnitOfWork;

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleAccountController : ControllerBase
    {
        private IAccountRepository AccountRepository;
        public SaleAccountController(
   IAccountRepository AccountRepository

    )
        {
            this.AccountRepository = AccountRepository;
        }


        [HttpPost()]
        [Route("getAllByMonthAndYear")]
        [Authorize()]
        public IActionResult GetAllByMonthAndYear([FromBody] FilterDefault filter)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                Expression<Func<Account, bool>> p1, p2, p3;
                var predicate = PredicateBuilder.New<Account>();
                p1 = p => p.Sale.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                p2 = p => p.DueDate.Month == filter.Month;
                predicate = predicate.And(p2);
                p3 = p => p.DueDate.Year == filter.Year;
                predicate = predicate.And(p3);

                return new JsonResult(AccountRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento das vendas: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save(Account account)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                AccountRepository.Update(account);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na baixa da conta: ", ex.Message));
            }
        }
    }
}
