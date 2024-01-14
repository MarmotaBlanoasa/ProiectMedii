using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProiectMedii.Models;

namespace ProiectMedii.Data
{
    public class ProiectMediiContext : DbContext
    {
        public ProiectMediiContext (DbContextOptions<ProiectMediiContext> options)
            : base(options)
        {
        }

        public DbSet<Speaker> Speaker { get; set; } = default!;
        public DbSet<Event> Events { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Utilizator> User { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<EventSponsor> EventSponsors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventSponsor>()
                .HasKey(es => new { es.EventId, es.SponsorId });

            modelBuilder.Entity<EventSponsor>()
                .HasOne(es => es.Event)
                .WithMany(e => e.EventSponsors)
                .HasForeignKey(es => es.EventId);

            modelBuilder.Entity<EventSponsor>()
                .HasOne(es => es.Sponsor)
                .WithMany(s => s.EventSponsors)
                .HasForeignKey(es => es.SponsorId);
            modelBuilder.Entity<Ticket>().HasKey(t => t.TicketId);

            // Configure the foreign key from Ticket to Event
            modelBuilder.Entity<Ticket>()
                .HasOne<Event>(t => t.Event)
                .WithMany(e => e.Tickets) // Assuming you have a collection of Tickets in your Event class
                .HasForeignKey(t => t.EventId);
        }
    }
}
