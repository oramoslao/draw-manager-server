using DrawManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawManager.Database.SqlServer
{
    public class DrawManagerSqlServerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Draw> Draws { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<Entrant> Entrants { get; set; }
        public DbSet<DrawEntry> DrawEntries { get; set; }
        public DbSet<PrizeSelectionStep> PrizeSelectionSteps { get; set; }

        public DrawManagerSqlServerDbContext(DbContextOptions<DrawManagerSqlServerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BuildModels(modelBuilder);
        }

        public IEnumerable<DrawEntry> GetEntriesByDrawExcludingPreviousWinnersAndLosers(int drawId)
        {
            return _getEntriesByDrawExcludingPreviousWinnersAndLosers(this, drawId);
        }

        private static readonly Func<DrawManagerSqlServerDbContext, int, IEnumerable<DrawEntry>> _getEntriesByDrawExcludingPreviousWinnersAndLosers = EF.CompileQuery((DrawManagerSqlServerDbContext context, int drawId) => context
                .DrawEntries
                .AsNoTracking()
                .Where(de => de.DrawId == drawId)
                .Select(de => new DrawEntry { Id = de.Id, EntrantId = de.EntrantId })
                );

        private static void BuildModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prize>(entity =>
            {
                entity
                    .HasOne(p => p.Draw)
                    .WithMany(d => d.Prizes)
                    .HasForeignKey(p => p.DrawId);

                entity
                    .HasMany(p => p.SelectionSteps)
                    .WithOne(pss => pss.Prize)
                    .HasForeignKey(pss => pss.PrizeId);

            });

            modelBuilder.Entity<Draw>(entity =>
            {
                entity
                    .HasMany(d => d.DrawEntries)
                    .WithOne(de => de.Draw)
                    .HasForeignKey(de => de.DrawId);

                entity
                    .HasMany(d => d.Prizes)
                    .WithOne(p => p.Draw)
                    .HasForeignKey(p => p.DrawId);

            });


            modelBuilder.Entity<Entrant>(entity =>
            {
                entity
                    .HasMany(e => e.DrawEntries)
                    .WithOne(de => de.Entrant)
                    .HasForeignKey(de => de.EntrantId);

                entity
                    .HasMany(e => e.SelectionSteps)
                    .WithOne(pss => pss.Entrant)
                    .HasForeignKey(pss => pss.EntrantId);
            });

            modelBuilder.Entity<DrawEntry>(entity =>
            {
                entity
                    .HasOne(de => de.Entrant)
                    .WithMany(e => e.DrawEntries)
                    .HasForeignKey(de => de.EntrantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(de => de.Draw)
                    .WithMany(d => d.DrawEntries)
                    .HasForeignKey(de => de.DrawId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasMany(de => de.SelectionSteps)
                    .WithOne(pss => pss.DrawEntry)
                    .HasForeignKey(pss => pss.DrawEntryId);

            });

            modelBuilder.Entity<PrizeSelectionStep>(prizeSelectionStep =>
            {
                prizeSelectionStep
                    .HasOne(pss => pss.Prize)
                    .WithMany(p => p.SelectionSteps)
                    .HasForeignKey(pss => pss.PrizeId)
                    .OnDelete(DeleteBehavior.Restrict);

                prizeSelectionStep
                    .HasOne(pss => pss.Entrant)
                    .WithMany(e => e.SelectionSteps)
                    .HasForeignKey(pss => pss.EntrantId)
                    .OnDelete(DeleteBehavior.Restrict);

                prizeSelectionStep
                   .HasOne(pss => pss.DrawEntry)
                   .WithMany(de => de.SelectionSteps)
                   .HasForeignKey(pss => pss.DrawEntryId)
                   .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
