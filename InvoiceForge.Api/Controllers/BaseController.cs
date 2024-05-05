using InvoiceForgeApi.Helpers;
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
        public CustomResponse<T> CreateRepsonse<T> (T data)
        {
            var builder = new ResponseBuilder<T>(data, null);
            return builder.Get();
        }
    }
}