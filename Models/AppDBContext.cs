using Microsoft.EntityFrameworkCore;


namespace MonitoreoAPI.Models
{
    public class AppDBContext : DbContext 
    {       
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {            
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Planta> Plantas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Aplicacion> Aplicaciones { get; set; }
        public DbSet<Tipo_recurso> Tipo_Recursos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recurso>()
                .ToTable("Recursos");

            modelBuilder.Entity<Equipo>()
                .ToTable("Equipos");

            modelBuilder.Entity<Aplicacion>()
                .ToTable("Aplicaciones");
      
            modelBuilder.Entity<Planta>()
                .HasMany(e => e.Areas)
                .WithOne(e => e.Planta)
                .HasForeignKey(e => e.Id_planta)
                .IsRequired();

            modelBuilder.Entity<Estado>()
                .HasMany(e => e.Recursos)
                .WithOne(e => e.Estado)
                .HasForeignKey(e => e.Id_estado)
                .IsRequired();

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Recursos)
                .WithOne(e => e.Area)
                .HasForeignKey(e => e.Id_area)
                .IsRequired();

            modelBuilder.Entity<Tipo_recurso>()
                .HasMany(t => t.Recursos)
                .WithOne(t => t.TipoRecurso)
                .HasForeignKey(t => t.Id_tipo_recurso)
                .IsRequired();
        }
    }
}
