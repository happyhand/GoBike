﻿using Microsoft.AspNetCore.Mvc;

namespace GoBikeAPI.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "ok";
        }

        [HttpPost]
        public string Post()
        {
            return "ok";
        }
    }
}