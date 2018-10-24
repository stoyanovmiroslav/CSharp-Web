using SIS.Framework.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Api
{
    public interface IMvcApplication
    {
        void Configure();

        void ConfigureService(IServiceCollection serviceCollection);
    }
}
