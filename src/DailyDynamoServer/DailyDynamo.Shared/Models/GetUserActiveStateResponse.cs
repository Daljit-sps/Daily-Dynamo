using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Shared.Models
{
    public class GetUserActiveStateResponse
    {
        public bool isSuccess {  get; set; }

        public bool isUserActive { get; set; } 
    }
}
