using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace PaymentSample.Common.Actions
{
    [Export("help", typeof(IAction))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class HelpAction : IAction
    {
        public List<string> Act(string command)
        {
            return AppsOnContainer.Instance.ResolveAll<IAction>()
                .SelectMany(p => p.HelpInformation)
                .ToList();
        }

        public List<string> HelpInformation => new List<string> {"help\tfor available commands"};
    }
}