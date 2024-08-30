using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Shared.Common
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => Exception == null && !HasValidationError;
        public bool HasValidationError { get; set; }

        /// <summary>
        /// Service Result
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="hasValidationError"></param>
        public ServiceResult(T data, string message, bool hasValidationError = false)
        {
            Data = data;
            Message = message;
            Exception = null;
            HasValidationError = hasValidationError;
        }

        /// <summary>
        /// Service Result
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public ServiceResult(Exception exception, string message)
        {
            Exception = exception;
            Message = message;
            Data = default;
        }

    }
}
