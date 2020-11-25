using System;
using System.Collections.Generic;
using System.Text;

namespace sales.Model
{
   public class Response
    {

        public bool IsSuccess { get; set; }

        public String Message { get; set; }

        public object Result { get; set; }

    }
}
