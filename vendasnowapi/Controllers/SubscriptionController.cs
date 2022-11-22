using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositorys;
using System.Security.Claims;
using System;
using UnitOfWork;
using System.Linq;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using MercadoPago.Client;
using System.Threading.Tasks;
using MercadoPago.Client.Common;

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
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }

                var paymentRequest = new PaymentCreateRequest
                {
                    TransactionAmount = 15,
                    Token = "TEST-8552635698318850-111810-d0e19cd51c1930e4e89c33f37f8a8e16-1242016974",
                    Description = "Pagamento VendasNow Pro",
                    Installments = 1,
                    PaymentMethodId = "visa",
                    Payer = new PaymentPayerRequest
                    {
                        Email = "paulosergiosouzapereira@gmail.com",
                        Identification = new IdentificationRequest
                        {
                            Type = "credit_card",
                            Number = "4235647728025682",
                        },
                        FirstName = "Paulo"
                    },
                };

                //var client = new PaymentClient();
                //Payment payment = await client.CreateAsync(paymentRequest);




                var requestOptions = new RequestOptions();
                requestOptions.AccessToken = "TEST-8552635698318850-111810-d0e19cd51c1930e4e89c33f37f8a8e16-1242016974";


                var client = new PaymentClient();
                Payment payment = await client.CreateAsync(paymentRequest, requestOptions);

                //_subscriptionRepository.Insert(_subscription);

                return new JsonResult(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
