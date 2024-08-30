using System.Net;

namespace DailyDynamo.Shared.Common
{
    public class APIResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public APIResponseModel(ServiceResult<T> serviceResult)
        {
            if(serviceResult.Exception != null)
            {
                this.Message = "Internal Server Error";
                this.Data = default;
                this.StatusCode = HttpStatusCode.InternalServerError;
            }

            this.Message = serviceResult.Message;
            this.Data = serviceResult.Data;

            if (serviceResult.IsSuccess)
            {
                this.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                this.StatusCode = HttpStatusCode.BadRequest;
            }
        }

        public APIResponseModel(string message)
        {
            this.Message = message;
            StatusCode = HttpStatusCode.BadRequest;
        }

      
    }
}
