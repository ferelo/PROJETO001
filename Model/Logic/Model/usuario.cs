using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Logic.Model
{
    [Table("usuario", Schema = "public")]
    public class usuario
    {
        [Key]
        [Column("id_usuario")]
        public int id_usuario { get; set; }

        [Column("nome")]
        public string nome { get; set; }

        [Column("cpf")]
        public string cpf { get; set; }

        [Column("sexo")]
        public int sexo { get; set; }

        [Column("telefone")]
        public string telefone { get; set; }

        [Column("cidade")]
        public string cidade { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("senha")]
        public string senha { get; set; }

        [Column("data_cadastro")]
        public DateTime data_cadastro { get; set; }

        [Column("status")]
        public int status { get; set; }
    }
}
