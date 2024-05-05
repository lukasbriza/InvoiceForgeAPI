using InvoiceForgeApi.Helpers;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/code-lists")]
    public class CodeListsController: BaseController
    {
        public CodeListsController(IRepositoryWrapper repository): base(repository) {}

        [HttpGet]
        [Route("countries")]
        public async Task<CustomResponse<List<CountryGetRequest>>> GetCountries()
        {
            var task = await _repository.CodeLists.GetCountries();
            return CreateRepsonse(task ?? new List<CountryGetRequest>());
        }
        [HttpGet]
        [Route("banks")]
        public async Task<CustomResponse<List<BankGetRequest>>> GetBanks()
        {
            var task = await _repository.CodeLists.GetBanks();
            return CreateRepsonse(task ?? new List<BankGetRequest>());
        }
        [HttpGet]
        [Route("client-type")]
        public async Task<CustomResponse<List<ClientTypeGetRequest>>> GetClientTypes()
        {
            var task = await Task.FromResult(_repository.CodeLists.GetClientTypes());
            return CreateRepsonse(task ?? new List<ClientTypeGetRequest>());
        }
        [HttpGet]
        [Route("all")]
        public async Task<CustomResponse<CodeListsAllGetRequest?>> GetCodeListsAll()
        {
            var task = await _repository.CodeLists.GetCodeListsAll();
            return CreateRepsonse(task ?? null);
        }
    }
}
