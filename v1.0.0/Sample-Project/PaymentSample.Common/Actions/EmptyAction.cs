using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace PaymentSample.Common.Actions
{
    [Export("empty", typeof(IAction))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EmptyAction : IAction
    {
        public List<string> Act(string command)
        {
            return new List<string>
            {
                "ERROR",
                $"Input: {command}. Command is not valid."
            };
        }

        public List<string> HelpInformation => new List<string>() {"quit\tquit the application"};
    }
}