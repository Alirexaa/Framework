namespace Framework.Abstraction.Types;

public interface IHaveIdentity<out TId>
{
    TId Id { get; }
}

public interface IHaveIdentity : IHaveIdentity<long> { }
