using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    class NotificationList
    {
        public List<Notifications> Notifications { get; set; }
    }
    public class Notifications
    {
        public DateTime SendDate { get; set; }
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string SendTo { get; set; }
    }
}
