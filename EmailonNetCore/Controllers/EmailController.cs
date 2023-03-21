using EmailonNetCore.Data;
using EmailonNetCore.DTOs;
using EmailonNetCore.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace EmailonNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly UserData _userdata;
        public EmailController(UserData userdata) 
        {
            _userdata = userdata;
        }

        [HttpPost]
        public IActionResult InputData([FromBody] InputUsersDTO inputDTO) 
        {
            try 
            {
                Users users = new()
                {
                    Name = inputDTO.Name,
                    Email = inputDTO.Email,
                    Task = inputDTO.Task,
                };

                bool result = _userdata.InputData(users);
                if (result)
                {
                    return StatusCode(201, "Data inserted");
                }
                else
                {
                    return StatusCode(500, "Data not inserted");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SendEmail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("zora.ratke@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("zora.ratke@ethereal.email"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587,SecureSocketOptions.StartTls);
            smtp.Authenticate("zora.ratke@ethereal.email", "ZdGbCHZChHZ9AVycy9");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
