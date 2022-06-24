using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.TransactionDTO
{
    public class RequestDTO
    {
        public Guid OrderId {get; set;}
        public string Userid {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;} 
        public string Email {get; set;}


    }
}
