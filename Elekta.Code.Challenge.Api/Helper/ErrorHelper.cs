using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Helper
{
    public static class ErrorHelper
    {
        public static ErrorDetails BuildError(string message )
        {
            var error = new ErrorDetails
            {
                StatusCode = 0,
                Message = message
            };

            return error;
        }
    }
}
