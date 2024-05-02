using InvoiceForgeApi.Abl.contractor;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceForgeApi.Controllers
{
    [Route("api/contractor")]
    public class ContractorController : BaseController
    {
        public ContractorController(IRepositoryWrapper repository): base(repository) {}

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<List<ContractorGetRequest>?> GetAllContractors(int userId)
        {
            return await _repository.Contractor.GetAll(userId, false);
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<List<ContractorGetRequest>?> GetPlainAllContractors(int userId)
        {
            return await _repository.Contractor.GetAll(userId, true);
        }
        [HttpGet]
        [Route("{contractorId}")]
        public async Task<ContractorGetRequest?> GetByContractorId(int contractorId)
        {
            return await _repository.Contractor.GetById(contractorId);
        }
        [HttpGet]
        [Route("plain/{contractorId}")]
        public async Task<ContractorGetRequest?> GetPlainByContractorId(int contractorId)
        {
            return await _repository.Contractor.GetById(contractorId, true);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<bool> AddContractor(int userId, ContractorAddRequest contractor)
        {
            if (contractor is null) throw new ValidationError("Contractor is not provided.");
            var abl = new AddContractorAbl(_repository);
            var result = await abl.Resolve(userId, contractor);
            return result;
        }
        [HttpPut]
        [Route("{contractorId}")]
        public async Task<bool> UpdateContractor(int contractorId, ContractorUpdateRequest contractor)
        {
            if (contractor is null) throw new ValidationError("Contractor is not provided.");
            var abl = new UpdateContractorAbl(_repository);
            var result = await abl.Resolve(contractorId, contractor);
            return result;
            
        }
        [HttpDelete]
        [Route("{contractorId}")]
        public async Task<bool> DeleteContractor(int contractorId)
        {
            var abl = new DeleteContractorAbl(_repository);
            var result = await abl.Resolve(contractorId);
            return result;
        }
    }
}