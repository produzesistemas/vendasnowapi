using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using UnitOfWork;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Security.Policy;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Models.Checkout;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Configuration;

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
        public async Task<IActionResult> SaveAsync([FromBody] Subscription _subscription)
        {
            try
            {
                //ClaimsPrincipal currentUser = this.User;
                //var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                //if (id == null)
                //{
                //    return BadRequest("Identificação do usuário não encontrada.");
                //}

                using (var httpClient = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Post, configuration["UrlRequisicaoCielo"].ToString());
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Add("MerchantId", configuration["MerchantId"].ToString());
                    request.Headers.Add("MerchantKey", configuration["MerchantKey"].ToString());

                    var CreditCard = new CreditCard()
                    {
                        CardToken = _subscription.CardToken,
                        Brand = "Visa",
                        SecurityCode = "123"
                    };
                    var Payment = new Payment()
                    {
                         Amount= 1200,
                          Installments = 1,
                           Provider = "Simulado",
                            SoftDescriptor = "VendasNow",
                             Type = "CreditCard",
                              CreditCard= CreditCard
                    };
                    var MerchantOrder = new MerchantOrder()
                    {
                         MerchantOrderId = "500",
                        Payment = Payment
                    };
                    var stringJson = JsonConvert.SerializeObject(MerchantOrder);
                    request.Content = new StringContent(stringJson, Encoding.UTF8);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var content = JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync());
                    return new JsonResult(content);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
