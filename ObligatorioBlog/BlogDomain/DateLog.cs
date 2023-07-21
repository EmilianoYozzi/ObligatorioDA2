using BlogDomain.DomainEnums;
using BlogDomain.DomainInterfaces;

namespace BlogDomain
{
    public class DateLog : IComparable
    {
        public string Id { get; set; }
        public readonly DateTime creationDate;
        public List<DateTime> ModificationDates { get; }
        public DateTime LastModification
        {
            get => ModificationDates.Count > 0 ? ModificationDates.Last() : creationDate;            
        }

        public DateLog()
        {
            Id = Guid.NewGuid().ToString();
            creationDate = DateTime.Now;
            ModificationDates = new List<DateTime>();
        }

        public void RegisterModification(DateTime date)
        {
            ModificationDates.Add(date);
        }

        public override bool Equals(object? obj) => Equals(obj as DateLog);

        private bool Equals(DateLog other) =>
            this.Id.Equals(other.Id);

        public int CompareTo(object? obj)
            => CompareTo(obj as DateLog);

        private int CompareTo(DateLog? other)
            => LastModification.CompareTo(other?.LastModification);
        
    }
}