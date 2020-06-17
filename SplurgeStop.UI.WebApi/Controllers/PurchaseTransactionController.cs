﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.PurchaseTransaction.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseTransactionController : ControllerBase
    {
        private readonly IPurchaseTransactionService service;

        public PurchaseTransactionController(IPurchaseTransactionService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<PurchaseTransactionStripped>> GetPurchaseTransactions(Period period = Period.Year, int number = 1)
        {
            // TODO:
            // "number" e.g. Period.Year, 2020 / Period.Month, 12 / Period.Week, 42 / Period.Day 30 ????
            // e.g. SearchItem { Period, INumber }
            return await service.GetAllPurchaseTransactions();
        }

        [HttpGet("{id}")]
        public async Task<PurchaseTransaction> GetPurchaseTransaction(Guid id)
        {
            return await service.GetDetailedPurchaseTransaction(id);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseTransactionCreated>> Post(Commands.CreateFull request)
        {
            try
            {
                request.Id = new PurchaseTransactionId(SequentialGuid.NewSequentialGuid());
                await RequestHandler.HandleCommand(request, service.Handle);

                //return new PurchaseTransactionCreated { Id = (Guid)request.Id };
                return RedirectToAction("GetPurchaseTransaction", new { id = request.Id });
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened =-O");
                //return new BadRequestResult();
                throw;
            }
        }

        [Route("purchaseDate")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.SetPurchaseTransactionDate request)
            => RequestHandler.HandleCommand(request, service.Handle);

        [Route("purchaseStore")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.SetPurchaseTransactionStore request)
            => RequestHandler.HandleCommand(request, service.Handle);

        [Route("purchaseLineItem")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.SetPurchaseTransactionLineItem request)
            => RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<PurchaseTransactionDeleted>> Delete(Commands.DeletePurchaseTransaction request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new PurchaseTransactionDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }

    public enum Period
    {
        Day,
        Week,
        Month,
        Year
    }
}