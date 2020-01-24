using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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


            builder.Entity<Club>().HasData(new Club { ClubId = 1, City = "Ansbach", Name = "Kanu-Sportclub Ansbach", VNr = "02-001", ShortName = "Ansbach" });
            builder.Entity<Club>().HasData(new Club { ClubId = 2, City = "Aschaffenburg", Name = "SSKC Poseidon Aschaffenburg Kanuabt.", VNr = "02-002", ShortName = "Aschaffenburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 11, City = "Berlin", Name = "Kanu-Gemeinschaft Tegel e. V.", VNr = "03-063", ShortName = "KG Tegel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 101, City = "Göttingen", Name = "Vereinigung für Kanurennsport Nord (VKN)", VNr = "09-252", ShortName = "VKN Göttingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 102, City = "Göttingen", Name = "Turn- und Wassersportverein Göttingen von 1861 e.V.", VNr = "09-030", ShortName = "TuW Göttingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 103, City = "Göttingen", Name = "Göttinger Paddler-Club e.V.", VNr = "09-028", ShortName = "Göttinger PC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 104, City = "Güsen", Name = "Güsener Handball-Club e.V.", VNr = "16-045", ShortName = "Güsener HC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 105, City = "Güstrow", Name = "Kanu-Sportverein Güstrow e.V.", VNr = "08-022", ShortName = "KSV Güstrow" });
            builder.Entity<Club>().HasData(new Club { ClubId = 106, City = "Haldensleben", Name = "WSV Haldensleben e.V. Abt. Kanu", VNr = "16-007", ShortName = "WSV Haldensleben" });
            builder.Entity<Club>().HasData(new Club { ClubId = 107, City = "Halle", Name = "Kanuverein 96 Halle e.V.", VNr = "16-023", ShortName = "KV 96 Halle" });
            builder.Entity<Club>().HasData(new Club { ClubId = 108, City = "Halle", Name = "Hallescher Kanu-Club 54 e.V.", VNr = "16-026", ShortName = "Hallescher KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 109, City = "Hamburg", Name = "Verein für Leibesübungen von 1893 e.V., Sparte Wassersport", VNr = "06-020", ShortName = "VfL Hamburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 110, City = "Hamburg", Name = "Hanseat Verein für Wassersport e.V.", VNr = "06-012", ShortName = "Hanseat VfW" });
            builder.Entity<Club>().HasData(new Club { ClubId = 12, City = "Berlin", Name = "Kanusport-Verein Neu Ahlbeck e. V.", VNr = "03-060", ShortName = "KV Neu Ahlbeck" });
            builder.Entity<Club>().HasData(new Club { ClubId = 111, City = "Hamburg", Name = "Bergedorfer Kanu Club v. 1953 e.V.", VNr = "06-005", ShortName = "Bergedorfer KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 112, City = "Hamburg", Name = "Alster Canoe Club e.V.", VNr = "06-002", ShortName = "Alster CC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 113, City = "Hamburg", Name = "Freier Wassersportverein Vorwärts e.V.", VNr = "06-009", ShortName = "FWV Vorwärts" });
            builder.Entity<Club>().HasData(new Club { ClubId = 114, City = "Hamburg", Name = "Sportvereinigung Polizei von 1920 e.V., Wassersportabteilung", VNr = "06-018", ShortName = "SVG Polizei HH" });
            builder.Entity<Club>().HasData(new Club { ClubId = 115, City = "Hamburg", Name = "Harburger Kanu-Club von 1922 e.V.", VNr = "06-013", ShortName = "Harburger KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 116, City = "Hamburg", Name = "Oberalster Verein für Wassersport e.V.", VNr = "06-016", ShortName = "Oberalster VfW" });
            builder.Entity<Club>().HasData(new Club { ClubId = 117, City = "Hamburg", Name = "Hamburger Kanu-Club e.V.", VNr = "06-010", ShortName = "Hamburger KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 118, City = "Hamburg", Name = "Alstereck Verein für Wassersport e.V.", VNr = "06-003", ShortName = "Alstereck VfW" });
            builder.Entity<Club>().HasData(new Club { ClubId = 119, City = "Hameln", Name = "Kanu-Club Hameln e.V.", VNr = "09-034", ShortName = "KC Hameln" });
            builder.Entity<Club>().HasData(new Club { ClubId = 120, City = "Hamm", Name = "DJK Wassersport Hamm e.V.", VNr = "10-137", ShortName = "DJK Hamm" });
            builder.Entity<Club>().HasData(new Club { ClubId = 13, City = "Berlin", Name = "Köpenicker Sport-Club e. V.", VNr = "03-043", ShortName = "Köpenicker SC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 121, City = "Hamm", Name = "Kanu-Verein 45 Herringen e.V.", VNr = "10-154", ShortName = "KV Herringen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 122, City = "Hamm", Name = "Kanu-Ring e.V. Hamm", VNr = "10-140", ShortName = "KR Hamm" });
            builder.Entity<Club>().HasData(new Club { ClubId = 123, City = "Hamm", Name = "Kanu-Verein Hamm e.V.", VNr = "10-141", ShortName = "KV Hamm" });
            builder.Entity<Club>().HasData(new Club { ClubId = 124, City = "Hann. Münden", Name = "Mündener Kanu-Club e.V.", VNr = "09-048", ShortName = "Mündener KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 125, City = "Hannover", Name = "Hannoverscher Kanu-Club v. 1921 e.V.", VNr = "09-036", ShortName = "Hannoverscher KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 126, City = "Hannover", Name = "Kanu-Club Limmer e.V.", VNr = "09-037", ShortName = "KC Limmer" });
            builder.Entity<Club>().HasData(new Club { ClubId = 127, City = "Hannover", Name = "Sport Club Hannover e.V. -Kanu-", VNr = "09-238", ShortName = "SC Hannover" });
            builder.Entity<Club>().HasData(new Club { ClubId = 128, City = "Havelberg", Name = "Havelberger WSV e.V.", VNr = "16-009", ShortName = "Havelberger WSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 129, City = "Heidelberg", Name = "Wassersportclub 1931 Heidelberg-Neuenheim e.V.", VNr = "01-013", ShortName = "WSV Heidelberg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 130, City = "Heilbronn", Name = "TSG Heilbronn", VNr = "01-112", ShortName = "TSG Heilbronn" });
            builder.Entity<Club>().HasData(new Club { ClubId = 14, City = "Berlin", Name = "Berliner Kanu Club Rotation e. V.", VNr = "03-049", ShortName = "KC Rotation" });
            builder.Entity<Club>().HasData(new Club { ClubId = 131, City = "Heilbronn", Name = "Sportverein Union 08 Böckingen e.V. Kanu-Abtl.", VNr = "01-103", ShortName = "SV Böckingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 132, City = "Herdecke", Name = "Herdecker Kanu-Club 1925 e.V.", VNr = "10-150", ShortName = "Herdecker KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 133, City = "Herford", Name = "Herforder Kanu-Klub e.V.", VNr = "10-151", ShortName = "Herforder KK" });
            builder.Entity<Club>().HasData(new Club { ClubId = 134, City = "Hof", Name = "Kanu Rennsport Vereinigung Hof e.V.", VNr = "02-151", ShortName = "KRV Hof" });
            builder.Entity<Club>().HasData(new Club { ClubId = 135, City = "Hof", Name = "Schwimmverein Hof 1911 e.V. Kanuabt.", VNr = "02-037", ShortName = "SV Hof" });
            builder.Entity<Club>().HasData(new Club { ClubId = 136, City = "Hof", Name = "Faltboot-Club Hof 1932 e.V.", VNr = "02-035", ShortName = "FC Hof" });
            builder.Entity<Club>().HasData(new Club { ClubId = 137, City = "Jena", Name = "USV Jena e.V. Abt. Kanu", VNr = "18-013", ShortName = "USV Jena" });
            builder.Entity<Club>().HasData(new Club { ClubId = 138, City = "Jeßnitz", Name = "Kanu-Club Jeßnitz/Anhalt e.V.", VNr = "16-036", ShortName = "KC Jeßnitz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 139, City = "Kaiserslautern", Name = "1. Ski- und Kanu-Club Kaiserslautern e.V.", VNr = "11-004", ShortName = "KC Kaiserslautern" });
            builder.Entity<Club>().HasData(new Club { ClubId = 140, City = "Kaiserslautern", Name = "Paddlergilde Kaiserslautern 1926 e.V.", VNr = "11-005", ShortName = "PG Kaiserslautern" });
            builder.Entity<Club>().HasData(new Club { ClubId = 15, City = "Berlin", Name = "Blau-Gelb-Köpenick e.V.", VNr = "03-065", ShortName = "BG Köpenick" });
            builder.Entity<Club>().HasData(new Club { ClubId = 141, City = "Karlsruhe", Name = "Rheinbrüder Karlsruhe e.V. Kanu-Segel-Skiclub", VNr = "01-019", ShortName = "RB Karlsruhe" });
            builder.Entity<Club>().HasData(new Club { ClubId = 142, City = "Kassel", Name = "Kanu-Sport Kassel e.V.", VNr = "07-038", ShortName = "KS Kassel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 143, City = "Kassel", Name = "PSV Grün-Weiß Kassel", VNr = "07-041", ShortName = "PSV Kassel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 144, City = "Kassel", Name = "WVC Cassel", VNr = "07-079", ShortName = "WVC Cassel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 145, City = "Kelheim", Name = "Kanu-Club Kelheim e.V.", VNr = "02-041", ShortName = "KC Kelheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 146, City = "Kiel", Name = "TUS Gaarden e.V. - Kanuabt.", VNr = "17-016", ShortName = "TUS Gaarden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 147, City = "Kiel", Name = "TSV Klausdorf von 1916 e.V. - Kanuabt.", VNr = "17-017", ShortName = "TSV Klausdorf" });
            builder.Entity<Club>().HasData(new Club { ClubId = 148, City = "Kiel", Name = "Kieler Kanu-Klub von 1921 e.V.", VNr = "17-015", ShortName = "Kieler KK" });
            builder.Entity<Club>().HasData(new Club { ClubId = 149, City = "Kiel", Name = "Ellerbeker Turnvereinigung von 1886 e.V., Kanuabtl.", VNr = "17-006", ShortName = "Ellerbeker TV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 150, City = "Kirchmöser", Name = "Eisenbahnersportverein Kirchmöser e.V. Kanuabt.", VNr = "04-024", ShortName = "ESV Kirchmöser" });
            builder.Entity<Club>().HasData(new Club { ClubId = 16, City = "Berlin", Name = "Ruder- und Kanu-Verein 1928 e.V.", VNr = "03-020", ShortName = "RKV Berlin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 151, City = "Kleinheubach", Name = "Wasser-Sport-Gemeinschaft Kleinheubach", VNr = "02-043", ShortName = "WSG Kleinheubach" });
            builder.Entity<Club>().HasData(new Club { ClubId = 152, City = "Koblenz", Name = "Wassersportverein Koblenz-Metternich e.V.", VNr = "13-009", ShortName = "WSV Koblenz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 153, City = "Köln", Name = "Kanusport Köln-Mülheim e.V.", VNr = "10-177", ShortName = "KS Köln" });
            builder.Entity<Club>().HasData(new Club { ClubId = 154, City = "Konstanz", Name = "Kanu-Club Konstanz 1932 e.V.", VNr = "01-027", ShortName = "KC Konstanz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 155, City = "Krefeld", Name = "SC Bayer 05 Uerdingen e.V., Kanuabteilung", VNr = "10-527", ShortName = "SCB05 Uerdingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 156, City = "Krefeld", Name = "Krefelder Kanu-Klub 1927 e.V.", VNr = "10-189", ShortName = "Krefelder KK" });
            builder.Entity<Club>().HasData(new Club { ClubId = 157, City = "Lampertheim", Name = "Kanuakademie Lampertheim", VNr = "07-126", ShortName = "KA Lampertheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 158, City = "Lampertheim", Name = "KC 1952 Lampertheim", VNr = "07-046", ShortName = "KC Lampertheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 159, City = "Lampertheim", Name = "Kajak-Team Hessen Lampertheim e.V.", VNr = "07-113", ShortName = "KTH Lampertheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 160, City = "Lampertheim", Name = "Kanu-Club Altrhein am Leistungszentrum Mannheim e.V.", VNr = "01-077", ShortName = "KC Altrhein Mannheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 17, City = "Berlin", Name = "Wassersportclub Blau-Weiß Tegel e.V.", VNr = "03-029", ShortName = "BW Tegel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 161, City = "Lampertheim", Name = "Wassersportverein 2002 am Leistungszentrum Mannheim e.V.", VNr = "01-075", ShortName = "WSV Mannheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 162, City = "Lampertheim", Name = "WSV Lampertheim", VNr = "07-047", ShortName = "WSV Lampertheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 163, City = "Langenprozelten", Name = "Paddel-Sport-Verein Langenprozelten", VNr = "02-115", ShortName = "PSV Langenprozelten" });
            builder.Entity<Club>().HasData(new Club { ClubId = 164, City = "Leipzig", Name = "Universitätssportclub Leipzig e. V.", VNr = "15-052", ShortName = "USC Leipzig" });
            builder.Entity<Club>().HasData(new Club { ClubId = 165, City = "Leipzig", Name = "Sportclub DHfK Leipzig e. V. - Abtl. Kanu -", VNr = "15-008", ShortName = "DHfK Leipzig" });
            builder.Entity<Club>().HasData(new Club { ClubId = 166, City = "Leipzig", Name = "Sportgemeinschaft Leipziger Verkehrsbetriebe e.V. Kanuabt.", VNr = "15-046", ShortName = "SG LVB" });
            builder.Entity<Club>().HasData(new Club { ClubId = 167, City = "Lilienthal", Name = "Turn- u. Sportverein Lilienthal v. 1862 e.V., Kanu-Abt.", VNr = "09-134", ShortName = "TuS Lilienthal" });
            builder.Entity<Club>().HasData(new Club { ClubId = 168, City = "Limburg", Name = "Kanuclub Limburg im ESV Blau Weiß", VNr = "07-049", ShortName = "KC Limburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 169, City = "Löcknitz", Name = "SV Einheit Löcknitz", VNr = "08-006", ShortName = "SV Löcknitz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 170, City = "Lohr", Name = "TSV 1846 Lohr e.V. Kanuabt.", VNr = "02-050", ShortName = "TSV Lohr" });
            builder.Entity<Club>().HasData(new Club { ClubId = 18, City = "Berlin", Name = "Kajak-Club Nord-West 1925 e.V.", VNr = "03-006", ShortName = "KC Nord-West" });
            builder.Entity<Club>().HasData(new Club { ClubId = 171, City = "Lübeck", Name = "Lübecker Kanu- und Segelsport-Verein e.V.", VNr = "17-019", ShortName = "Lübecker KSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 172, City = "Lübeck", Name = "Lübecker Motor-Yacht-Club e.V., Kanuabtl.", VNr = "17-056", ShortName = "Lübecker MYC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 173, City = "Lübeck", Name = "Kanu-Club Lübeck e.V.", VNr = "17-057", ShortName = "KC Lübeck" });
            builder.Entity<Club>().HasData(new Club { ClubId = 174, City = "Lünen", Name = "Kanu- u. Ski-Club Lünen e.V. 1949", VNr = "10-408", ShortName = "KSC Lünen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 175, City = "Magdeburg", Name = "Wassersportverein Lok Magdeburg", VNr = "16-061", ShortName = "WSV Lok Magdeburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 176, City = "Magdeburg", Name = "Kanuteam Sachsen-Anhalt e.V.", VNr = "16-059", ShortName = "KT Sachsen-Anhalt" });
            builder.Entity<Club>().HasData(new Club { ClubId = 177, City = "Magdeburg", Name = "Sportclub Magdeburg e.V.", VNr = "16-034", ShortName = "SC Magdeburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 178, City = "Magdeburg", Name = "Wassersportverein Buckau-Fermersleben e.V., Kanuabtl.", VNr = "16-010", ShortName = "WSV Buckau" });
            builder.Entity<Club>().HasData(new Club { ClubId = 179, City = "Magdeburg", Name = "Kanu - Klub Börde Magdeburg e.V.", VNr = "16-033", ShortName = "KK Börde" });
            builder.Entity<Club>().HasData(new Club { ClubId = 180, City = "Mainz", Name = "Kanufreunde 1929 e.V. Mainz-Mombach", VNr = "12-009", ShortName = "KF Mainz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 19, City = "Berlin", Name = "Paddelclub Gut Naß Tegel 1924 e.V.", VNr = "03-014", ShortName = "Gut Naß Tegel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 181, City = "Mainz", Name = "Kanu- u. Ski-Gesellschaft 1921 Mainz-Mombach", VNr = "12-005", ShortName = "KSG Mainz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 182, City = "Malchin", Name = "Malchiner Kanu-Club e.V.", VNr = "08-027", ShortName = "Malchiner KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 183, City = "Mannheim", Name = "Kanu-Gesellschaft Mannheim-Neckarau", VNr = "01-030", ShortName = "KG Mannheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 184, City = "Mannheim", Name = "Paddel-Gesellschaft Mannheim e.V.", VNr = "01-033", ShortName = "PG Mannheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 185, City = "Mannheim", Name = "WSV Mannheim-Sandhofen e.V.", VNr = "01-035", ShortName = "WSV Mannheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 186, City = "Markranstädt", Name = "Kanu- und Freizeitclub Markranstädt e. V.", VNr = "15-021", ShortName = "KF Makranstädt" });
            builder.Entity<Club>().HasData(new Club { ClubId = 187, City = "Marl", Name = "TSV Marl-Hüls 1912 e.V.,Kanuabteilung", VNr = "10-208", ShortName = "TSV Marl-Hüls" });
            builder.Entity<Club>().HasData(new Club { ClubId = 188, City = "Merzig", Name = "Kanu-Club Merzig e.V.", VNr = "14-004", ShortName = "KC Merzig" });
            builder.Entity<Club>().HasData(new Club { ClubId = 189, City = "Mettlach", Name = "Kanu-Freunde Mettlach", VNr = "14-005", ShortName = "KF Mettlach" });
            builder.Entity<Club>().HasData(new Club { ClubId = 190, City = "Mittweida", Name = "Sächsischer Kanusportverein Mittweida e. V.", VNr = "15-012", EMail = "SKSV_Mittweida@web.de", ShortName = "SKSV Mittweida" });
            builder.Entity<Club>().HasData(new Club { ClubId = 20, City = "Berlin", Name = "Turngemeinde in Berlin 1848 e.V., Kanuabteilung Oberspree", VNr = "03-023_2", ShortName = "Turngemeinde" });
            builder.Entity<Club>().HasData(new Club { ClubId = 191, City = "Mittweida", Name = "Sportgemeinschaft Lauenhain e.V./Abtl. Kanu", VNr = "15-065", EMail = "korehnke@hs-mittweida.de", ShortName = "SG Lauenhain" });
            builder.Entity<Club>().HasData(new Club { ClubId = 192, City = "Mülheim", Name = "DJK Ruhrwacht e.V. Mülheim/Ruhr", VNr = "10-229", ShortName = "DJK Mülheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 193, City = "Mülheim", Name = "Mülheimer Kanusport-Verein e.V.", VNr = "10-234", ShortName = "Mülheimer KV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 194, City = "München", Name = "Kanu-Regattaverein München", VNr = "02-155", ShortName = "KRV München" });
            builder.Entity<Club>().HasData(new Club { ClubId = 195, City = "München", Name = "MTV München von 1879 Abt. Kanu", VNr = "02-060", ShortName = "MTV München" });
            builder.Entity<Club>().HasData(new Club { ClubId = 196, City = "Nassau", Name = "Nassauer Kanu-Club 1950 e.V.", VNr = "13-012", ShortName = "Nassauer KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 197, City = "Neckarsulm", Name = "Kanu-Team Baden-Württemberg", VNr = "01-157", ShortName = "KT BW" });
            builder.Entity<Club>().HasData(new Club { ClubId = 198, City = "Neckarsulm", Name = "Neckarsulmer Sport-Union", VNr = "01-120", ShortName = "Neckarsulmer SU" });
            builder.Entity<Club>().HasData(new Club { ClubId = 199, City = "Nettetal", Name = "Ruder-u.Kanu-Club Lobberich 1948 e.V.", VNr = "10-202", ShortName = "RKC Lobberich" });
            builder.Entity<Club>().HasData(new Club { ClubId = 200, City = "Neubrandenburg", Name = "Sportclub Neubrandenburg e.V., Abtl. Kanu", VNr = "08-012", ShortName = "SC Neubrandenburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 3, City = "Bad Dürrenberg", Name = "KC Bad Dürrenberg e.V.", VNr = "16-025", ShortName = "Bad Dürrenberg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 21, City = "Berlin", Name = "Kanuklub Charlottenburg e.V.", VNr = "03-010", EMail = "webmaster@kanuklub-charlottenburg.de", ShortName = "KC Charlottenburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 201, City = "Neuburg", Name = "Donau-Ruder-Club Neuburg Kanu", VNr = "02-065", ShortName = "DRC Neuburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 202, City = "Neukloster", Name = "VfL Blau-Weiß Neukloster", VNr = "08-009", ShortName = "VfL Blau-Weiß" });
            builder.Entity<Club>().HasData(new Club { ClubId = 203, City = "Neumünster", Name = "Erster Kanuklub Neumünster e.V.", VNr = "17-021", ShortName = "KK Neumünster" });
            builder.Entity<Club>().HasData(new Club { ClubId = 204, City = "Neuruppin", Name = "Kanu-Verein Neuruppin e.V.", VNr = "04-003", ShortName = "KV Neuruppin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 205, City = "Neuss", Name = "Holzheimer Sportgemeinschaft 1920 e.V.", VNr = "10-159", ShortName = "Holzheimer SG" });
            builder.Entity<Club>().HasData(new Club { ClubId = 206, City = "Neustrelitz", Name = "Wassersportverein Einheit Neustrelitz, Abteilung Kanu", VNr = "08-005", ShortName = "WSV Neustrelitz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 207, City = "Neuwied", Name = "Neuwieder Wassersportverein e.V.", VNr = "13-014", ShortName = "Neuwieder WSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 208, City = "Niederkassel", Name = "Wassersportverein Blau-Weiß Rheidt", VNr = "10-261", ShortName = "WSV Rheidt" });
            builder.Entity<Club>().HasData(new Club { ClubId = 209, City = "Niegripp", Name = "SG Blau-Weiß Niegripp e.V.", VNr = "16-001", ShortName = "SGBW Niegripp" });
            builder.Entity<Club>().HasData(new Club { ClubId = 210, City = "Nürnberg", Name = "Kanu-Verein Nürnberg", VNr = "02-132", ShortName = "KV Nürnberg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 22, City = "Berlin", Name = "Kanu Team Berlin e. V.", VNr = "03-064", ShortName = "KT Berlin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 211, City = "Oberhausen", Name = "Kanu-Team 2000 e.V.", VNr = "10-483", ShortName = "KT 2000" });
            builder.Entity<Club>().HasData(new Club { ClubId = 212, City = "Oberhausen", Name = "Turnclub Sterkrade 1869 e.V. Oberhausen", VNr = "10-248", ShortName = "TC Sterkrade" });
            builder.Entity<Club>().HasData(new Club { ClubId = 213, City = "Oberhausen", Name = "Oberhausener Kanu-Verein v.1928 e.V.", VNr = "10-246", ShortName = "Oberhausender KV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 214, City = "Oberhausen", Name = "Alstadener Kanu-Club e.V. Oberhausen", VNr = "10-245", ShortName = "Alstadener KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 215, City = "Oberschleißheim", Name = "Schleißheimer Paddelclub e.V.", VNr = "02-166", ShortName = "Schleißheimer PC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 216, City = "Osnabrück", Name = "Wassersportverein Osnabrück e.V.", VNr = "09-079", ShortName = "WSV Osnabrück" });
            builder.Entity<Club>().HasData(new Club { ClubId = 217, City = "Parum", Name = "SG Blau - Weiß Parum e.V.Sektion Kanu", VNr = "08-032", ShortName = "SG BW Parum" });
            builder.Entity<Club>().HasData(new Club { ClubId = 218, City = "Peitz", Name = "Kanuverein Peitz e.V.", VNr = "04-035", ShortName = "KV Peitz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 219, City = "Petershagen", Name = "KSV Kenterpreis Windheim e.V.", VNr = "10-477", ShortName = "KSV Windheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 220, City = "Pirna", Name = "Sportverein Grün-Weiß Pirna e.V. Kanuabt.", VNr = "15-031", ShortName = "SV GW Prina" });
            builder.Entity<Club>().HasData(new Club { ClubId = 23, City = "Berlin", Name = "Pro Sport Berlin 24 e.V., Kanuabteilung (ehemals Post SV Berlin)", VNr = "03-017", ShortName = "Pro Sport Berlin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 221, City = "Plön", Name = "Wassersportverein Plön-Fegetasche e.V.", VNr = "17-024", ShortName = "WSV Plön" });
            builder.Entity<Club>().HasData(new Club { ClubId = 222, City = "Potsdam", Name = "Preussen-Kanu im OSC Luftschiffhafen e.V. Potsdam", VNr = "04-005", ShortName = "OSC LH Potsdam" });
            builder.Entity<Club>().HasData(new Club { ClubId = 223, City = "Potsdam", Name = "Wassersportfreunde Pirschheide e.V. Abtl. Kanu", VNr = "04-018", ShortName = "WSF Pirschheide" });
            builder.Entity<Club>().HasData(new Club { ClubId = 224, City = "Potsdam", Name = "Kanu Club Potsdam im OSC Luftschiffhafen e.V.", VNr = "04-007", ShortName = "KC Potsdam" });
            builder.Entity<Club>().HasData(new Club { ClubId = 225, City = "Preetz", Name = "Preetzer Turn- und Sportverein e.V. - Kanuabt.", VNr = "17-026", ShortName = "Preetzer TuS" });
            builder.Entity<Club>().HasData(new Club { ClubId = 226, City = "Prenzlau", Name = "Sport Club Blau Weiß Energie Prenzlau e.V.", VNr = "04-032", ShortName = "S BW Prenzlau" });
            builder.Entity<Club>().HasData(new Club { ClubId = 227, City = "Radevormwald", Name = "Kanusportverein Radevormwald/Remscheidt e.V.", VNr = "10-480", ShortName = "KV Radevormwald" });
            builder.Entity<Club>().HasData(new Club { ClubId = 228, City = "Raisdorf", Name = "Raisdorfer Kanu-Klub e.V.", VNr = "17-031", ShortName = "Raisdorfer KK" });
            builder.Entity<Club>().HasData(new Club { ClubId = 229, City = "Rastatt", Name = "Rastatter Kanu-Club 1925 e.V.", VNr = "01-043", ShortName = "Rastatter KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 230, City = "Rathenow", Name = "Rathenower Wassersportverein Kanu 1922 e.V.", VNr = "04-017", ShortName = "Rathenower WSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 24, City = "Berlin", Name = "Heiligenseer Kanu-Club e.V.", VNr = "03-004", ShortName = "Heiligenseer KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 231, City = "Raunheim", Name = "EURO CANOE SPORT TEAM 2000 RAUNHEIM e.V.", VNr = "07-111", ShortName = "ECST Raunheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 232, City = "Raunheim", Name = "KCW 1955 Raunheim", VNr = "07-061", ShortName = "KCW Raunheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 233, City = "Recklinghausen", Name = "TuWSV e.V. Recklinghausen-Süd, Kanuabteilung", VNr = "10-258", ShortName = "TuWSV Recklinghausen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 234, City = "Regensburg", Name = "Regensburger Ruderverein Faltbootabteilung", VNr = "02-081", EMail = "asdfsdaf@sfasf.de", ShortName = "Regensburger RV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 235, City = "Rendsburg", Name = "Rendsburger Kanu-Club e.V.", VNr = "17-027", ShortName = "Rendsburger KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 236, City = "Rheine", Name = "Wassersportverein Rheine 1932 e.V.", VNr = "10-265", ShortName = "WSV Rheine" });
            builder.Entity<Club>().HasData(new Club { ClubId = 237, City = "Rheine", Name = "Paddel- und Radsportclub Emsstern Rheine 1933 e.V.", VNr = "10-264", ShortName = "PRSC Emsstem" });
            builder.Entity<Club>().HasData(new Club { ClubId = 238, City = "Rheine", Name = "Kanu-Club Rheine 1950 e.V.", VNr = "10-263", ShortName = "KC Rheine" });
            builder.Entity<Club>().HasData(new Club { ClubId = 239, City = "Rheinsheim", Name = "KV Bruhrain Rheinsheim e.V.", VNr = "01-039", ShortName = "KV Bruhrain" });
            builder.Entity<Club>().HasData(new Club { ClubId = 240, City = "Riesa", Name = "Sportclub Riesa e.V. Kanuabt.", VNr = "15-045", ShortName = "SC Riesa" });
            builder.Entity<Club>().HasData(new Club { ClubId = 25, City = "Berlin", Name = "Verein für Kanusport Berlin e.V.", VNr = "03-024", ShortName = "Verein KS Berlin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 241, City = "Riesa", Name = "Riesaer Wassersportverein e.V.", VNr = "15-028", ShortName = "Riesaer WSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 242, City = "Rogätz", Name = "Sportclub Kanu Rogätz e.V.", VNr = "16-039", ShortName = "SC Rogätz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 243, City = "Rosenheim", Name = "Kajak-Klub Rosenheim", VNr = "02-084", ShortName = "KK Rosenheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 244, City = "Rosenthal", Name = "WSV Rosenthal e.V. Abt. Kanurennsport", VNr = "18-024", ShortName = "WSV Rosenthal" });
            builder.Entity<Club>().HasData(new Club { ClubId = 245, City = "Rostock", Name = "Kanufreunde Rostocker Greif e.V.", VNr = "08-015", ShortName = "KF Rostocker Greif" });
            builder.Entity<Club>().HasData(new Club { ClubId = 246, City = "Rostock", Name = "Sportverein Breitling e.V.", VNr = "08-007", ShortName = "SV Breitling" });
            builder.Entity<Club>().HasData(new Club { ClubId = 247, City = "Rostock", Name = "Rostocker Kanu-Club e.V.", VNr = "08-016", ShortName = "Rostocker KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 248, City = "Rüsselsheim", Name = "WSV ‚Undine' Rüsselsheim", VNr = "07-064", ShortName = "WSV Rüsselsheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 249, City = "Saarbrücken", Name = "Saarbrücker Kanu-Club e.V.", VNr = "14-007", ShortName = "Saarbrücker KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 250, City = "Saarbrücken", Name = "Kanu-Wanderer Saarbrücken e.V.", VNr = "14-006", ShortName = "KW Saarbrücken" });
            builder.Entity<Club>().HasData(new Club { ClubId = 26, City = "Berlin", Name = "Wander-Paddler-Havel e. V.", VNr = "03-027", ShortName = "WP Havel" });
            builder.Entity<Club>().HasData(new Club { ClubId = 251, City = "Saarbrücken", Name = "VfK Saar", VNr = "14-003", ShortName = "VfK Saar" });
            builder.Entity<Club>().HasData(new Club { ClubId = 252, City = "Saarbrücken", Name = "Verein zur Förderung des Jugendsports Saar, Abteilung Kanu", VNr = "14-015", ShortName = "VzFJ Saar" });
            builder.Entity<Club>().HasData(new Club { ClubId = 253, City = "Saarlouis", Name = "Kanu-Club Undine Saarlouis", VNr = "14-008", ShortName = "KC Saarlouis" });
            builder.Entity<Club>().HasData(new Club { ClubId = 254, City = "Salzgitter", Name = "Kanu-Club Salzgitter e.V.", VNr = "09-087", ShortName = "KC Salzgitter" });
            builder.Entity<Club>().HasData(new Club { ClubId = 255, City = "Sandersdorf", Name = "Sandersdorfer Kanu-Verein", VNr = "16-056", ShortName = "Sandersdorfer KV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 256, City = "Schönebeck", Name = "Schönebecker Sportclub e.V., Kanuabt.", VNr = "16-037", ShortName = "Schönebecker SC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 257, City = "Schönebeck", Name = "SG Lok Schönebeck", VNr = "16-042", ShortName = "SG Lok Schönebeck" });
            builder.Entity<Club>().HasData(new Club { ClubId = 258, City = "Schwedt", Name = "Wassersport PCK Schwedt e.V., Abtl. Kanu", VNr = "04-043", ShortName = "WS PCK Schwedt" });
            builder.Entity<Club>().HasData(new Club { ClubId = 259, City = "Schwerin", Name = "Kanurenngemeinschaft Schwerin e.V.", VNr = "08-003", ShortName = "KRG Schwerin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 260, City = "Schwerte", Name = "Kanu-und Surf-Verein Schwerte e.V.", VNr = "10-272", ShortName = "KSV Schwerte" });
            builder.Entity<Club>().HasData(new Club { ClubId = 27, City = "Berlin", Name = "Kajak-Club Albatros 1926 e.V.", VNr = "03-005", EMail = "thorstenschwerdtfeger@msn.com", ShortName = "KC Albatros" });
            builder.Entity<Club>().HasData(new Club { ClubId = 261, City = "Schwörstadt", Name = "WSV e.V. Rheinstrom Schwörstadt Hochrhein", VNr = "01-046", ShortName = "WSV Schwörstadt" });
            builder.Entity<Club>().HasData(new Club { ClubId = 262, City = "Spremberg", Name = "Sportgemeinschaft Einheit Spremberg e.V., Kanuabt.", VNr = "04-014", ShortName = "SG Spremberg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 263, City = "Springe", Name = "Kanu-Club Springe e.V.", VNr = "09-093", ShortName = "KC Springe" });
            builder.Entity<Club>().HasData(new Club { ClubId = 264, City = "Stahnsdorf", Name = "Aktiv e.V., Abtl. Parakanu", VNr = "04-048", ShortName = "Aktiv Stahnsdorf" });
            builder.Entity<Club>().HasData(new Club { ClubId = 265, City = "Stralsund", Name = "Stralsunder Kanu-Club e.V.", VNr = "08-001", ShortName = "Stralsunder KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 266, City = "Stuttgart", Name = "KG Stuttgart e.V.", VNr = "01-128", ShortName = "KG Stuttgart" });
            builder.Entity<Club>().HasData(new Club { ClubId = 267, City = "Templin", Name = "Kanusportverein Templin e.V.", VNr = "04-011", ShortName = "KSV Templin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 268, City = "Trier", Name = "Trierer Kanu-Fahrer 1948 e.V.", VNr = "13-018", ShortName = "Trierer KF" });
            builder.Entity<Club>().HasData(new Club { ClubId = 269, City = "Troisdorf", Name = "Kanu-Klub Pirat e.V. Bergheim/Sieg", VNr = "10-012", ShortName = "KK Pirat Bergheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 270, City = "Verden", Name = "Wassersportverein Verden e.V.", VNr = "09-100", ShortName = "WSV Verden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 28, City = "Berlin", Name = "Berliner Sportverein Akademie d.Wissenschaften e.V., Kanuabt.", VNr = "03-053", ShortName = "AdW Berlin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 271, City = "Voerde", Name = "Kanu-Club Friedrichsfeld e.V.", VNr = "10-118", ShortName = "KC Friedrichsfeld" });
            builder.Entity<Club>().HasData(new Club { ClubId = 272, City = "Waren", Name = "Müritzsportclub Waren e.V., Abtl. Kanu", VNr = "08-013", ShortName = "MSC Waren" });
            builder.Entity<Club>().HasData(new Club { ClubId = 273, City = "Wehr", Name = "Kanu Sport Wehr e.V.", VNr = "01-206", ShortName = "KS Wehr" });
            builder.Entity<Club>().HasData(new Club { ClubId = 274, City = "Wengelsdorf", Name = "SV 1919 Wacker Wengelsdorf/Abt. Kanurennsport", VNr = "16-063", ShortName = "SV Wacker" });
            builder.Entity<Club>().HasData(new Club { ClubId = 275, City = "Wesenberg", Name = "SV Union Wesenberg, Abtl. Kanu", VNr = "08-021", ShortName = "SV Wesenberg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 276, City = "Wiesbaden", Name = "Wassersport-Verein Schierstein 1921 e.V.", VNr = "07-073", ShortName = "WSV Schierstein" });
            builder.Entity<Club>().HasData(new Club { ClubId = 277, City = "Wilhelmshaven", Name = "Wilhelmshavener Kanu-Freunde 1950 e.V.", VNr = "09-108", ShortName = "Wilhelmshavener KF" });
            builder.Entity<Club>().HasData(new Club { ClubId = 278, City = "Wismar", Name = "Turn- und Sportgemeinschaft Wismar", VNr = "08-002", ShortName = "TuS Wismar" });
            builder.Entity<Club>().HasData(new Club { ClubId = 279, City = "Witten", Name = "Kanu-Club Witten e.V.", VNr = "10-305", ShortName = "KC Witten" });
            builder.Entity<Club>().HasData(new Club { ClubId = 280, City = "Wittenberge", Name = "Wassersportverein Wittenberge e.V.", VNr = "04-029", ShortName = "WSV Wittenberge" });
            builder.Entity<Club>().HasData(new Club { ClubId = 29, City = "Berlin", Name = "Köpenicker Kanusportclub e. V.", VNr = "03-045", ShortName = "Köpenicker KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 281, City = "Wolfsburg", Name = "Wolfsburger Kanu-Club e.V.", VNr = "09-111", ShortName = "Wolfsburger KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 282, City = "Wolmirstedt", Name = "Wolmirstedter KV", VNr = "16-011", ShortName = "Wolmirstedter KV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 283, City = "Worms", Name = "Wassersportverein Worms e.V.", VNr = "12-014", ShortName = "WSV Worms" });
            builder.Entity<Club>().HasData(new Club { ClubId = 284, City = "Wuppertal", Name = "Verein für Kanusport e.V. Wuppertal", VNr = "10-314", ShortName = "VK Wuppertal" });
            builder.Entity<Club>().HasData(new Club { ClubId = 285, City = "Wuppertal", Name = "Wuppertaler Kanu-Club e.V.", VNr = "10-315", ShortName = "Wuppertaler KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 286, City = "Wuppertal", Name = "Wuppertaler Paddler-Gilde e.V.", VNr = "10-316", ShortName = "Wuppertaler PG" });
            builder.Entity<Club>().HasData(new Club { ClubId = 287, City = "Wuppertal", Name = "ESV Wuppertal Ost e.V., Kanuabteilung", VNr = "10-311", ShortName = "ESV Wuppertal" });
            builder.Entity<Club>().HasData(new Club { ClubId = 288, City = "Wuppertal", Name = "Kanu-Sport-Gemeinschaft Wuppertal e.V.", VNr = "10-312", ShortName = "KSG Wuppertal" });
            builder.Entity<Club>().HasData(new Club { ClubId = 289, City = "Würzburg", Name = "Kanurennsport Verein Bayern", VNr = "02-163", ShortName = "KRV Bayern" });
            builder.Entity<Club>().HasData(new Club { ClubId = 290, City = "Würzburg", Name = "Kanu-Club Würzburg", VNr = "02-100", ShortName = "KC Würzburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 30, City = "Bernburg", Name = "Bernburger-Wassersport-Verein e.V.", VNr = "16-030", ShortName = "Bernburger WSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 291, City = "Würzburg", Name = "TG Würzburg Heidingsfeld Kanuabt.", VNr = "02-101", ShortName = "TG Würzburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 292, City = "Wurzen", Name = "Sportgemeinschaft Lokomotive Wurzen e.V.", VNr = "15-007", ShortName = "SG Lok Wurzen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 293, City = "Wusterwitz", Name = "Blau Weiß Wusterwitz e.V., Abtl. Kanu", VNr = "04-019", ShortName = "BW Wusterwitz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 294, City = "Platzhalter", Name = "Platzhalter", VNr = "00-000", ShortName = "Platzhalter" });
            builder.Entity<Club>().HasData(new Club { ClubId = 4, City = "Bad Lobenstein", Name = "Kanuteam Thüringen e.V.", VNr = "18-031", ShortName = "Bad Lobenstein" });
            builder.Entity<Club>().HasData(new Club { ClubId = 31, City = "Bitburg", Name = "Turnverein Bitburg 1911 e.V. Kanuabteilung", VNr = "13-001", ShortName = "TV Bitburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 32, City = "Bochum", Name = "Bochumer Kanu-Club e.V.", VNr = "10-016", ShortName = "Bochumer KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 33, City = "Bochum", Name = "Linden-Dahlhauser Kanu-Club e.V.", VNr = "10-018", ShortName = "Linden-Dahlhauser KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 34, City = "Bochum", Name = "Kanu-Club Wiking Bochum 1951 e.V.", VNr = "10-017", ShortName = "KC Wiking" });
            builder.Entity<Club>().HasData(new Club { ClubId = 35, City = "Borna", Name = "SV Blau-Gelb Borna e.V.", VNr = "15-010", ShortName = "SV Borna" });
            builder.Entity<Club>().HasData(new Club { ClubId = 36, City = "Bornheim", Name = "Herseler Wassersportverein 1930 e.V.", VNr = "10-155", ShortName = "Herseler WSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 37, City = "Bramsche", Name = "Turn- u. Sportverein Bramsche, Kanu-Abteilung", VNr = "09-009", ShortName = "TuS Bramsche" });
            builder.Entity<Club>().HasData(new Club { ClubId = 38, City = "Brandenburg", Name = "Brandenburger Kanuverein Freie Wasserfahrer 1925 e.V.", VNr = "04-020", ShortName = "Freie Wasserfahrer" });
            builder.Entity<Club>().HasData(new Club { ClubId = 39, City = "Brandenburg", Name = "Wassersportverein Stahl Beetzsee Brandenburg e.V., Kanu", VNr = "04-028", ShortName = "WSV Stahl Beetzsee" });
            builder.Entity<Club>().HasData(new Club { ClubId = 40, City = "Brandenburg", Name = "Brandenburger Sport-Club Süd 05 e.V. Abteilung Kanu", VNr = "04-006", ShortName = "Brandenburger SC Süd" });
            builder.Entity<Club>().HasData(new Club { ClubId = 5, City = "Bamberg", Name = "Bamberger Faltbootclub", VNr = "02-006", ShortName = "Bamberg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 41, City = "Braunsbedra OT Großkayna", Name = "SV Großkayna 1922 e.V.", VNr = "16-065", ShortName = "SV Großkayna" });
            builder.Entity<Club>().HasData(new Club { ClubId = 42, City = "Bremen", Name = "Störtebeker Bremer Paddelsport e.V von 1924", VNr = "05-015", ShortName = "Störtebeker PS" });
            builder.Entity<Club>().HasData(new Club { ClubId = 43, City = "Bremen", Name = "Turn- und Rasensportverein Bremen e.V.", VNr = "05-017", ShortName = "TuRSV Bremen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 44, City = "Bremen", Name = "Verein für Kanusport Bremen e.V.", VNr = "05-018", ShortName = "VfK Bremen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 45, City = "Burg", Name = "WSF Burg 1924 e.V.", VNr = "16-012", ShortName = "WSF Burg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 46, City = "Calbe", Name = "TSG Calbe e.V. Abt. Kanu", VNr = "16-032", ShortName = "TSG Calbe" });
            builder.Entity<Club>().HasData(new Club { ClubId = 47, City = "Coburg", Name = "Paddel u. Segelclub Coburg-Schney 1926 e.V.", VNr = "02-012", ShortName = "PuS Coburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 48, City = "Coburg", Name = "Schwimmverein Coburg Faltbootabteilung", VNr = "02-013", ShortName = "SV Coburg" });
            builder.Entity<Club>().HasData(new Club { ClubId = 49, City = "Colditz", Name = "Colditzer Kanu- und Sportverein e.V.", VNr = "15-066", EMail = "zyma.randy@web.de", ShortName = "Colditzer KSV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 50, City = "Cottbus", Name = "Eisenbahnersportverein Lok RAW Cottbus e.V. Kanuabt.", VNr = "04-013", ShortName = "ESV Lok Cottbus" });
            builder.Entity<Club>().HasData(new Club { ClubId = 6, City = "Barby", Name = "SSV Blau - Weiß 04 Barby e.V.", VNr = "16-041", ShortName = "Barby" });
            builder.Entity<Club>().HasData(new Club { ClubId = 51, City = "Darmstadt", Name = "Darmstädter TSG 1846 - Kanuabteilung", VNr = "07-003", ShortName = "Darmstädter TSG" });
            builder.Entity<Club>().HasData(new Club { ClubId = 52, City = "Datteln", Name = "Kanuten Emscher Lippe e.V. Datteln", VNr = "10-038", ShortName = "Emscher Lippe" });
            builder.Entity<Club>().HasData(new Club { ClubId = 53, City = "Dillingen", Name = "Kanu-Club Dillingen e.V./Saar", VNr = "14-002", ShortName = "KC Dillingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 54, City = "Döbeln", Name = "Eisenbahnersportverein Lokomotive Döbeln e.V.", VNr = "15-005", EMail = "husky24@gmx.de", ShortName = "ESV Lok Döbeln" });
            builder.Entity<Club>().HasData(new Club { ClubId = 55, City = "Dortmund", Name = "Freier Sportverein von 1898 Dortmund e.V.,Kanu-Abtl.", VNr = "10-043", ShortName = "FSV Dortmund" });
            builder.Entity<Club>().HasData(new Club { ClubId = 56, City = "Dresden", Name = "Sportverein TuR Dresden e.V. Kanuabt.", VNr = "15-055", ShortName = "TuR Dresden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 57, City = "Dresden", Name = "Eisenbahner Sportverein Dresden e.V.", VNr = "15-027", ShortName = "ESV Dresden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 58, City = "Dresden", Name = "Kanuverein Laubegast e.V. Dresden", VNr = "15-039", ShortName = "KV Laubegast" });
            builder.Entity<Club>().HasData(new Club { ClubId = 59, City = "Dresden", Name = "TSV Rotation Dresden 1990 e.V. Kanuabt.", VNr = "15-013", EMail = "rennsportwart@tsv-rotation-kanurennsport.de", ShortName = "TSV Rotation" });
            builder.Entity<Club>().HasData(new Club { ClubId = 60, City = "Dresden", Name = "Verein Kanusport Dresden e.V.", VNr = "15-011", ShortName = "VK Dresden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 7, City = "Bederkesa", Name = "Wassersportverein Bederkesa e.V.", VNr = "09-006", ShortName = "Bederkesa" });
            builder.Entity<Club>().HasData(new Club { ClubId = 61, City = "Dresden", Name = "Wassersportverein Am Blauen Wunder e.V.Dresden", VNr = "15-059", EMail = "burkhama@freenet.de", ShortName = "BW Dresden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 62, City = "Dresden", Name = "Kanu-Club Dresden e.V.", VNr = "15-026", ShortName = "KC Dresden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 63, City = "Duisburg", Name = "Bertasee Duisburg e.V.", VNr = "10-067", ShortName = "Bertasee" });
            builder.Entity<Club>().HasData(new Club { ClubId = 64, City = "Duisburg", Name = "ESV Grün-Weiß-Meiderich e.V., Kanuabteilung", VNr = "10-071", ShortName = "ESV GW Meiderich" });
            builder.Entity<Club>().HasData(new Club { ClubId = 65, City = "Duisburg", Name = "Wassersportverein Niederrhein e.V. Duisburg", VNr = "10-084", ShortName = "WSV Niederrhein" });
            builder.Entity<Club>().HasData(new Club { ClubId = 66, City = "Düsseldorf", Name = "Wassersportverein Rheintreue Düsseldorf e.V.", VNr = "10-063", ShortName = "WSV Rheintreue" });
            builder.Entity<Club>().HasData(new Club { ClubId = 67, City = "Düsseldorf", Name = "Düsseldorfer Kanu-Club e.V.", VNr = "10-050", ShortName = "Düsseldorfer KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 68, City = "Eberswalde-Finow", Name = "Sportvereinigung Stahl Finow e.V. Sek. Kanu", VNr = "04-016", ShortName = "SV Stahl Finow" });
            builder.Entity<Club>().HasData(new Club { ClubId = 69, City = "Eberswalde-Finow", Name = "Eberswalder Sportverein Empor e.V.Kanuabt.", VNr = "04-022", ShortName = "SV Empor" });
            builder.Entity<Club>().HasData(new Club { ClubId = 70, City = "Eisenhüttenstadt", Name = "Kanu-Verein Brandenburger Adler e. V.", VNr = "04-040", ShortName = "KV Adler" });
            builder.Entity<Club>().HasData(new Club { ClubId = 8, City = "Berlin", Name = "Sportclub Berlin-Grünau e. V., Abt. Kanu", VNr = "03-050", ShortName = "Berlin-Grünau" });
            builder.Entity<Club>().HasData(new Club { ClubId = 71, City = "Eisenhüttenstadt", Name = "Kanucentrum 1957 Eisenhüttenstadt e.V.", VNr = "04-001", ShortName = "KC Eisenhüttenstadt" });
            builder.Entity<Club>().HasData(new Club { ClubId = 72, City = "Elze", Name = "CJD Elze Leichtathletik e.V.", VNr = "09-024", ShortName = "CJD Elze" });
            builder.Entity<Club>().HasData(new Club { ClubId = 73, City = "Emsdetten", Name = "Canu-Club Emsdetten 1950 e.V.", VNr = "10-087", ShortName = "CC Emsdetten" });
            builder.Entity<Club>().HasData(new Club { ClubId = 74, City = "Erlangen", Name = "Naturfreunde Deutschlands, OG Erlangen", VNr = "02-020", ShortName = "NFD Erlangen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 75, City = "Esens", Name = "Wassersportverein Harle e.V.", VNr = "09-001", ShortName = "WSV Harle" });
            builder.Entity<Club>().HasData(new Club { ClubId = 76, City = "Essen", Name = "Eisenbahner-Sportverein Essen Kupferdreh e.V.", VNr = "10-092", ShortName = "ESV Essen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 77, City = "Essen", Name = "Schwimmverein Steele 1911 e.V., Kanuabteilung", VNr = "10-113", ShortName = "SV Steele" });
            builder.Entity<Club>().HasData(new Club { ClubId = 78, City = "Essen", Name = "Kanusport-Gemeinschaft Essen e.V.", VNr = "10-105", ShortName = "KG Essen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 79, City = "Essen", Name = "Steeler Kanu Club e.V.", VNr = "10-114", ShortName = "Steeler KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 80, City = "Essen", Name = "Kanu-Regatta-Verein Baldeneysee e.V. Essen", VNr = "10-506", ShortName = "KRV Baldeneysee" });
            builder.Entity<Club>().HasData(new Club { ClubId = 9, City = "Berlin", Name = "Berliner Kanu Club Borussia e.V.", VNr = "03-001", EMail = "test", ShortName = "Borussia" });
            builder.Entity<Club>().HasData(new Club { ClubId = 81, City = "Esslingen", Name = "KV Esslingen", VNr = "01-105", ShortName = "KV Esslingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 82, City = "Esslingen", Name = "Sportvereinigung 1845 Esslingen e.V., Kanuabtl.", VNr = "01-106", ShortName = "SV Esslingen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 83, City = "Flecken Zechlin", Name = "Wassersportverein Zechlin e.V.", VNr = "04-046", ShortName = "WSV Zechlin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 84, City = "Flensburg", Name = "Flensburger Paddelfreunde e.V.", VNr = "17-009", ShortName = "Flensburger PF" });
            builder.Entity<Club>().HasData(new Club { ClubId = 85, City = "Flöha", Name = "Kanusportverein 1928 Flöha e.V.", VNr = "15-006", EMail = "ksv-floeha@t-online.de", ShortName = "KSV Flöha" });
            builder.Entity<Club>().HasData(new Club { ClubId = 86, City = "Forst", Name = "Sportgemeinschaft Turbine Forst/Lausitz e.V. Sek. Kanu", VNr = "04-009", ShortName = "SGTF Lausitz" });
            builder.Entity<Club>().HasData(new Club { ClubId = 87, City = "Frankfurt/M.", Name = "Frankfurter Ruder- u. Kanusportverein Sachsenhausen v. 1898 e.V.", VNr = "07-015", ShortName = "FFRKV Sachsenhausen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 88, City = "Frankfurt/M.", Name = "Frankfurter Kanu-Verein 1913 e.V.", VNr = "07-014", ShortName = "FFKV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 89, City = "Friedersdorf", Name = "WSC Friedersdorf 1949 e.V.", VNr = "16-016", ShortName = "WSC Friedersdorf" });
            builder.Entity<Club>().HasData(new Club { ClubId = 90, City = "Friedrichshafen", Name = "KSF Friedrichshafen", VNr = "01-109", ShortName = "KSF Friedrichshafen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 10, City = "Berlin", Name = "Grünauer Kanuverein 1990 e. V.", VNr = "03-042", ShortName = "GKV" });
            builder.Entity<Club>().HasData(new Club { ClubId = 91, City = "Fürth", Name = "TV Fürth 1860 Kanuabt.", VNr = "02-024", ShortName = "TV Fürth" });
            builder.Entity<Club>().HasData(new Club { ClubId = 92, City = "Gelsenkirchen", Name = "Gelsenkirchener Kanu-Club e.V.", VNr = "10-121", ShortName = "Gelsenkirchener KC" });
            builder.Entity<Club>().HasData(new Club { ClubId = 93, City = "Gemünden", Name = "KT Main-Spessart", VNr = "02-164", ShortName = "KT Main-Spessart" });
            builder.Entity<Club>().HasData(new Club { ClubId = 94, City = "Gemünden", Name = "Kanu und Ski-Club 1929 e.V. Gemünden am Main", VNr = "02-028", ShortName = "KSC Gemünden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 95, City = "Gemünden", Name = "White Water Company Gemünden", VNr = "02-145", ShortName = "WWC Gemünden" });
            builder.Entity<Club>().HasData(new Club { ClubId = 96, City = "Genthin", Name = "SV Chemie Genthin e.V., Sek. Kanu", VNr = "16-031", ShortName = "SVC Genthin" });
            builder.Entity<Club>().HasData(new Club { ClubId = 97, City = "Geringswalde", Name = "Kanuverein Geringswalde e.V.", VNr = "15-062", EMail = "ines.naarmann@web.de", ShortName = "KV Geringswalde" });
            builder.Entity<Club>().HasData(new Club { ClubId = 98, City = "Gießen", Name = "SKC Gießen", VNr = "07-025", ShortName = "SKC Gießen" });
            builder.Entity<Club>().HasData(new Club { ClubId = 99, City = "Ginsheim", Name = "KV Ginsheim-Gustavsburg", VNr = "07-027", ShortName = "KV Ginsheim" });
            builder.Entity<Club>().HasData(new Club { ClubId = 100, City = "Görlitz", Name = "NSV Gelb - Weiß Görlitz e.V.Kanuabt.", VNr = "15-032", ShortName = "NSV Görlitz" });

            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 1, Name = "K1", Seats = 1 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 2, Name = "K2", Seats = 2 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 11, Name = "S4", Seats = 4 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 3, Name = "K4", Seats = 4 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 4, Name = "C1", Seats = 1 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 5, Name = "C2", Seats = 2 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 6, Name = "C4", Seats = 4 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 7, Name = "S1", Seats = 1 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 8, Name = "S2", Seats = 2 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 9, Name = "S6", Seats = 6 });
            builder.Entity<Boatclass>().HasData(new Boatclass { BoatclassId = 10, Name = "S8", Seats = 8 });

            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1, FromAge = 0, Name = "Schüler D", ToAge = 6 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 2, FromAge = 7, Name = "Schüler C", ToAge = 9 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 11, FromAge = 60, Name = "Senioren D", ToAge = 99 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1002, FromAge = 13, Name = "Schüler A13", ToAge = 13 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1003, FromAge = 14, Name = "Schüler A14", ToAge = 14 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1004, FromAge = 10, Name = "Schüler B10", ToAge = 10 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1007, FromAge = 7, Name = "Schüler C7", ToAge = 7 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1008, FromAge = 8, Name = "Schüler C8", ToAge = 8 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1009, FromAge = 9, Name = "Schüler C9", ToAge = 9 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1010, FromAge = 11, Name = "Schüler B11", ToAge = 11 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1011, FromAge = 12, Name = "Schüler B12", ToAge = 12 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1012, FromAge = 7, Name = "Schüler C/B10", ToAge = 10 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 3, FromAge = 10, Name = "Schüler B", ToAge = 12 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 1013, FromAge = 0, Name = "alle Klassen", ToAge = 99 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 4, FromAge = 13, Name = "Schüler A", ToAge = 14 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 5, FromAge = 15, Name = "Jugend", ToAge = 16 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 6, FromAge = 17, Name = "Junioren", ToAge = 18 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 7, FromAge = 19, Name = "Leistungsklasse", ToAge = 31 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 8, FromAge = 32, Name = "Senioren A", ToAge = 39 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 9, FromAge = 40, Name = "Senioren B", ToAge = 49 });
            builder.Entity<Oldclass>().HasData(new Oldclass { OldclassId = 10, FromAge = 50, Name = "Senioren C", ToAge = 59 });

            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 1, Length = 200, Name = "200m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 2, Length = 500, Name = "500m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 3, Length = 1000, Name = "1000m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 1002, Length = 2000, Name = "2000m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 1003, Length = 4000, Name = "4000m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 1004, Length = 6000, Name = "6000m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 1005, Length = 100, Name = "100m" });
            builder.Entity<Raceclass>().HasData(new Raceclass { RaceclassId = 1006, Length = 250, Name = "250m" });
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
        public DbSet<RegattaMeldung.Models.DeletedStartboats> DeletedStartboats { get; set; }
        public DbSet<RegattaMeldung.Models.RRFreeStartslots> RRFreeStartslots { get; set; }
    }
}
