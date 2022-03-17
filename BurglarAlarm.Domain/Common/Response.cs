
namespace BurglarAlarm.Domain.Common
{
    public class Response
    {
        public object Data { get; set; }

        public string Error { get; set; }

        public bool IsSuccess { get; set; }
    }
}
