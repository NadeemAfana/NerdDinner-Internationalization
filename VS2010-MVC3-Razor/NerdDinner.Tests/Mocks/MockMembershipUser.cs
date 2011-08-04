using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace NerdDinner.Tests.Mocks
{
    public class MockMembershipUser : MembershipUser
    {
        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            return newPassword.Equals("newPass");
        }
    }
}
