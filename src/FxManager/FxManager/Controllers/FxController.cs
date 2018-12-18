﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FxManager.Models;
using FxManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FxManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FxController : ControllerBase
    {
        private readonly IFxService _fxService;

        public FxController(IFxService fxService)
        {
            _fxService = fxService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<FxResponse> Get([FromQuery] FxRequest request)
        {
            var response = _fxService.GetRate(request.BaseCurrency, request.TargetCurrency);

            return response;
        }
    }
}