using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Languages.Dtos;

namespace Kodlama.Io.Devs.Application.Features.Languages.Models
{
    public class LanguageListModel : BasePageableModel
    {
        public IList<ListLanguageDto> Items { get; set; }
    }
}
