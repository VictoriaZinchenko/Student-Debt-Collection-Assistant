using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace SdcaFramework.ClientSteps
{
    class BaseSteps<T, K>
    {
        private static RestClient RestClient = new RestClient("http://localhost:81/");

        protected virtual string Resource { get;}

        protected int LastObjectId => GetListOfObjects(HttpStatusCode.OK).Count - 1;

        protected List<T> GetListOfObjects(HttpStatusCode expectedStatusCode) 
            => GetDeserializedResponseForList(ExecuteRequest(Method.GET, expectedStatusCode));

        protected void CreateNewObject(K objectData, HttpStatusCode expectedStatusCode)
                => ExecuteRequest(Method.POST, expectedStatusCode, objectData);

        protected void ModifyExistingObject(T objectData, HttpStatusCode expectedStatusCode) 
            => ExecuteRequest(Method.PUT, expectedStatusCode, objectData);

        protected void DeleteObjectById(int id, HttpStatusCode expectedStatusCode)
            => ExecuteRequest(Method.DELETE, expectedStatusCode, null, id.ToString());

        protected T GetObjectById(int id, HttpStatusCode expectedStatusCode)
            => GetDeserializedResponseForSingleObject(ExecuteRequest(Method.GET, expectedStatusCode, null, id.ToString()));

        protected HttpStatusCode GetStatusCodeForGetByIdAction(int id)
            => ExecuteRequest(Method.GET, 0, null, id.ToString()).StatusCode;

        protected HttpStatusCode GetStatusCodeForDeleteAction(int id)
            => ExecuteRequest(Method.DELETE, 0, null, id.ToString()).StatusCode;

        private IRestResponse ExecuteRequest(Method method, HttpStatusCode expectedStatusCode, object body = null, string objectId = "")
        {
            RestRequest restRequest = new RestRequest($"{Resource}/{objectId}", method);
            restRequest.RequestFormat = DataFormat.Json;
            if(body != null)
            {
                restRequest.AddBody(body);
            }
            IRestResponse response =  RestClient.Execute(restRequest);
            if (expectedStatusCode != 0)
            {
                CheckHttpStatusCode(response, expectedStatusCode);
            }

            return response;
        }

        private void CheckHttpStatusCode(IRestResponse response, HttpStatusCode expectedStatusCode)
        {
            var actualStatusCode = response.StatusCode;
            if (actualStatusCode != expectedStatusCode)
            {
                throw new Exception("The response has an incorrect status code" +
                    $"\nThe expected status code: {expectedStatusCode}" +
                    $"\nThe actual status code: {actualStatusCode}");
            }
        }

        private List<T> GetDeserializedResponseForList(IRestResponse response)
        {
            var content = response.Content;
            if (content == string.Empty || content == null)
            {
                throw new Exception("Response content is empty");
            }
            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        private T GetDeserializedResponseForSingleObject(IRestResponse response)
        {
            var content = response.Content;
            if (content == string.Empty || content == null)
            {
                throw new Exception("Response content is empty");
            }
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
