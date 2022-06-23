using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFF
{
    /// <inheritdoc />
    public class Config : IConfig
    {

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        public bool broadcast { get; set; } = true;
    }
}
