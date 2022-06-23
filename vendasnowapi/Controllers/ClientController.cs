using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class ClientController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IClientRepository ClientRepository;
        public ClientController(
    UserManager<ApplicationUser> userManager,
   IClientRepository ClientRepository

    )
        {
            this.ClientRepository = ClientRepository;
            this.userManager = userManager;
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

                Expression<Func<Client, bool>> p1;
                var predicate = PredicateBuilder.New<Client>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                return new JsonResult(ClientRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos Clientes: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save(Client client)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }

                if (client.Id > decimal.Zero)
                {
                    var clientBase = ClientRepository.Get(client.Id);
                    clientBase.Name = client.Name;
                    clientBase.Telephone = client.Telephone;
                    ClientRepository.Update(clientBase);
                }
                else
                {
                    client.AspNetUsersId = id;
                    ClientRepository.Insert(client);
                }
                return new JsonResult(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [Route("delete")]
        [Authorize()]
        public IActionResult Delete([FromBody] Client client)
        {
            try
            {
                ClientRepository.Delete(client.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na exclusão do cliente: ", ex.Message));

            }
        }
    }
}
