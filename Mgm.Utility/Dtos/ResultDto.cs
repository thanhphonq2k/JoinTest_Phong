namespace Mgm.Utility.Dtos
{
    public class ResultDto
    {
        public string Code { get; set; }
        public string Error { get; set; }
        public ResultDto()
        {
            Code = "200";
            Error = null;
        }
    }
}
