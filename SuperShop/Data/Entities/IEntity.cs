namespace SuperShop.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; }

         // bool WasDeleted { get; set; } // Para se apagar, mas manter em BD. Para isso basta alterar para 'True'

    }
}
