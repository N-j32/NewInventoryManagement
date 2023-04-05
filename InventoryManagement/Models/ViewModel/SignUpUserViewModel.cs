using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class SignUpUserViewModel
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "* User Name Required")]
        [Remote(action: "UserNameIsExist", controller:"Account")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
       
        [Required(ErrorMessage = "* Email Required")]
        public string Email { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "*Mobile Number is  Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",ErrorMessage ="Mobile number is not valid")]
        public long? Mobile { get; set; }
        
        [Required(ErrorMessage = "* Password Required")]
       
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "* Confirm the Password ")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Active")]
        public bool IsActive { get; set; }
       
    }
}
