namespace Core.DbEntity;

public interface IDbEntity;

public interface IDbEntity<TId>
{
    TId Id { get; init; }
}