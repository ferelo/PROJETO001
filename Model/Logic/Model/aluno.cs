using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Logic.Model
{
    [Table("aluno", Schema = "public")]
    public class aluno
    {
        [Key]
        [Column("id_aluno")]
        public int id_aluno { get; set; }

        [Column("id_cidade")]
        public int id_cidade { get; set; }

        [Column("nome")]
        public string nome { get; set; }

        [Column("cpf")]
        public string cpf { get; set; }

        [Column("rg")]
        public string rg { get; set; }

        [Column("sexo")]
        public int sexo { get; set; }

        [Column("data_nascimento")]
        public DateTime data_nascimento { get; set; }

        [Column("telefone")]
        public string telefone { get; set; }

        [Column("logradouro")]
        public string logradouro { get; set; }

        [Column("bairro")]
        public string bairro { get; set; }

        [Column("numero")]
        public string numero { get; set; }

        [Column("matricula")]
        public string matricula { get; set; }

        [Column("idade")]
        public int idade { get; set; }

        [Column("data_cadastro")]
        public DateTime data_cadastro { get; set; }

        [Column("status")]
        public int status { get; set; }
    }
}
