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
        private ISaleRepository _saleRepository;
        private ISubscriptionRepository _subscriptionRepository;
        public SaleController(ISaleRepository SaleRepository, ISubscriptionRepository subscriptionRepository)
        {
            _saleRepository = SaleRepository;
            _subscriptionRepository = subscriptionRepository;
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
                var subscriptionExpire = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("userdata")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                if (subscriptionExpire == null)
                {
                    return BadRequest("Usuário sem assinatura.");
                }

                var expireDate = Convert.ToDateTime(subscriptionExpire);
                if (expireDate.Date < DateTime.Now.Date)
                {
                    return StatusCode(600);
                }


                Expression<Func<Subscription, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Subscription>();
                ps1 = p => p.AspNetUsersId.Equals(id);
                pred = pred.And(ps1);
                ps2 = p => p.Active == true;
                pred = pred.And(ps2);
                var subscription = _subscriptionRepository.GetCurrent(pred);
                if (subscription == null)
                {
                    return StatusCode(600);
                }
                else
                {
                    if (subscription.SubscriptionDate.AddDays(subscription.Plan.Days).Date < DateTime.Now.Date)
                    {
                        return StatusCode(600);
                    }
                }

                Expression<Func<Sale, bool>> p1, p2, p3;
                var predicate = PredicateBuilder.New<Sale>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                if (filter.Month > decimal.Zero)
                {
                    p2 = p => p.SaleDate.Month == filter.Month;
                    predicate = predicate.And(p2);
                }

                if (filter.Year > decimal.Zero)
                {
                    p3 = p => p.SaleDate.Year == filter.Year;
                    predicate = predicate.And(p3);
                }
                return new JsonResult(_saleRepository.Where(predicate).ToList());
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
                var subscriptionExpire = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("userdata")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                if (subscriptionExpire == null)
                {
                    return BadRequest("Usuário sem assinatura.");
                }

                var expireDate = Convert.ToDateTime(subscriptionExpire);
                if (expireDate.Date < DateTime.Now.Date)
                {
                    return StatusCode(600);
                }


                Expression<Func<Subscription, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Subscription>();
                ps1 = p => p.AspNetUsersId.Equals(id);
                pred = pred.And(ps1);
                ps2 = p => p.Active == true;
                pred = pred.And(ps2);
                var subscription = _subscriptionRepository.GetCurrent(pred);
                if (subscription == null)
                {
                    return StatusCode(600);
                }
                else
                {
                    if (subscription.SubscriptionDate.AddDays(subscription.Plan.Days).Date < DateTime.Now.Date)
                    {
                        return StatusCode(600);
                    }
                }

                Expression<Func<Sale, bool>> p1;
                var predicate = PredicateBuilder.New<Sale>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                return new JsonResult(_saleRepository.Where(predicate).ToList());
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
                _saleRepository.Insert(sale);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no cadastro da venda: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("delete")]
        [Authorize()]
        public IActionResult Delete([FromBody] Sale sale)
        {
            try
            {
                _saleRepository.Delete(sale.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na exclusão da venda: ", ex.Message));

            }
        }

    }
}
