using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquaWater.Domain.Entities;

namespace AquaWater.Data.Services.Interfaces
{
    public interface ITokenGenerator
    {
        public  Task<string> GenerateTokenAsync(User user);
     }
}
