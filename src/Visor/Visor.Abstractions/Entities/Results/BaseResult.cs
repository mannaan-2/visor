using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visor.Abstractions.Entities.Results
{
    public class BaseResult
    {
        public IList<Error> Errors { get; set; }
        public bool Succeeded { get; set; }
    }
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
