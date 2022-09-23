using Core.Security.Entities;

namespace Kodlama.Io.Devs.Domain.Entities
{
    public class Developer : User
    {
        public string GitHub { get; set; }

        public Developer()
        {
        }

        public Developer(string gitHub) : this()
        {
            GitHub = gitHub;
        }
    }
}
