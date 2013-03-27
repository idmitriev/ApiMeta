using System;

namespace ApiMeta.Common.Membership
{
    public interface IMembershipValidator
    {
        Boolean ValidateUser(String username, String password);
    }
}
