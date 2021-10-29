namespace TMC.Models
{
    public class ajaxResponse
    {
        public ResponseStatus respstatus { get; set; }
        public object data { get; set; }
        public string respmessage { get; set; }
    }
    public enum ResponseStatus
    {
        success = 0,
        error = 1
    }
}
