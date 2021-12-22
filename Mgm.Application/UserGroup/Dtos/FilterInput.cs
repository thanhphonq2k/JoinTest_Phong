using Abp.Application.Services.Dto;

namespace Mgm.UserGroup.Dtos
{
    public class FilterInput : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
