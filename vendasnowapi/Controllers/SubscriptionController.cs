using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using UnitOfWork;
using System.Threading.Tasks;
using System.Net.Http;

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

                //string basicAuthUserName = "basicAuthUserName"; // The username to use with basic authentication
                //string basicAuthPassword = "basicAuthPassword"; // The password to use with basic authentication

                //PagarmeCoreApiClient client = new PagarmeCoreApiClient(basicAuthUserName, basicAuthPassword);

                using (var httpClient = new HttpClient())
                {
                    var transaction = new Models.PagarMe.Transaction();
                    transaction.amount = "1000";
                    transaction.api_key = "pk_weQ4owu0GSPx4yNJ";
                    transaction.capture = "false";
                    transaction.card_hash = _subscription.Card_Hash;
                    transaction.Customer = new Models.PagarMe.Customer();
                    transaction.Customer.document_number = "92545278157";
                    transaction.Customer.email = "jappleseed@apple.com";
                    transaction.Customer.name = "John Appleseed";
                    transaction.Customer.Address = new Models.PagarMe.Address();
                    transaction.Customer.Address.neighborhood = "Bonfim";
                    transaction.Customer.Address.street = "Rua da Imperatriz, 80";
                    transaction.Customer.Address.street_number = "80";
                    transaction.Customer.Address.zipcode = "40415180";
                    transaction.Customer.Phone = new Models.PagarMe.Phone();
                    transaction.Customer.Phone.ddd = "71";
                    transaction.Customer.Phone.number = "994100367";



                    return new JsonResult(transaction);


                }





                    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
