using System.ComponentModel.DataAnnotations;

namespace WebAPI_dapper.Data.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
    }
}
