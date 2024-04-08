using InvoiceForgeApi.Enum;
using InvoiceForgeApi.DTO;

namespace InvoiceForgeApi.Helpers
{
    public class ResolveDTO {
        public string? InvoiceNumber { get; set; } = null;
        public bool Overflowed { get; set; }
        public bool Resolved { get; set; }
    }
    public class InvoiceNumberMatrix
    {
        readonly List<NumberingVariable> _numberingTemplate;
        readonly DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        string _invoiceNumber = "";
        string _replaceString = "";
        string? _lastInvoiceNumber = null;
        string? _lastInvoiceYear = null;
        string? _lastInvoiceMonth = null;
        string? _lastInvoiceDay = null;
        string? _actualInvoiceYear = null;
        string? _actualInvoiceMonth = null;
        string? _actualInvoiceDay = null;


        public InvoiceNumberMatrix(List<NumberingVariable> numberingTemplate, string? lastInvoiceNumber)
        {
            _numberingTemplate = numberingTemplate;
            _lastInvoiceNumber = lastInvoiceNumber;

            if (lastInvoiceNumber is not null)
            {
                //EXTRACT LAST INVOICE VALUES
                int pointer = 0;
                numberingTemplate.ForEach((variable) => {
                    if (lastInvoiceNumber is not null)
                    {
                        if (variable == NumberingVariable.Day)
                        {
                            _lastInvoiceDay = lastInvoiceNumber.Substring(pointer, 2);
                            pointer += 2;
                        }
                        if (variable == NumberingVariable.Month)
                        {
                            _lastInvoiceMonth = lastInvoiceNumber.Substring(pointer, 2);
                            pointer += 2;
                        }
                        if (variable == NumberingVariable.Year)
                        {
                            _lastInvoiceYear = lastInvoiceNumber.Substring(pointer, 4);
                            pointer += 4;
                        }
                        if (variable == NumberingVariable.Number)
                        {
                            pointer += 1;
                        }
                    }
                });
            }

            //ASSIGN ACTUAL POSSIBLE VARIABLES
            if (numberingTemplate.Contains(NumberingVariable.Day))
            {
                var day = date.Day.ToString();
                _actualInvoiceDay = day.Length == 1 ? "0" + day : day;
            }
            if (numberingTemplate.Contains(NumberingVariable.Month))
            {
                var month = date.Month.ToString();
                _actualInvoiceMonth = month.Length == 1 ? "0" + month : month;
            }
            if (numberingTemplate.Contains(NumberingVariable.Year))
            {
                _actualInvoiceYear = date.Year.ToString();
            }

            var numberVars = numberingTemplate.FindAll(var => var == NumberingVariable.Number);
            numberVars.ForEach(_ => {
                _replaceString += "*";
            });
        }
        public ResolveDTO GetNumbering()
        {
            PrepareInvoiceNumber();
            ValidateNumberLayout();
            var hasYears = _numberingTemplate.Contains(NumberingVariable.Year);
            var hasMonts = _numberingTemplate.Contains(NumberingVariable.Month);
            var hasDays = _numberingTemplate.Contains(NumberingVariable.Day);
            return ComputeNumberValue(hasYears, hasMonts, hasDays);
        }

        private void PrepareInvoiceNumber()
        {
            if(_invoiceNumber.Length == 0){
                _numberingTemplate.ForEach(varibale => {
                    if (varibale == NumberingVariable.Year)
                    {
                        _invoiceNumber += _actualInvoiceYear;
                    }
                    if (varibale == NumberingVariable.Month)
                    {
                        _invoiceNumber += _actualInvoiceMonth;
                    }
                    if (varibale == NumberingVariable.Day)
                    {
                        _invoiceNumber += _actualInvoiceDay;
                    }
                    if (varibale == NumberingVariable.Number)
                    {
                        _invoiceNumber += "*";
                    }
                });
            }
        }
        private void ValidateNumberLayout()
        {
            var charArray = _invoiceNumber.ToCharArray();
            bool locked = false;
            for (int charNumber = 0; charNumber < charArray.Length; charNumber++)
            {
                int? nextIndex = charNumber + 1 < charArray.Length ? charNumber + 1 : null;
                if (charArray[charNumber].ToString() == "*")
                {
                    if (locked) 
                    {
                        throw new ValidationError("Number variable can be only in one sequence in numbering template.");
                    }
                    if (nextIndex is not null && charArray[(int)nextIndex].ToString() != "*")
                    {
                        locked = true;
                        continue;
                    }
                }
            }
        }
        private ResolveDTO ComputeNumberValue(bool hasYears, bool hasMonths, bool hasDays)
        {
            int numberStartIndex = _invoiceNumber.IndexOf("*");
            int numberEndIndex = _replaceString.Length - 1;
            int lastNumber = _lastInvoiceNumber is null ? 0 : int.Parse(_lastInvoiceNumber.Substring(numberStartIndex,numberEndIndex-numberStartIndex));
            
            bool compareYears = _actualInvoiceYear == _lastInvoiceYear;
            bool compareMonths = _actualInvoiceMonth == _lastInvoiceMonth;
            bool compareDays = _actualInvoiceDay == _lastInvoiceDay;

            if (hasYears && hasMonths && hasDays) return ResolveNumberValue(lastNumber, compareYears && compareMonths && compareDays);
            if (hasYears && hasMonths && !hasDays) return ResolveNumberValue(lastNumber, compareYears && compareMonths);
            if (hasYears && !hasMonths && !hasDays) return ResolveNumberValue(lastNumber, compareYears);
            return new ResolveDTO { InvoiceNumber = null, Overflowed = false, Resolved = false };
        }
        private ResolveDTO ResolveNumberValue(int lastNumber, bool condition)
        {
            var zeroGenerator = new ZeroGeneratorHelper();
            var overflowHelper = new NumberOverflowHelper();
            bool isOverflowing = overflowHelper.IsOverflowingOnAdd(lastNumber);

            int numberValue = lastNumber + 1;
            string prefix = zeroGenerator.Generate(isOverflowing ? 0 : _replaceString.Length - numberValue.ToString().Length);
            if (_lastInvoiceNumber is null || condition)
            {
                _invoiceNumber = _invoiceNumber.Replace(_replaceString, prefix + numberValue);
                return new ResolveDTO{ InvoiceNumber = _invoiceNumber, Overflowed = isOverflowing, Resolved = true };
            }
            numberValue = 1;
            prefix = zeroGenerator.Generate(_replaceString.Length - 1);
            _invoiceNumber = _invoiceNumber.Replace(_replaceString, prefix + numberValue);
            return new ResolveDTO{ InvoiceNumber = _invoiceNumber, Overflowed = isOverflowing, Resolved = true };
        }
    }
}