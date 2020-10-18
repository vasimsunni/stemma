using System;

namespace Stemma.Services.DTOs.Response
{
    public class Person
    {
        public long Id { get; set; }
        public string IdentityUserIDF { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SurnameIDF { get; set; }
        public long FatherPersonIDF { get; set; }
        public long MotherPersonIDF { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOD { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string AlternativeNumber { get; set; }
        public string EmergencyNumber { get; set; }
        public long ContinentIDF { get; set; }
        public long CountryIDF { get; set; }
        public long StateIDF { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Surname { get; set; }
        public Person Father { get; set; }
        public Person Mother { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string MaritalStatus { get; set; }
        public int Age { get; set; }
        public string ProfilePictureURL { get; set; }
    }
}
