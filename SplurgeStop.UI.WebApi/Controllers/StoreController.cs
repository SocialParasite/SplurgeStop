﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.DTO;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.StoreProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService service;

        public StoreController(IStoreService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<StoreStripped>> GetStores()
        {
            return await service.GetAllStoresStripped();
        }

        //[Route("StoreInfo")]
        [HttpGet("{id}")]
        public async Task<Store> GetStore(Guid id)
        {
            return await service.GetDetailedStore(id);
        }

        [HttpPost]
        public async Task<ActionResult<StoreCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new StoreId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Name))
                {
                    return new StoreCreated { Id = (Guid)request.Id, Name = request.Name };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Store was not in valid state! Name should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new Store =-O");

                throw;
            }
        }

        [Route("StoreInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.SetStoreName request) 
            => await RequestHandler.HandleCommand(request, service.Handle);
    }
}