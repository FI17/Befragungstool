using System.Data.Entity;

namespace Domain.Acces
{
    public class DatabaseContent : DbContext
    {
        public DatabaseContent() : base("Umfrage-tool")
        {

        }


        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Answering> Answerings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasOptional(d => d.survey)
                .WithMany(k => k.questions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answer>()
                .HasOptional(f => f.question)
                .WithMany(s => s.answers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Session>()
                .HasOptional(d => d.survey)
                .WithMany(f => f.sessions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answering>()
                    .HasOptional(d => d.session)
                    .WithMany(f => f.answerings)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answering>()
                    .HasOptional(d => d.question)
                    .WithMany(f => f.answerings)
                    .WillCascadeOnDelete(false);
        }
    }
}



