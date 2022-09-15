namespace Core.Persistence.Repositories;

public class Entity
{
    public int Id { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Entity()
    {
    }

    public Entity(int id) : this()
    {
        Id = id;
        CreateDate = DateTime.Now.Date;
    }
}