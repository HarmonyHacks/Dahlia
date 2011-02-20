namespace Dahlia.Models
{
    public class Registration : IAmPersistable
    {
        public virtual int Id { get; set; }
        public virtual Retreat Retreat { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual string BedCode { get; set; }
    }
}