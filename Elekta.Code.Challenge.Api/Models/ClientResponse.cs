using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Models
{
    public class ClientResponse
    {
        public string  Message { get; set; }
        public List<ErrorDetails> Errors { get; set; }
    }
}
