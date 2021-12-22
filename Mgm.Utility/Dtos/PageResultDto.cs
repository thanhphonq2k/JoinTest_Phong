using System.Collections.Generic;

namespace Mgm.Utility.Dtos
{
    public class PageResultDto<T>
    {
        public List<T> items { get; set; }

        public int totalCount { get; set; }
    }
}
