using System.Collections.Generic;

namespace FxManager.Services.Http
{
    public class FixerRate
    {
        public long Timestamp { get; set; }
        public IDictionary<string, decimal> Rates { get; set; }
    }
}