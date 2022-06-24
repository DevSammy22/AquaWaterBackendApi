using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class Response<T> 
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
    }
}
