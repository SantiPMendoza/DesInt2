﻿using System;
using System.Net;

namespace ExamenRecu2Ev_SantiPuebla.Services
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
