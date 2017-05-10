using System;
using System.Net;

namespace BabyBus.Logic.Shared
{

    class BabyBusWebServiceException : Exception
    {
       

        public BabyBusWebServiceException(HttpStatusCode statusCode, string statusDescription, string errorMessage)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ErrorMessage = errorMessage;
        }

        public BabyBusWebServiceException(HttpStatusCode statusCode, string statusDescription, string errorMessage, Exception errorException)
            : base(errorMessage, errorException)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ErrorMessage = errorMessage;
        }

        public HttpStatusCode StatusCode
        {
            get;
            set;
        }

        public string StatusDescription
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public bool IsAny400()
        {
            return (int)StatusCode >= 400 && (int)StatusCode < 500;
        }

        public bool IsAny500()
        {
            return (int)StatusCode >= 500 && (int)StatusCode < 600;
        }

    }

}
