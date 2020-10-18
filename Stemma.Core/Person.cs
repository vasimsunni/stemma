using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemma.Core
{
    public class Person
    {
        private long id, surnameIDF, fatherPersonIDF, motherPersonIDF, continentIDF, countryIDF, stateIDF;
        private string identityUserIDF,firstName, lastName, gender, email, mobileNumber, alternativeNumber, emergencyNumber, city, address, createdBy, updatedBy;
        private DateTime? dOB, dOD, createdOn, updatedOn;
        private bool isActive, isDeleted;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get => id; set => id = value; }
        public string IdentityUserIDF { get => identityUserIDF; set => identityUserIDF = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public long SurnameIDF { get => surnameIDF; set => surnameIDF = value; }
        public long FatherPersonIDF { get => fatherPersonIDF; set => fatherPersonIDF = value; }
        public long MotherPersonIDF { get => motherPersonIDF; set => motherPersonIDF = value; }
        public DateTime? DOB { get => dOB; set => dOB = value; }
        public DateTime? DOD { get => dOD; set => dOD = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Email { get => email; set => email = value; }
        public string MobileNumber { get => mobileNumber; set => mobileNumber = value; }
        public string AlternativeNumber { get => alternativeNumber; set => alternativeNumber = value; }
        public string EmergencyNumber { get => emergencyNumber; set => emergencyNumber = value; }
        public long ContinentIDF { get => continentIDF; set => continentIDF = value; }
        public long CountryIDF { get => countryIDF; set => countryIDF = value; }
        public long StateIDF { get => stateIDF; set => stateIDF = value; }
        public string City { get => city; set => city = value; }
        public string Address { get => address; set => address = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime? CreatedOn { get => createdOn; set => createdOn = value; }
        public string UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime? UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}
