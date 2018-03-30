using System;
using System.Collections.Generic;
using System.Text;

namespace Trss.Infrastructure.Services
{
    public class YiFiResponse<T>
    {
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public T Data { get; set; }
    }
}
