using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using UnitOfWork;

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productRepository;
        private ISubscriptionRepository _subscriptionRepository;
        public ProductController(
   IProductRepository ProductRepository, ISubscriptionRepository subscriptionRepository

    )
        {
            _productRepository = ProductRepository;
            _subscriptionRepository = subscriptionRepository;
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

                Expression<Func<Product, bool>> p1;
                var predicate = PredicateBuilder.New<Product>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                return new JsonResult(_productRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos Clientes: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save([FromBody] Product product)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }

                if (product.Id > decimal.Zero)
                {
                    _productRepository.Update(product);
                }
                else
                {
                    product.AspNetUsersId = id;
                    _productRepository.Insert(product);
                }
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
        public IActionResult Delete([FromBody] Product product)
        {
            try
            {
                _productRepository.Delete(product.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na exclusão do produto: ", ex.Message));

            }
        }
    }
}
