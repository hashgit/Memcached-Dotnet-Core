using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FxManager.Services.Http
{
    [DataContract]
    public class FixerRate
    {
        [DataMember]
        public long Timestamp { get; set; }

        [DataMember]
        public IDictionary<string, decimal> Rates { get; set; }
    }
}