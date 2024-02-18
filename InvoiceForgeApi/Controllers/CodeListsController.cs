using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [ApiController]
    [Route("api/code-lists")]
    public class CodeListsController: ControllerBase
    {
        private readonly ICodeListsRepository _codeListsRepository;

        public CodeListsController(ICodeListsRepository codeListsRepository)
        {
            _codeListsRepository = codeListsRepository;
        }

        [HttpGet]
        [Route("countries")]
        public async Task<List<CountryGetRequest>?> GetCountries()
        {
            return await _codeListsRepository.GetCountries();
        }
        [HttpGet]
        [Route("banks")]
        public async Task<List<BankGetRequest>?> GetBanks()
        {
            return await _codeListsRepository.GetBanks();
        }
        [HttpGet]
        [Route("all")]
        public async Task<CodeListsAllGetRequest> GetCodeListsAll()
        {
            return await _codeListsRepository.GetCodeListsAll();
        }
    }
}
