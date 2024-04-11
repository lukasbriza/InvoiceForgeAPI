using InvoiceForgeApi.DTO.Model;
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
        public async Task<List<CountryGetRequest>?> GetCountries()
        {
            return await _codeListRepository.GetCountries();
        }
        [HttpGet]
        [Route("banks")]
        public async Task<List<BankGetRequest>?> GetBanks()
        {
            return await _codeListRepository.GetBanks();
        }
        [HttpGet]
        [Route("client-type")]
        public List<ClientTypeGetRequest>? GetClientTypes()
        {
            return _codeListRepository.GetClientTypes();
        }
        [HttpGet]
        [Route("all")]
        public async Task<CodeListsAllGetRequest> GetCodeListsAll()
        {
            return await _codeListRepository.GetCodeListsAll();
        }
    }
}
