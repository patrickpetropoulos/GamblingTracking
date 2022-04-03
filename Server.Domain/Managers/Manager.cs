using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Domain.Managers
{
    public abstract class Manager : IManager
    {
        private IDisposable ConfigChangeCallback;

        protected readonly IConfiguration Configuration;
        protected readonly ILoggerFactory LoggerFactory;

        private ILogger _Logger;
        protected ILogger _log
        {
            get
            {
                return _Logger;
            }
        }

        public Manager()
        {

        }
        public Manager(IConfiguration configuration)
        {
            if (configuration != null)
            {
                Configuration = configuration;
                var ConfigChangeToken = Configuration.GetReloadToken();
                ConfigChangeCallback = ConfigChangeToken.RegisterChangeCallback((o) => { InitializeConfiguration(); }, null);
                InitializeConfiguration();
            }
        }

        public virtual void SetLogger(ILoggerFactory loggerFactory, string name)
        {
            _Logger = loggerFactory.CreateLogger(name);
        }

        protected abstract void InitializeConfiguration();

        public virtual void Dispose()
        {
            if (ConfigChangeCallback != null)
            {
                ConfigChangeCallback.Dispose();
                ConfigChangeCallback = null;
            }
        }
    }
}
