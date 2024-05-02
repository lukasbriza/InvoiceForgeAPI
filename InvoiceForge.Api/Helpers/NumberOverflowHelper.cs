namespace InvoiceForgeApi.Helpers
{
    public class NumberOverflowHelper
    {
        readonly int _numberOfNumberVariables;
        public NumberOverflowHelper(int numberOfNumberVariables)
        {
            _numberOfNumberVariables = numberOfNumberVariables;
        }
        public bool IsOverflowingOnAdd(int number)
        {
            int newNumber = number + 1;
            string newStringNumber = newNumber.ToString();
            int lnNew = newStringNumber.Length;
            bool isOverflowing = _numberOfNumberVariables < lnNew;
            return isOverflowing;
        }
    }
}