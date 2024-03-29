﻿using LinqKit;
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
    public class ClientController : ControllerBase
    {
        private IClientRepository _clientRepository;
        public ClientController(
   IClientRepository ClientRepository

    )
        {
            _clientRepository = ClientRepository;
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

                Expression<Func<Client, bool>> p1;
                var predicate = PredicateBuilder.New<Client>();
                p1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                return new JsonResult(_clientRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento dos Clientes: ", ex.Message));
            }
        }

        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save([FromBody] Client client)
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
                    _clientRepository.Update(client);
                }
                else
                {
                    client.AspNetUsersId = id;
                    _clientRepository.Insert(client);
                }
                return new OkResult();
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
                _clientRepository.Delete(client.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na exclusão do cliente: ", ex.Message));

            }
        }
    }
}
