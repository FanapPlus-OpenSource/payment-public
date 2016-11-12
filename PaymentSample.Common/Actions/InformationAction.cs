using System.Collections.Generic;

namespace PaymentSample.Common.Actions
{
    public abstract class InformationAction : IAction
    {
        protected static readonly string SignKey = "[OBTAIN FROM PG.APPSON.IR]";

        protected static readonly string ProductCode = "[OBTAIN FROM PG.APPSON.IR]";

        protected static readonly string ProductItemCode = "[OBTAIN FROM PG.APPSON.IR]";

        protected virtual string BaseAddress => "https://pg.appson.ir";
        public abstract List<string> Act(string command);
        public abstract List<string> HelpInformation { get; }
    }
}