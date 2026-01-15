using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.ExternalServices.Interfaces.Jobs
{
    public interface IBackgroundJobService
    {
        void EnqueueEmail(string to, string subject, string body);
    }
}
