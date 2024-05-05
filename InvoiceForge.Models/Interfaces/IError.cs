namespace InvoiceForgeApi.Models.Interfaces
{
    public interface IError 
    {
        string name { get; set;}
        string message { get; set; }
    }
}