using System;
using System.ComponentModel.DataAnnotations;

namespace Stemma.Services.DTOs.Request
{
    public class Person
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please select Surname")]
        public long SurnameIDF { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please select Father")]
        public long FatherPersonIDF { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please select Mother")]
        public long MotherPersonIDF { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime? DOB { get; set; }
        public DateTime? DOD { get; set; }
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string AlternativeNumber { get; set; }
        public string EmergencyNumber { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please select Continent")]
        public long ContinentIDF { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please select Country")]
        public long CountryIDF { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please select State")]
        public long StateIDF { get; set; }
        [Required(ErrorMessage = "Please select City")]
        public string City { get; set; }
        public string Address { get; set; }
        public long PictureUploadId { get; set; }
    }
}
