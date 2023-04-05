using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class EmployeeRegister
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "* Employee Name Required")]
        [MaxLength (15,ErrorMessage ="Maximum length is 15")]
        [MinLength(3, ErrorMessage = "Minimum length must be 3")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "* First Name Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* Last Name Required")]
        public string LastName { get; set; }



        [Required(ErrorMessage = "* Email required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "* Password required")]
        [MaxLength(15, ErrorMessage = "* Maximum length is 15")]
        [MinLength(3, ErrorMessage = "* Minimum length must be 3")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "* Address Required")]
        public string Address { get; set; }

       

        [Required(ErrorMessage = "* Number required")]
        public long Number { get; set; }

        public bool IsActive { get; set; }

        public bool IsRemember { get; set; }

    }
}
