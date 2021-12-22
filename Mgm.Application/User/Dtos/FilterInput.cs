using Abp.Application.Services.Dto;

namespace Mgm.User.Dtos
{
    public class FilterInput : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
