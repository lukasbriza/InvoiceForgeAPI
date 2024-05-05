using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.client
{
    public class DeleteClientAbl: AblBase
    {
        public DeleteClientAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int clientId)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    var hasInvoiceTemplatesReference = await _repository.InvoiceTemplate.GetByCondition((t) => t.ClientId == clientId);
                    if (hasInvoiceTemplatesReference is not null && hasInvoiceTemplatesReference.Count > 0) throw new EntityReferenceError();
                    
                    bool deleteClient = await _repository.Client.Delete(clientId);

                    await SaveResult(deleteClient, transaction);
                    return deleteClient;
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