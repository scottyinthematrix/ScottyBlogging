
using System.Runtime.Serialization;
namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Comment : Entry
    {
        public Entry TargetEntry { get; set; }
    }
}
