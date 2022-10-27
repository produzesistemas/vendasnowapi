﻿using LinqKit;
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
    public class SaleAccountController : ControllerBase
    {
        private IAccountRepository AccountRepository;
        public SaleAccountController(
   IAccountRepository AccountRepository

    )
        {
            this.AccountRepository = AccountRepository;
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
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                Expression<Func<Account, bool>> p1, p2, p3, p4;
                var predicate = PredicateBuilder.New<Account>();
                p1 = p => p.Sale.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                p2 = p => p.DueDate.Month == filter.Month;
                predicate = predicate.And(p2);
                p3 = p => p.DueDate.Year == filter.Year;
                predicate = predicate.And(p3);

                if (filter.Status > decimal.Zero)
                {
                    p4 = p => p.Status == filter.Status;
                    predicate = predicate.And(p4);
                }

                return new JsonResult(AccountRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento das vendas: ", ex.Message));
            }
        }


        [HttpPost()]
        [Route("save")]
        [Authorize()]
        public IActionResult Save(Account account)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                account.DateOfPayment = DateTime.Now;
                AccountRepository.Update(account);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha na baixa da conta: ", ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Authorize()]
        public IActionResult Get(int id)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                if (currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value == null)
                {
                    return BadRequest("Para acessar a conta é necessário fazer login no aplicativo!");
                }
                return new JsonResult(AccountRepository.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest("Não foi possível carregar o model: " + ex.Message);
            }
        }

        [HttpGet()]
        [Route("getAllToNotification")]
        [Authorize()]
        public IActionResult GetAllToNotification()
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var id = currentUser.Claims.FirstOrDefault(z => z.Type.Contains("primarysid")).Value;
                if (id == null)
                {
                    return BadRequest("Identificação do usuário não encontrada.");
                }
                Expression<Func<Account, bool>> p1, p2, p3;
                var predicate = PredicateBuilder.New<Account>();
                p1 = p => p.Sale.AspNetUsersId.Equals(id);
                predicate = predicate.And(p1);
                p2 = p => p.DueDate.Date <= DateTime.Now.Date;
                predicate = predicate.And(p2);
                p3 = p => p.Status == 1;
                predicate = predicate.And(p3);
                return new JsonResult(AccountRepository.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(string.Concat("Falha no carregamento das vendas: ", ex.Message));
            }
        }


    }
}