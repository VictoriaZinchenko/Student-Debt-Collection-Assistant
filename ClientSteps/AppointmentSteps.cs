﻿using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using System.Collections.Generic;
using System.Net;

namespace SdcaFramework.ClientSteps
{
    class AppointmentSteps : BaseSteps<Appointment, AppointmentCreator>
    {

        protected override string Resource { get; } = "/api/appointment";

        public int LastAppointmentId => LastObjectId;

        public List<Appointment> GetListOfAppointments(HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => GetListOfObjects(expectedStatusCode);

        public void CreateAppointment(AppointmentCreator appointment, HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
            => CreateNewObject(appointment, expectedStatusCode);

        //public void ModifyAppointment(Appointment appointment, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        //    => ModifyExistingObject(appointment, expectedStatusCode);

        public Appointment GetAppointmentById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            => GetObjectById(id, expectedStatusCode);

        public void DeleteAppointmentById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => DeleteObjectById(id, expectedStatusCode);

        public HttpStatusCode GetResponseGetAppointmentByIdAction(int id)
            => GetStatusCodeForGetByIdAction(id);

        public HttpStatusCode GetResponseDeleteAppointmentAction(int id)
            => GetStatusCodeForDeleteAction(id);
    }
}