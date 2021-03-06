﻿using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using System.Collections.Generic;
using System.Net;

namespace SdcaFramework.ClientSteps
{
    class CollectorSteps : BaseSteps<Collector, CollectorCreator>
    {
        protected override string Resource { get; } = "collector";

        public int LastCollectorId => LastObjectId;

        public List<Collector> GetListOfCollectors(HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => GetListOfObjects(expectedStatusCode);

        public void CreateCollector(CollectorCreator collector, HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
            => CreateNewObject(collector, expectedStatusCode);

        public void ModifyCollector(Collector collector, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            => ModifyExistingObject(collector, expectedStatusCode);

        public Collector GetCollectorById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            => GetObjectById(id, expectedStatusCode);

        public void DeleteCllectorById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => DeleteObjectById(id, expectedStatusCode);

        public HttpStatusCode GetStatusCodeGetCollectorByIdAction(int id)
            => GetHttpStatusCodeForGetByIdAction(id);

        public HttpStatusCode GetStatusCodeDeleteCollectorAction(int id)
            => GetHttpStatusCodeForDeleteAction(id);

        public HttpStatusCode GetStatusCodeCreateCollectorAction(CollectorCreator collector)
            => GetHttpStatusCodeForPostAction(collector);

        public HttpStatusCode GetStatusCodeForInvalidPostAction(Dictionary<string, object> parameters)
            => GetHttpStatusCodeForInvalidPostAction(parameters);
    }
}