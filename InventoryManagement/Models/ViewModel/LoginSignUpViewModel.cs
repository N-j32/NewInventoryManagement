using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models.ViewModel
{
    public class LoginSignUpViewModel
    {

       
        [Required(ErrorMessage ="* Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "* Password is required")]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool IsRemember { get; set; }
    }
}