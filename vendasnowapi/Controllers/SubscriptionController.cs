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

namespace vendasnowapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionRepository _subscriptionRepository;
        public SubscriptionController(ISubscriptionRepository SubscriptionRepository)
        {
            this._subscriptionRepository = SubscriptionRepository;
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

                    var request = new HttpRequestMessage(HttpMethod.Post, "https://apisandbox.cieloecommerce.cielo.com.br/1/sales/");
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Add("MerchantId", "52b1e339-f735-4db9-9909-2408f4a23382");
                    request.Headers.Add("MerchantKey", "QTXQVXZMTHNJMVNFBCKRXZSGWAYMQQNFKPJPPSMI");

                    var CreditCard = new CreditCard()
                    {
                         Brand = "Visa",
                          CardNumber = "4024.0071.5376.3191",
                           ExpirationDate = "12/2021",
                            Holder = "Teste Holder",
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
                    var content = await response.Content.ReadAsStringAsync();
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
