using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController: ControllerBase
    {
        public IRepositoryWrapper _repository;
        protected BaseController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
    }
}