using System.Security.Principal;

namespace NerdDinner.Tests.Mocks
{
    public class MockIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get
            {
                return "MockAuthentication";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return "someUser";
            }
        }
    }
}
