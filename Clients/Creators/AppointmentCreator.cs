using System;
using System.Collections.Generic;

namespace SdcaFramework.Clients.Creators
{
    public class AppointmentCreator
    {
        public int debtId { get; set; }

        public List<int> collectorIds { get; set; }

        public DateTime appointmentDate { get; set; }
    }
}
