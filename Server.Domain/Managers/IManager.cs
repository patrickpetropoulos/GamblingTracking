using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Domain.Managers
{
    public interface IManager : IDisposable
    {
        void SetLogger(ILoggerFactory loggerFactory, string name);
    }
}
