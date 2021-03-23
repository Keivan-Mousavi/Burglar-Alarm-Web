using System;

namespace BurglarAlarm.Web.Models
{
    public class ControllerModel
    {
        public DateTime Id
        {
            get
            {
                return DateTime.Now;
            }
            set
            {

            }
        }

        public string Serial { get; set; }

        public string SendNEC { get; set; }
    }
}
