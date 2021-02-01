using System.Collections.Generic;

namespace SdcaFramework.Clients.Creators
{
    public class AppointmentCreator
    {
        public int debtId { get; set; }
        public List<int> collectorIds { get; set; }
        public string appointmentDate { get; set; }
    }
}
