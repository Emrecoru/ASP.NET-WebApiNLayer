using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Services
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }

        public List<string>? ErrorMessages { get; set; }

        [JsonIgnore]
        public bool IsSuccess => ErrorMessages == null || ErrorMessages.Count == 0;

        [JsonIgnore]
        public bool IsFail => !IsSuccess;

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonIgnore]
        public string? UrlAsCreated { get; set; }


        //static factory method
        public static ServiceResult<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ServiceResult<T> { Data = data, StatusCode = statusCode };
        }

        public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated)
        {
            return new ServiceResult<T> { Data = data, StatusCode = HttpStatusCode.Created, UrlAsCreated = urlAsCreated };
        }

        public static ServiceResult<T> Fail(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T> { ErrorMessages = errorMessages, StatusCode = statusCode };
        }

        public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T> { ErrorMessages = [errorMessage] , StatusCode = statusCode };
        }
    }

    public class ServiceResult
    {
        public List<string>? ErrorMessages { get; set; }

        [JsonIgnore]
        public bool IsSuccess => ErrorMessages == null || ErrorMessages.Count == 0;

        [JsonIgnore]
        public bool IsFail => !IsSuccess;

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }


        //static factory method
        public static ServiceResult Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ServiceResult { StatusCode = statusCode };
        }

        public static ServiceResult Fail(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult { ErrorMessages = errorMessages, StatusCode = statusCode };
        }

        public static ServiceResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ServiceResult { ErrorMessages = [errorMessage], StatusCode = statusCode };
        }
    }
}
