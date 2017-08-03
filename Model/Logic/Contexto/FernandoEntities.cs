using Model.Logic.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Model.Logic.Contexto
{
    public class FernandoEntities : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //remove a pluralização
            modelBuilder = MudarLocalizacao(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #region Configuração do Entities

        public FernandoEntities(string connectionString)
            : base(connectionString)
        {
        }

        public FernandoEntities()
            : base("name=FernandoEntities")
        {
        }

        #endregion

        #region Idenficação de Todos os DbSet


        public DbSet<usuario> usuario { get; set; }
        public DbSet<aluno> aluno { get; set; }

        public DbSet<cidade> cidade { get; set; }
        public DbSet<pais> pais { get; set; }
        #endregion

        #region Utilitários para o Contexto
        public DbModelBuilder MudarLocalizacao(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<usuario>().ToTable("usuario", "public");
            modelBuilder.Entity<aluno>().ToTable("aluno", "public");
            modelBuilder.Entity<cidade>().ToTable("cidade", "public");
            modelBuilder.Entity<pais>().ToTable("pais", "public");

            return modelBuilder;
        }

        #endregion

    }
}