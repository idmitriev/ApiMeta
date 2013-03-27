using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiMeta.Common.Membership
{
    public class FakeMembershipValidator : IMembershipValidator
    {
        public bool ValidateUser(string username, string password)
        {
            return true;
        }
    }
}
