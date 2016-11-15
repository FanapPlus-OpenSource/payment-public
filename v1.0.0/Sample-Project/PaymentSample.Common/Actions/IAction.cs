using System.Collections.Generic;

namespace PaymentSample.Common.Actions
{
    public interface IAction
    {
        List<string> Act(string command);
        List<string> HelpInformation { get; }
    }
}