namespace InvoiceForgeApi.Helpers
{
    public class NumberOverflowHelper
    {
        public bool IsOverflowingOnAdd(int number)
        {
            string stringNumber = number.ToString();
            int lnActual = stringNumber.Length;
            int newNumber = number + 1;
            string newStringNumber = newNumber.ToString();
            int lnNew = newStringNumber.Length;
            return lnActual != lnNew;
        }
    }
}