using AutoMapper;
using LipaNaMpesaAPI.Dto;
using LipaNaMpesaAPI.Models;
using LipaNaMpesaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LipaNaMpesaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MpesaController : ControllerBase
    {
        private readonly MpesaSettingsContext _db;
        private readonly IMapper _mapper;

        public MpesaController(MpesaSettingsContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }

        [HttpPost("MpesaConfugurations")]
        public async Task<IActionResult> CreateStudent(CreateSettingsDto mpesaSettings)
        {
            if (await PaybillExists(mpesaSettings.BusinessCode))
            {
                return Unauthorized("Paybill already exists");
            }

            Random random = new();
            var mpesaConfig = new Setting
            {
                ConsumerKey = mpesaSettings.ConsumerKey,
                ConsumerSecret =mpesaSettings.ConsumerSecret ,
                BusinessCode = mpesaSettings.BusinessCode,
                BusinessName = mpesaSettings.BusinessName,
                BusinessId = mpesaSettings.BusinessName.Substring(0, 4).ToUpper()+random.Next(1000).ToString(),
                PassKey = mpesaSettings.PassKey,
                TransactionDesc = mpesaSettings.TransactionDesc,
                DateCreated = DateTime.Now
            };

            await _db.Settings.AddAsync(mpesaConfig);
            await _db.SaveChangesAsync();

            return Created("Confiurations saved succesfully", mpesaSettings);
        }

        [HttpGet("{businessID}")]
        public async Task<IActionResult> GetOneStudentInfo(string businessID)
        {
            var config = await _db.Settings.SingleOrDefaultAsync(x => x.BusinessId.ToUpper() == businessID.ToUpper());
            if (config == null)
            {
                return NotFound();
            }
            var configDto = _mapper.Map<GetConfigDto>(config);
            return Ok(configDto);
        }

        [HttpGet]
        public async Task<IActionResult> MakePayment(string businessID, string phoneNumber, string Amount, string AccountNumber)
        {
            var config = await _db.Settings.SingleOrDefaultAsync(x => x.BusinessId.ToUpper() == businessID.ToUpper());
            if (config == null)
            {
                return NotFound();
            }
            var configDto = _mapper.Map<GetConfigDto>(config);
            

            phoneNumber = phoneNumber.Remove(0, 1);
            string number = "254" + phoneNumber;

            string businessCode = configDto.BusinessCode;
            string consumerKey = configDto.ConsumerKey;
            string consumerSecret = configDto.ConsumerSecret;
            string passKey = configDto.PassKey;
            string description = configDto.TransactionDesc;

            bool mpesaResponse = StkPush.MpesaStk(number, Amount, AccountNumber, businessCode, consumerKey, consumerSecret, passKey, description);
            if (mpesaResponse == true)
            {
                return new ObjectResult(new
                {
                    message = "Success",

                });
            }
            else
            {
                return new ObjectResult(new
                {
                    message = "Failed",

                });
            }
        }




        private async Task<bool> PaybillExists(string paybill)
        {
            return await _db.Settings.AnyAsync(x => x.BusinessCode == paybill);
        }
    }
}
