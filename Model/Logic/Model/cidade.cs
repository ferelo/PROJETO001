using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Logic.Model
{
    [Table("cidade", Schema = "public")]
    public class cidade
    {
        [Key]
        [Column("id_cidade")]
        public int id_cidade { get; set; }

        [Column("nome")]
        public string nome { get; set; }

        [Column("cep")]
        public string cep { get; set; }

        [Column("estado")]
        public int estado { get; set; }
      
    }
}
