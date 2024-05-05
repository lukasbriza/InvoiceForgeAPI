using InvoiceForgeApi.Abl.contractor;
using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
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
        public async Task<CustomResponse<List<ContractorGetRequest>>> GetAllContractors(int userId)
        {
            var task = await _repository.Contractor.GetAll(userId, false);
            return CreateRepsonse(task ?? new List<ContractorGetRequest>());
        }
        [HttpGet]
        [Route("plain/all/{userId}")]
        public async Task<CustomResponse<List<ContractorGetRequest>>> GetPlainAllContractors(int userId)
        {
            var task = await _repository.Contractor.GetAll(userId, true);
            return CreateRepsonse(task ?? new List<ContractorGetRequest>());
        }
        [HttpGet]
        [Route("{contractorId}")]
        public async Task<CustomResponse<ContractorGetRequest?>> GetByContractorId(int contractorId)
        {
            var task = await _repository.Contractor.GetById(contractorId);
            return CreateRepsonse(task);
        }
        [HttpGet]
        [Route("plain/{contractorId}")]
        public async Task<CustomResponse<ContractorGetRequest?>> GetPlainByContractorId(int contractorId)
        {
            var task = await _repository.Contractor.GetById(contractorId, true);
            return CreateRepsonse(task);
        }
        [HttpPost]
        [Route("{userId}")]
        public async Task<CustomResponse<bool>> AddContractor(int userId, ContractorAddRequest contractor)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }

            var abl = new AddContractorAbl(_repository);
            var result = await abl.Resolve(userId, contractor);
            return CreateRepsonse(result);
        }
        [HttpPut]
        [Route("{contractorId}")]
        public async Task<CustomResponse<bool>> UpdateContractor(int contractorId, ContractorUpdateRequest contractor)
        {
            if(!ModelState.IsValid){
                throw new InvalidModelError();
            }
            
            var abl = new UpdateContractorAbl(_repository);
            var result = await abl.Resolve(contractorId, contractor);
            return CreateRepsonse(result);
            
        }
        [HttpDelete]
        [Route("{contractorId}")]
        public async Task<CustomResponse<bool>> DeleteContractor(int contractorId)
        {
            var abl = new DeleteContractorAbl(_repository);
            var result = await abl.Resolve(contractorId);
            return CreateRepsonse(result);
        }
    }
}