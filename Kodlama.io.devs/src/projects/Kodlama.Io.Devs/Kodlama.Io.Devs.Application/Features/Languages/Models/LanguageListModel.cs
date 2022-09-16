using Core.Persistence.Paging;

namespace Kodlama.Io.Devs.Application.Features.Languages.Models
{
    public class LanguageListModel : BasePageableModel
    {
        public IList<LanguageListModel> Items { get; set; }
    }
}
