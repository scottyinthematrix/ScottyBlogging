
using System.Runtime.Serialization;
namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Gossip : Entry
    {
    }
}
