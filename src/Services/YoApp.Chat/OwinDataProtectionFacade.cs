using System;
using Microsoft.AspNetCore.DataProtection;
using IDataProtector = Microsoft.Owin.Security.DataProtection.IDataProtector;

namespace YoApp.Chat
{
    public class OwinDataProtectionFacade : Microsoft.Owin.Security.DataProtection.IDataProtectionProvider
    {
        private readonly IDataProtectionProvider _dataProtector;

        public OwinDataProtectionFacade(IDataProtectionProvider dataProtector)
        {
            _dataProtector = dataProtector;
        }

        public IDataProtector Create(params string[] purposes)
        {
            var proc = _dataProtector.CreateProtector(purposes);
            return new OwinDataProtectorFacade(proc);
        }

        public static OwinDataProtectionFacade CreateFromServiceProvider(IServiceProvider provider)
        {
            var dataProtectionProvider = provider.GetDataProtectionProvider();
            return new OwinDataProtectionFacade(dataProtectionProvider);
        }

        public class OwinDataProtectorFacade : IDataProtector
        {
            private readonly Microsoft.AspNetCore.DataProtection.IDataProtector _dataProtector;

            public OwinDataProtectorFacade(Microsoft.AspNetCore.DataProtection.IDataProtector dataProtector)
            {
                _dataProtector = dataProtector;
            }

            public byte[] Protect(byte[] userData) => _dataProtector.Protect(userData);

            public byte[] Unprotect(byte[] protectedData) => _dataProtector.Unprotect(protectedData);
        }
    }
}
