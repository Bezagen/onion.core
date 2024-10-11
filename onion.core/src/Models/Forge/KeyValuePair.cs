using System.ComponentModel.DataAnnotations;

namespace onion.core.src.Models.Forge
{
    public class KeyValuePair
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "There is no key name")]
        public string Key { get; set; }

        public string Value { get; set; }

        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public KeyValuePair() { 
        }
    }
}
