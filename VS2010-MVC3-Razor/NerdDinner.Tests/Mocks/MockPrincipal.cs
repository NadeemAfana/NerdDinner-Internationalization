using System.Security.Principal;

namespace NerdDinner.Tests.Mocks
{
    public class MockPrincipal : IPrincipal
    {
        IIdentity _identity;

        public IIdentity Identity
        {
            get
            {
                if (_identity == null)
                {
                    _identity = new MockIdentity();
                }
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}
