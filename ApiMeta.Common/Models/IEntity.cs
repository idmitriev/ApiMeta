namespace ApiMeta.Common.Models
{
    public interface IEntity<TIdentifier> 
    {
        TIdentifier Id { get; set; }
    }
}
