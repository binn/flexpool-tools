using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexSettings
{

    [Serializable]
    public class SettingsPushException : Exception
    {
        public SettingsPushException() : base("unable to push settings to Flexpool. check your configuration and try again") { }

    }
}
