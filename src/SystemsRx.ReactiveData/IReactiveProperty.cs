namespace SystemsRx.ReactiveData
{
    public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        new T Value { get; set; }
    }
}