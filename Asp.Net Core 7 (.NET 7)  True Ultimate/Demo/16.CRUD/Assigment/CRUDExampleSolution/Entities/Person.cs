using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)] // nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set;}
        // unique
        public Guid? CountryID { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        [StringLength(200)]
        public string? Address { get; set;}
        public bool ReciveNewsLetters { get; set; }

    }
}
