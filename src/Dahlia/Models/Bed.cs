namespace Dahlia.Models
{
    public class Bed : IAmPersistable
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual bool IsUpstairs { get; set; }
    }
}