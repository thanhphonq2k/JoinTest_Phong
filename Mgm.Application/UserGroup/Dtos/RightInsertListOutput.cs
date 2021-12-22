using System;

namespace Mgm.UserGroup.Dtos
{
    public class RightInsertListOutput
    {
        public int UserGroupId { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public int ViewRight { get; set; }
        public int UpdateRight { get; set; }
    }
}

