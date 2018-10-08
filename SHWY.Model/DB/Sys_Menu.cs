using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Model.DB
{
    [Serializable]
    [Table("Sys_Menu")]
    public class Sys_Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public string MenuUrl { get; set; }
        public int MenuSortID { get; set; }

        [NotMapped]
        public List<Sys_Menu> children { get; set; }
    }
}
