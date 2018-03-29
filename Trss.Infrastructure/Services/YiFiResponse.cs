using System;
using System.Collections.Generic;
using System.Text;

namespace Trss.Infrastructure.Services
{
    public class YiFiResponse
    {
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public YiFiReleasesList Data { get; set; }
    }
}
