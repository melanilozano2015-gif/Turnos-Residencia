using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Models;

namespace ResidenciaApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NinoAdolescente> NinosAdolescentes { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<HistorialTurno> HistorialTurnos { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<Profesional> Profesionales { get; set; }
    public DbSet<Institucion> Instituciones { get; set; }
    public DbSet<EstadoTurno> EstadosTurno { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<NinoAdolescente>(entity =>
        {
            entity.HasIndex(e => e.Dni).IsUnique();
        });

        builder.Entity<Turno>(entity =>
        {
            entity.HasOne(e => e.NinoAdolescente)
                  .WithMany(n => n.Turnos)
                  .HasForeignKey(e => e.IdNinoAdolescente)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.EstadoTurno)
                  .WithMany(es => es.Turnos)
                  .HasForeignKey(e => e.IdEstadoTurno)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Turnos)
                  .HasForeignKey(e => e.IdUsuario)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<HistorialTurno>(entity =>
        {
            entity.HasOne(e => e.Turno)
                  .WithMany(t => t.HistorialTurnos)
                  .HasForeignKey(e => e.IdTurno)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.HistorialTurnos)
                  .HasForeignKey(e => e.IdUsuario)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.EstadoTurno)
                  .WithMany(es => es.HistorialTurnos)
                  .HasForeignKey(e => e.IdEstadoTurno)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Profesional>(entity =>
        {
            entity.HasOne(e => e.Especialidad)
                  .WithMany(es => es.Profesionales)
                  .HasForeignKey(e => e.IdEspecialidad)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed de datos
        builder.Entity<Especialidad>().HasData(
            new Especialidad { IdEspecialidad = 1, Nombre = "Salud Mental", Descripcion = "Psicología y psiquiatría" },
            new Especialidad { IdEspecialidad = 2, Nombre = "Medicina General", Descripcion = "Pediatría y clínica general" },
            new Especialidad { IdEspecialidad = 3, Nombre = "Terapias", Descripcion = "Fonoaudiología y terapia ocupacional" },
            new Especialidad { IdEspecialidad = 4, Nombre = "Nutrición", Descripcion = "Dietética y nutrición" },
            new Especialidad { IdEspecialidad = 5, Nombre = "Odontología", Descripcion = "Salud bucal" }
        );

        builder.Entity<EstadoTurno>().HasData(
            new EstadoTurno { IdEstadoTurno = 1, Nombre = "Pendiente", Descripcion = "Turno pendiente de atención" },
            new EstadoTurno { IdEstadoTurno = 2, Nombre = "Confirmado", Descripcion = "Turno confirmado por el profesional" },
            new EstadoTurno { IdEstadoTurno = 3, Nombre = "Completado", Descripcion = "Turno realizado exitosamente" },
            new EstadoTurno { IdEstadoTurno = 4, Nombre = "Cancelado", Descripcion = "Turno cancelado" }
        );
    }
}
