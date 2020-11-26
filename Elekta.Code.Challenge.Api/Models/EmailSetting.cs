using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Models
{
    public class EmailSetting
    {
        public string SMTP_Server { get; set; }

        public int SMTP_Port { get; set; }

        public string NoReply_Email { get; set; }

        public string NoReply_Email_Pwd { get; set; }

        public string Environment { get; set; } 

    }
}
