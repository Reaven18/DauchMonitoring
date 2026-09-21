using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DauchMonitoring.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Planta> Plantas { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Aplicacion> Aplicaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Recurso)
                .WithOne(r => r.Equipo)
                .HasForeignKey<Equipo>(e => e.Id);

            modelBuilder.Entity<Aplicacion>()
                .HasOne(a => a.Recurso)
                .WithOne(r => r.Aplicacion)
                .HasForeignKey<Aplicacion>(a => a.Id);

            modelBuilder.Entity<Area>()
                .HasOne(a => a.Planta)
                .WithMany(p => p.Areas)
                .HasForeignKey(a => a.IdPlanta);            

            modelBuilder.Entity<Recurso>()
                .HasOne(r => r.Area)
                .WithMany(a => a.Recursos)
                .HasForeignKey(r => r.IdArea);

            modelBuilder.Entity<Recurso>()
                .HasOne(r => r.Estado)
                .WithMany(e => e.Recursos)
                .HasForeignKey(r => r.IdEstado);
        }

    }
}
