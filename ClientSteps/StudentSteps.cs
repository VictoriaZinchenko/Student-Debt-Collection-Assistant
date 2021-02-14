using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using System.Collections.Generic;
using System.Net;

namespace SdcaFramework.ClientSteps
{
    class StudentSteps : BaseSteps<Student, StudentCreator>
    {
        protected override string Resource { get; } = "student";

        public int LastStudentId => LastObjectId;

        public List<Student> GetListOfStudents(HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => GetListOfObjects(expectedStatusCode);

        public void CreateStudent(StudentCreator student, HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
            => CreateNewObject(student, expectedStatusCode);

        public void ModifyStudent(Student student, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            => ModifyExistingObject(student, expectedStatusCode);

        public Student GetStudentById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            => GetObjectById(id, expectedStatusCode);

        public void DeleteStudentById(int id, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) => DeleteObjectById(id, expectedStatusCode);

        public HttpStatusCode GetResponseGetStudentByIdAction(int id)
            => GetStatusCodeForGetByIdAction(id);

        public HttpStatusCode GetResponseDeleteStudentAction(int id)
            => GetStatusCodeForDeleteAction(id);

        public HttpStatusCode GetResponseCreateStudentAction(StudentCreator student)
            => GetStatusCodeForPostAction(student);

        public HttpStatusCode GetStatusCodeForInvalidPostAction(Dictionary<string, object> parameters)
            => GetHttpStatusCodeForInvalidPostAction(parameters);
    }
}
