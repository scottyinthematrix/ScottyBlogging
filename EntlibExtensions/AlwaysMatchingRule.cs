using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace ScottyApps.Utilities.EntlibExtensions
{
    public class AlwaysMatchingRule : IMatchingRule
    {
        public AlwaysMatchingRule()
        {
        }

        public AlwaysMatchingRule(NameValueCollection configuration)
        {
        }

        public bool Matches(MethodBase member)
        {
            return true;
        }
    }
}
