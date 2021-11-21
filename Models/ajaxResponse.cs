namespace TMC.Models
{
    public class ajaxResponse
    {
        public ResponseStatus respstatus { get; set; }
        public object data { get; set; }
        public string respmessage { get; set; }

        public ajaxResponse()
        {
            this.data = null;
            this.respmessage = "Something went wrong, please try again later.";
            this.respstatus = ResponseStatus.error;
        }
    }
    public enum ResponseStatus
    {
        success = 0,
        error = 1
    }
}
