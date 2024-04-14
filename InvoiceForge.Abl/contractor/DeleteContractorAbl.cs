
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.contractor
{
    public class DeleteContractorAbl: AblBase
    {
        public DeleteContractorAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int contractorId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    var hasInvoiceTemplatesReference = await _repository.InvoiceTemplate.GetByCondition((t) => t.ContractorId == contractorId);
                    if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new ValidationError("Can´t delete. Still assigned to some entity.");

                    bool deleteContractor = await _repository.Contractor.Delete(contractorId);

                    await SaveResult(deleteContractor, transaction);
                    return deleteContractor;

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}