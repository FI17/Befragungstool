using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain;

namespace Domain.Acces
{
    public class DatabaseContent : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<SurveyQuestionLink> SurveyQuestionLinks { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Answering> Answerings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .HasOptional(f => f.question)
                .WithMany(s => s.Answers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SurveyQuestionLink>()
                .HasOptional(d => d.survey)
                .WithMany(k => k.SurveyQuestionLinks)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SurveyQuestionLink>()
                .HasOptional(d => d.question)
                .WithMany(f => f.SurveyQuestionLinks)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answering>()
                    .HasOptional(d => d.surveyQuestionLink)
                    .WithMany(f => f.Answerings)
                    .WillCascadeOnDelete(false);
        }
    }
}
