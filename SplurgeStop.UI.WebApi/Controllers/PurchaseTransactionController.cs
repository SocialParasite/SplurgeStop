using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.UI.WebApi.Common;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseTransactionController : ControllerBase
    {
        private readonly PurchaseTransactionService service;

        public PurchaseTransactionController(PurchaseTransactionService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public Task<IActionResult> Post(Commands.Create request)
        {
            try
            {
                return RequestHandler.HandleCommand(request, service.Handle);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened =-O");
                //return new BadRequestResult();
                throw;
            }
            //return RequestHandler.HandleCommand(request, service.Handle);
        }

        [Route("purchaseDate")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.SetPurchaseTransactionDate request)
            => RequestHandler.HandleCommand(request, service.Handle);
    }
}