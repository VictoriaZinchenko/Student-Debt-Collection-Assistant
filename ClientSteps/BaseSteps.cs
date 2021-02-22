using Newtonsoft.Json;
using NLog;
using RestSharp;
using SdcaFramework.Utilities;
using SdcaFramework.Utilities.Configuration;
using System;
using System.Collections.Generic;
using System.Net;

namespace SdcaFramework.ClientSteps
{
    class BaseSteps<T, K>
    {
        protected Logger Logger = LogManager.GetCurrentClassLogger();
        private static string BaseUrl = new Startup().Configuration["BaseUrl"];
        private readonly RestClient RestClient = new RestClient(BaseUrl);

        protected virtual string Resource { get; }

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

        protected HttpStatusCode GetHttpStatusCodeForGetByIdAction(int id)
            => ExecuteRequest(Method.GET, 0, null, id.ToString()).StatusCode;

        protected HttpStatusCode GetHttpStatusCodeForDeleteAction(int id)
            => ExecuteRequest(Method.DELETE, 0, null, id.ToString()).StatusCode;

        protected HttpStatusCode GetHttpStatusCodeForPostAction(K objectData)
           => ExecuteRequest(Method.POST, 0, objectData).StatusCode;

        protected HttpStatusCode GetHttpStatusCodeForInvalidPostAction(Dictionary<string, object> parameters)
            => ExecuteRequest(Method.POST, 0, UtilityMethods.DictionaryToObject(parameters)).StatusCode;

        private IRestResponse ExecuteRequest(Method method, HttpStatusCode expectedStatusCode, object body = null, string objectId = "")
        {
            var resourceUrl = $"{Resource}/{objectId}";
            RestRequest restRequest = new RestRequest(resourceUrl, method);
            restRequest.RequestFormat = DataFormat.Json;
            Logger.Debug($"\nTrying send a request for url {BaseUrl}{resourceUrl}." +
                $"\nMethod: {method}");
            if (body != null)
            {
                restRequest.AddJsonBody(body);
                Logger.Debug($"\nBody: {PropertiesDescriber.GetObjectProperties(body)}");
            }
            IRestResponse response = RestClient.Execute(restRequest);
            if (expectedStatusCode != 0)
            {
                CheckHttpStatusCode(response, expectedStatusCode);
            }
            Logger.Debug($"\nResponse status code: {response.StatusCode}");
            return response;
        }

        private void CheckHttpStatusCode(IRestResponse response, HttpStatusCode expectedStatusCode)
        {
            var actualStatusCode = response.StatusCode;
            if (actualStatusCode != expectedStatusCode)
            {
                Logger.Debug($"Error message: {response.ErrorMessage}");
                Logger.Error("An incorrect status code. Throw an exception");
                throw new Exception("The response has an incorrect status code" +
                    $"\nThe expected status code: {expectedStatusCode}" +
                    $"\nThe actual status code: {actualStatusCode}");
            }
        }

        private List<T> GetDeserializedResponseForList(IRestResponse response)
        {
            string content = response.Content;
            CheckContent(content);
            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        private T GetDeserializedResponseForSingleObject(IRestResponse response)
        {
            string content = response.Content;
            CheckContent(content);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private void CheckContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new Exception("Response content is empty");
            }
            Logger.Debug($"\nContent: {content}");
        }
    }
}