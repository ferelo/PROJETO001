using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Logic.Model
{
    [Table("pais", Schema = "public")]
    public class pais
    {
        [Key]
        [Column("id_pais")]
        public int id_pais { get; set; }

        [Column("id_aluno")]
        public int id_aluno { get; set; }

        [Column("nome")]
        public string nome { get; set; }

        [Column("cpf")]
        public string cpf { get; set; }

        [Column("rg")]
        public string rg { get; set; }

        [Column("profissao")]
        public string profissao { get; set; }

        [Column("celular")]
        public string celular { get; set; }

        [Column("data_cadastro")]
        public DateTime data_cadastro { get; set; }

        [Column("status")]
        public int status { get; set; }
    }
}
