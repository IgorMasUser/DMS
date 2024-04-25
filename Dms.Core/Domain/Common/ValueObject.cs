namespace Dms.Core.Domain.Common
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if ((left == null) ^ (right == null))
            {
                return false;
            }

            return left?.Equals(right) ?? true;
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();


        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            ValueObject valueObject = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return (from x in GetEqualityComponents()
                    select x?.GetHashCode() ?? 0).Aggregate((int x, int y) => x ^ y);
        }
    }
}
