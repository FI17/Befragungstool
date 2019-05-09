using System.Data.Entity;
using Microsoft.Win32.SafeHandles;

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

        public DbSet<Choice> Choices { get; set; }

        public DbSet<GivenAnswer> GivenAnswers { get; set; }

        public DbSet<Chapter> Chapters { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasOptional(d => d.survey)
                .WithMany(k => k.questions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Choice>()
                .HasOptional(f => f.question)
                .WithMany(s => s.choice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Session>()
                .HasOptional(d => d.survey)
                .WithMany(f => f.sessions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GivenAnswer>()
                    .HasOptional(d => d.session)
                    .WithMany(f => f.givenAnswer)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<GivenAnswer>()
                    .HasOptional(d => d.question)
                    .WithMany(f => f.givenAnswer)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chapter>()
                .HasOptional(g => g.survey)
                .WithMany(h=>h.chapters)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Question>()
                .HasOptional(z=> z.chapter)
                .WithMany(q=> q.questions)
                .WillCascadeOnDelete(false);
        }
    }
}



