using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/contractor")]
    public class ContractorController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public ContractorController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        
    }
}