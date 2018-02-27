using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Jurassic.WebFrame;

namespace Jurassic.WebApi
{
    public partial class Startup : IStartupConfig
    {
        public void Config(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
