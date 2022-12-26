using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;
using System;
using UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using PagarmeCoreApi.PCL;

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

                string basicAuthUserName = "basicAuthUserName"; // The username to use with basic authentication
                string basicAuthPassword = "basicAuthPassword"; // The password to use with basic authentication

                PagarmeCoreApiClient client = new PagarmeCoreApiClient(basicAuthUserName, basicAuthPassword);


                return new JsonResult(_subscription);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
