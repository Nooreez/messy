using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.Models
{
    public class Messages
    {
        public int id { get; set; }
        [Required]
        public string text { get; set; }
        [Required]
        public int from { get; set; }
        [Required]
        public int to { get; set; }
        public DateTimeOffset date { get; set; }
    }
}
