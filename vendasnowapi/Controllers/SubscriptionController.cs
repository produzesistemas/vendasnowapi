using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using UnitOfWork;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Models.Checkout;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Repositorys;
using System.Security.Claims;
using System.Linq;
using LinqKit;
using System.Linq.Expressions;

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionRepository _subscriptionRepository;
        private IConfiguration configuration;

        public SubscriptionController(ISubscriptionRepository SubscriptionRepository,
            IConfiguration Configuration)
        {
            this._subscriptionRepository = SubscriptionRepository; 
            this.configuration = Configuration;
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public async Task<IActionResult> SaveAsync([FromBody] Subscription _subscription)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }

                using (var httpClient = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Post, configuration["UrlRequisicaoCielo"].ToString());
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Add("MerchantId", configuration["MerchantId"].ToString());
                    request.Headers.Add("MerchantKey", configuration["MerchantKey"].ToString());

                    var CreditCard = new CreditCard()
                    {
                        CardToken = _subscription.CardToken,
                        Brand = _subscription.Brand,
                        SecurityCode = _subscription.SecurityCode
                    };
                    var valueStr = Convert.ToString(_subscription.Value);
                    var str = valueStr.Split(",");
                    var decimals = str[1].PadRight(2, '0');
                    var amountFormated = string.Concat(str[0], decimals);
                    var Payment = new Payment()
                    {
                         Amount= Convert.ToInt32(amountFormated),
                          Installments = 1,
                           Provider = "Simulado",
                            SoftDescriptor = _subscription.AppName,
                             Type = "CreditCard",
                              CreditCard= CreditCard
                    };
                    var MerchantOrder = new MerchantOrder()
                    {
                         MerchantOrderId = id,
                        Payment = Payment
                    };
                    var stringJson = JsonConvert.SerializeObject(MerchantOrder);
                    request.Content = new StringContent(stringJson, Encoding.UTF8);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var merchantOrderReturn = JsonConvert.DeserializeObject<MerchantOrder>(await response.Content.ReadAsStringAsync());
                    if (merchantOrderReturn.Payment.ReturnCode == "4")
                    {
                        _subscription.AspNetUsersId = id;
                        _subscription.PaymentId = merchantOrderReturn.Payment.PaymentId;
                        _subscription.SubscriptionDate = DateTime.Now;
                        _subscriptionRepository.Insert(_subscription);
                    }
                    return new OkResult();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [Route("getCurrent")]
        [Authorize()]
        public IActionResult GetCurrent()
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                Expression<Func<Subscription, bool>> ps1, ps2;
                var pred = PredicateBuilder.New<Subscription>();
                ps1 = p => p.AspNetUsersId.Equals(id);
                pred = pred.And(ps1);
                ps2 = p => p.Active == true;
                pred = pred.And(ps2);
                return new JsonResult(_subscriptionRepository.GetCurrent(pred));
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento da conta: ", ex.Message));
            }
        }
    }
}
