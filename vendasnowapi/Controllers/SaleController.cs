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
    public class SaleController : ControllerBase
    {
        private ISaleRepository SaleRepository;
        public SaleController(
   ISaleRepository SaleRepository

    )
        {
            this.SaleRepository = SaleRepository;
        }


        [HttpPost()]
        [Route("getPagination")]
        [Authorize()]
        public IActionResult GetPagination(FilterDefault filter)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                Expression<Func<Sale, bool>> p1;
                var predicate = PredicateBuilder.New<Sale>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                return new JsonResult(SaleRepository.GetPagination(predicate, filter.SizePage).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento das vendas: ", ex.Message));
            }
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
                Expression<Func<Sale, bool>> p1, p2, p3;
                var predicate = PredicateBuilder.New<Sale>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                p2 = p => p.SaleDate.Month == filter.Month;
                predicate = predicate.And(p2);
                p3 = p => p.SaleDate.Year == filter.Year;
                predicate = predicate.And(p3);

                return new JsonResult(SaleRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento das vendas: ", ex.Message));
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
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }

                Expression<Func<Sale, bool>> p1;
                var predicate = PredicateBuilder.New<Sale>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                return new JsonResult(SaleRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento das vendas: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save(Sale sale)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                sale.AspNetUsersId = id;
                sale.CreateDate = DateTime.Now;
                SaleRepository.Insert(sale);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no cadastro do produto: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("delete")]
        [Authorize()]
        public IActionResult Delete([FromBody] Sale sale)
        {
            try
            {
                SaleRepository.Delete(sale.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na exclusão do produto: ", ex.Message));

            }
        }

    }
}
