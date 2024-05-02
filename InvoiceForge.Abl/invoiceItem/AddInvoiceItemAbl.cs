using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Abl.invoiceItem
{
    public class AddInvoiceItemAbl: AblBase
    {
        public AddInvoiceItemAbl(IRepositoryWrapper repository): base(repository) {}

        public async Task<bool> Resolve(int userId, InvoiceItemAddRequest invoiceItem)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    Tariff isTariff = await IsInDatabase<Tariff>(invoiceItem.TariffId, "Invalid tariff Id.");
                    
                    List<InvoiceItem>? isDuplicitInvoiceItemName = await _repository.InvoiceItem.GetByCondition(i => i.ItemName == invoiceItem.ItemName && i.Owner == userId);
                    if (isDuplicitInvoiceItemName is not null && isDuplicitInvoiceItemName.Any()) throw new ValidationError("Invoice item name must be unique.");

                    int? addInvoiceItem = await _repository.InvoiceItem.Add(userId, invoiceItem);
                    bool saveCondition = addInvoiceItem is not null;

                    await SaveResult(saveCondition, transaction, false);
                    return saveCondition;
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