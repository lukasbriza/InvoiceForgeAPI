using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/contractor")]
    public class InvoiceTemplateController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public InvoiceTemplateController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        
    }
}