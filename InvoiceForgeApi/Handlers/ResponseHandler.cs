using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Interfaces.Handlers;
using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.Handlers
{
    public class ResponseHandler<T>: IResponseHandler<T>
    {
        private T? _Data;
        private List<ResponseErrorDTO> _ErrorMap = new();
        public T Data
        {
            get => _Data;
            set => _Data = value;
        }
        public List<ResponseErrorDTO> ErrorMap
        {
            get => _ErrorMap; 
            set => _ErrorMap = value;
        }
        public ResponseHandler(HttpResponse Response, RequestHandler<T?> RequestHandler)
        {
            //Assign data
            _Data = RequestHandler.Data;
            //Convert exceptions and add to ErrorMap
            RequestHandler.Exceptions.ForEach((exception) =>
                {
                    _ErrorMap.Add(new ResponseErrorDTO(
                        exception.Code ?? ErrorCodes.U_E, 
                        exception.Message, 
                        exception.Source
                    ));
                }
            );
            //Set status code of response
            if (RequestHandler.Exceptions.Count != 0){
                Response.StatusCode = 500;
            }
        }
    }
}
