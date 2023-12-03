using System.Collections;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Models.Entities
{
    public class ShoppingList
    {
        [Key]
        public int ShoppingListId { get; set; }

        [Key]
        public string UserId { get; set; }

        public virtual ICollection<ShoppingListProduct> ShoppingListProducts { get; set; }

        public virtual User User { get; set; }
    }

}
