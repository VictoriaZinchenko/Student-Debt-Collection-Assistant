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

        public HttpStatusCode GetStatusCodeGetStudentByIdAction(int id)
            => GetHttpStatusCodeForGetByIdAction(id);

        public HttpStatusCode GetStatusCodeDeleteStudentAction(int id)
            => GetHttpStatusCodeForDeleteAction(id);

        public HttpStatusCode GetStatusCodeCreateStudentAction(StudentCreator student)
            => GetHttpStatusCodeForPostAction(student);

        public HttpStatusCode GetStatusCodeForInvalidPostAction(Dictionary<string, object> parameters)
            => GetHttpStatusCodeForInvalidPostAction(parameters);
    }
}
