namespace Trinkit
{
    public class Object : IEquatable<Object>
    {
        public Guid UUID { get; set; }

        public Object()
        {
            if (UUID == Guid.Empty)
                UUID = Guid.NewGuid();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Object? other)
        {
            if (other == null) return false;

            return UUID == other.UUID;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Object)) return false;

            return Equals((Object)obj);
        }
    }
}
