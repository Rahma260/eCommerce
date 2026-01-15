using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs.Responses
{
    //record optimized for immutable data.
    //use this for simple responses from services (add, update, delete) which does not return data and only manipulate data.
    public record ServiceResponse(bool success = false, string message = null);
}
