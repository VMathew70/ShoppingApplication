using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.ProdDAL;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEMailService emailService;

        public EmailController(IEMailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailMessage(MailRequest request)
        {
            try
            {
                await emailService.SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);

            }
            return Ok("Messaged");

        }
    }
}
