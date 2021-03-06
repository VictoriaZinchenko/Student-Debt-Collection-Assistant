﻿using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using System.Collections.Generic;
using System.Net;

namespace SdcaFramework.ClientSteps
{
    class DebtSteps : BaseSteps<Debt, DebtCreator>
    {
        protected override string Resource { get; } = "debt";

        public int LastDebtId => LastObjectId;

        public List<Debt> GetListOfDebts(HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => GetListOfObjects(expectedStatusCode);

        public void CreateDebt(DebtCreator debt, HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
            => CreateNewObject(debt, expectedStatusCode);

        public Debt GetDebtById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            => GetObjectById(id, expectedStatusCode);

        public void DeleteDebtById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => DeleteObjectById(id, expectedStatusCode);

        public HttpStatusCode GetStatusCodeGetDebtByIdAction(int id)
            => GetHttpStatusCodeForGetByIdAction(id);

        public HttpStatusCode GetStatusCodeDeleteDebtAction(int id)
            => GetHttpStatusCodeForDeleteAction(id);

        public HttpStatusCode GetStatusCodeCreateDebtAction(DebtCreator debt)
            => GetHttpStatusCodeForPostAction(debt);

        public HttpStatusCode GetStatusCodeForInvalidPostAction(Dictionary<string, object> parameters)
            => GetHttpStatusCodeForInvalidPostAction(parameters);
    }
}
