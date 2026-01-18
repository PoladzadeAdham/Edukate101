using System.ComponentModel.DataAnnotations;

namespace Edukate101.ViewModel.UserViewModel
{
    public class LoginVm
    {
        [Required, MaxLength(255), MinLength(2), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(255), MinLength(2), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
