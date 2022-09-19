using Core.Persistence.Repositories;

namespace Kodlama.Io.Devs.Domain.Entities
{
    public class LanguageTechnology : Entity
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Language? Language { get; set; }
        public LanguageTechnology()
        {
        }

        public LanguageTechnology(int id,int languageId, string name) : this()
        {
            Id = id;
            LanguageId = languageId;
            Name = name;
        }
    }
}
