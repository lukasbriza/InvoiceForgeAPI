using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("client")]
    public class Client : ControllerBase
    {
        public Client()
        {
           
        }

        [HttpGet]
        public bool Get()
        {
            return true;
        }
    }
}