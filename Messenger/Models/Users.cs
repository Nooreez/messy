using System.ComponentModel.DataAnnotations;

namespace Messenger.Models
{
    public class Users
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string username { get; set; }
    }
}
