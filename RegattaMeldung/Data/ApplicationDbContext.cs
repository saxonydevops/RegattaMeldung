using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Models;

namespace RegattaMeldung.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ReportedStartboatMember>()
               .HasKey(t => new { t.ReportedStartboatId, t.MemberId });

            builder.Entity<ReportedStartboatMember>()
                .HasOne(sm => sm.ReportedStartboat)
                .WithMany(s => s.ReportedStartboatMembers)
                .HasForeignKey(sm => sm.ReportedStartboatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReportedStartboatMember>()
                .HasOne(sm => sm.Member)
                .WithMany(m => m.ReportedStartboatMembers)
                .HasForeignKey(sm => sm.MemberId);

            builder.Entity<ReportedStartboatStandby>()
               .HasKey(t => new { t.ReportedStartboatId, t.MemberId });

            builder.Entity<ReportedStartboatStandby>()
                .HasOne(sm => sm.ReportedStartboat)
                .WithMany(s => s.ReportedStartboatStandbys)
                .HasForeignKey(sm => sm.ReportedStartboatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReportedStartboatStandby>()
                .HasOne(sm => sm.Member)
                .WithMany(m => m.ReportedStartboatStandbys)
                .HasForeignKey(sm => sm.MemberId);

            builder.Entity<ReportedStartboat>()
                .HasOne(r => r.Regatta)
                .WithMany(rs => rs.ReportedStartboats)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaStartingFee>()
                .HasKey(t => new { t.StartingFeeId, t.RegattaId });

            builder.Entity<RegattaStartingFee>()
                .HasOne(r => r.Regattas)
                .WithMany(rs => rs.RegattaStartingFees)
                .HasForeignKey(rm => rm.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaStartingFee>()
                .HasOne(s => s.StartingFees)
                .WithMany(sr => sr.RegattaStartingFees)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCampingFee>()
                .HasKey(t => new { t.CampingFeeId, t.RegattaId });

            builder.Entity<RegattaCampingFee>()
                .HasOne(r => r.Regatta)
                .WithMany(rs => rs.RegattaCampingFees)
                .HasForeignKey(rm => rm.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCampingFee>()
                .HasOne(c => c.CampingFee)
                .WithMany(cr => cr.RegattaCampingFees)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaOldclass>()
                .HasKey(t => new { t.OldclassId, t.RegattaId });

            builder.Entity<RegattaOldclass>()
                .HasOne(r => r.Regattas)
                .WithMany(ro => ro.RegattaOldclasses)
                .HasForeignKey(rr => rr.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaOldclass>()
                .HasOne(o => o.Oldclasses)
                .WithMany(ro => ro.RegattaOldclasses)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCompetition>()
                .HasKey(t => new { t.CompetitionId, t.RegattaId });

            builder.Entity<RegattaCompetition>()
                .HasOne(r => r.Regattas)
                .WithMany(c => c.RegattaCompetitions)
                .HasForeignKey(rr => rr.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCompetition>()
                .HasOne(b => b.Competitions)
                .WithMany(c => c.RegattaCompetitions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaClub>()
                .HasKey(t => new { t.ClubId, t.RegattaId });

            builder.Entity<RegattaClub>()
                .HasOne(r => r.Regatta)
                .WithMany(c => c.RegattaClubs)
                .HasForeignKey(rr => rr.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaClub>()
                .HasOne(b => b.Club)
                .WithMany(c => c.RegattaClubs)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<RegattaMeldung.Models.Boatclass> Boatclasses { get; set; }
        public DbSet<RegattaMeldung.Models.CampingFee> CampingFees { get; set; }
        public DbSet<RegattaMeldung.Models.Club> Clubs { get; set; }
        public DbSet<RegattaMeldung.Models.Competition> Competitions { get; set; }
        public DbSet<RegattaMeldung.Models.Member> Members { get; set; }
        public DbSet<RegattaMeldung.Models.Oldclass> Oldclasses { get; set; }
        public DbSet<RegattaMeldung.Models.Raceclass> Raceclasses { get; set; }
        public DbSet<RegattaMeldung.Models.Regatta> Regattas { get; set; }
        public DbSet<RegattaMeldung.Models.RegattaCampingFee> RegattaCampingFees { get; set; }
        public DbSet<RegattaMeldung.Models.RegattaClub> RegattaClubs { get; set; }
        public DbSet<RegattaMeldung.Models.RegattaCompetition> RegattaCompetitions { get; set; }
        public DbSet<RegattaMeldung.Models.RegattaOldclass> RegattaOldclasses { get; set; }
        public DbSet<RegattaMeldung.Models.RegattaStartingFee> RegattaStartingFees { get; set; }
        public DbSet<RegattaMeldung.Models.ReportedStartboat> ReportedStartboats { get; set; }
        public DbSet<RegattaMeldung.Models.ReportedStartboatMember> ReportedStartboatMembers { get; set; }
        public DbSet<RegattaMeldung.Models.ReportedStartboatStandby> ReportedStartboatStandbys { get; set; }
        public DbSet<RegattaMeldung.Models.ReportedRace> ReportedRaces { get; set; }
        public DbSet<RegattaMeldung.Models.StartingFee> StartingFees { get; set; }
        public DbSet<RegattaMeldung.Models.Water> Waters { get; set; }        
    }
}
