namespace Kodlama.Io.Devs.Application.Features.Languages.Dtos
{
    public class DeletedLanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> LanguageTechnologies { get; set; }
    }
}
