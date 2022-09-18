using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Kodlama.Io.Devs.Domain.Entities
{
    public class GitHub : Entity
    {
        public string Name { get; set; }
        public int UserId { get; set; }        
        public User? User { get; set; }

        public GitHub()
        {
        }

        public GitHub(int id,int userId, string name) : this()
        {
            Id = id;
            UserId = userId;
            Name = name;
        }
    }
}
