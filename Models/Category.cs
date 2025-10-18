using Restaurant.Api.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public ICollection<MenuItem>? MenuItems { get; set; }
    }
}
