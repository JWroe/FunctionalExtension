namespace FunctionalExtension.Option
{
    public class None
    {
        private None()
        {
        }

        internal static None Default => new None();
    }

    public class Some<T>
    {
        public T Value { get; }

        internal Some(T value) => Value = value;
    }
}