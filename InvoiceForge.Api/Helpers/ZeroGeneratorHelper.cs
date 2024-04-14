namespace InvoiceForgeApi.Helpers
{
    public class ZeroGeneratorHelper
    {
        public string Generate(int numberOfZeros)
        {
            var prefix = "";

            for (var i = 0; i < numberOfZeros; i++)
            {
                prefix += "0";
            }

            return prefix;
        }
    }
}