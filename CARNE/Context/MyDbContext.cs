using System;
using System.Collections.Generic;
using CARNE.Models;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Alerta> Alertas { get; set; }

    public virtual DbSet<Animale> Animales { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Establecimiento> Establecimientos { get; set; }

    public virtual DbSet<Inspeccione> Inspecciones { get; set; }

    public virtual DbSet<Irregularidad> Irregularidads { get; set; }

    public virtual DbSet<ItemsVerificacion> ItemsVerificacions { get; set; }

    public virtual DbSet<ListaVerificacion> ListaVerificacions { get; set; }

    public virtual DbSet<LotesProducto> LotesProductos { get; set; }

    public virtual DbSet<Normativa> Normativas { get; set; }

    public virtual DbSet<ResultadosInspeccion> ResultadosInspeccions { get; set; }

    public virtual DbSet<RolePermiso> RolePermisos { get; set; }

    public virtual DbSet<SancionIrregularidad> SancionIrregularidads { get; set; }

    public virtual DbSet<Sancione> Sanciones { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:sqlcarnicos.database.windows.net,1433;Initial Catalog=CarnicosBD;Persist Security Info=False;User ID=ciprian123;Password=Miguel123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("PK__Admin__B2C3ADE53E68961D");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Username, "UQ__Admin__F3DBC57287E53781").IsUnique();

            entity.Property(e => e.IdAdmin).HasColumnName("idAdmin");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaIngreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaIngreso");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Alerta>(entity =>
        {
            entity.HasKey(e => e.IdAlerta).HasName("PK__Alertas__D0995427C10CEF68");

            entity.Property(e => e.IdAlerta).HasColumnName("idAlerta");
            entity.Property(e => e.Destinatarios)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("destinatarios");
            entity.Property(e => e.EstadoAlerta)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estadoAlerta");
            entity.Property(e => e.FechaGenerada)
                .HasColumnType("datetime")
                .HasColumnName("fechaGenerada");
            entity.Property(e => e.IdIrregularidad).HasColumnName("idIrregularidad");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("mensaje");

            entity.HasOne(d => d.IdIrregularidadNavigation).WithMany(p => p.Alerta)
                .HasForeignKey(d => d.IdIrregularidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Alertas__idIrreg__3C34F16F");
        });

        modelBuilder.Entity<Animale>(entity =>
        {
            entity.HasKey(e => e.IdAnimal).HasName("PK__Animales__0276B503C7CBFA28");

            entity.Property(e => e.IdAnimal).HasColumnName("idAnimal");
            entity.Property(e => e.Especie)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("especie");
            entity.Property(e => e.FechaSacrificio)
                .HasColumnType("datetime")
                .HasColumnName("fechaSacrificio");
            entity.Property(e => e.IdEstablecimientoSacrificio).HasColumnName("idEstablecimientoSacrificio");
            entity.Property(e => e.IdentificacionAnimal)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("identificacionAnimal");

            entity.HasOne(d => d.IdEstablecimientoSacrificioNavigation).WithMany(p => p.Animales)
                .HasForeignKey(d => d.IdEstablecimientoSacrificio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Animales__idEsta__1DB06A4F");

            entity.HasMany(d => d.IdLotes).WithMany(p => p.IdAnimals)
                .UsingEntity<Dictionary<string, object>>(
                    "AnimalesLote",
                    r => r.HasOne<LotesProducto>().WithMany()
                        .HasForeignKey("IdLote")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AnimalesL__idLot__2180FB33"),
                    l => l.HasOne<Animale>().WithMany()
                        .HasForeignKey("IdAnimal")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AnimalesL__idAni__208CD6FA"),
                    j =>
                    {
                        j.HasKey("IdAnimal", "IdLote").HasName("PK__Animales__A3CFAAFF6EEDDA5B");
                        j.ToTable("AnimalesLotes");
                        j.IndexerProperty<int>("IdAnimal").HasColumnName("idAnimal");
                        j.IndexerProperty<int>("IdLote").HasColumnName("idLote");
                    });
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.IdDocumento).HasName("PK__Document__572A36FC42C2B7D6");

            entity.Property(e => e.IdDocumento).HasColumnName("idDocumento");
            entity.Property(e => e.FechaEmision)
                .HasColumnType("datetime")
                .HasColumnName("fechaEmision");
            entity.Property(e => e.FechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaVencimiento");
            entity.Property(e => e.IdLote).HasColumnName("idLote");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipoDocumento");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documento__idLot__3F115E1A");
        });

        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity.HasKey(e => e.IdEstablecimiento).HasName("PK__Establec__A9B5B72F1EB0E388");

            entity.Property(e => e.IdEstablecimiento).HasColumnName("idEstablecimiento");
            entity.Property(e => e.CapacidadOperativa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("capacidadOperativa");
            entity.Property(e => e.Comerciales)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comerciales");
            entity.Property(e => e.Direccion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.EstadoEstablecimiento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estadoEstablecimiento");
            entity.Property(e => e.LicenciasCertificaciones)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("licenciasCertificaciones");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PeriodoVolumen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("periodoVolumen");
            entity.Property(e => e.Riesgo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipoOperacion");
            entity.Property(e => e.TipoProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipoProducto");
            entity.Property(e => e.UnidadVolumen)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unidadVolumen");
            entity.Property(e => e.VolumenProcesado)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("volumenProcesado");
        });

        modelBuilder.Entity<Inspeccione>(entity =>
        {
            entity.HasKey(e => e.IdInspeccion).HasName("PK__Inspecci__A5F326E1EF116B89");

            entity.Property(e => e.IdInspeccion).HasColumnName("idInspeccion");
            entity.Property(e => e.FechaInspeccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaInspeccion");
            entity.Property(e => e.IdAdmin).HasColumnName("idAdmin");
            entity.Property(e => e.IdAdminInspector).HasColumnName("idAdminInspector");
            entity.Property(e => e.IdEstablecimiento).HasColumnName("idEstablecimiento");
            entity.Property(e => e.IdSolicitud).HasColumnName("idSolicitud");
            entity.Property(e => e.Prioridad).HasColumnName("prioridad");
            entity.Property(e => e.Resultado)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.Inspecciones)
                .HasForeignKey(d => d.IdAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspeccio__idAdm__29221CFB");

            // Nueva configuración para IdAdminInspector
            entity.HasOne<Admin>()
                .WithMany()
                .HasForeignKey(d => d.IdAdminInspector)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdEstablecimientoNavigation).WithMany(p => p.Inspecciones)
                .HasForeignKey(d => d.IdEstablecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspeccio__idEst__2739D489");

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.Inspecciones)
                .HasForeignKey(d => d.IdSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false)
                .HasConstraintName("FK__Inspeccio__idSol__282DF8C2");
        });

        modelBuilder.Entity<Irregularidad>(entity =>
        {
            entity.HasKey(e => e.IdIrregularidad).HasName("PK__Irregula__5591F75316BE9982");

            entity.ToTable("Irregularidad");

            entity.Property(e => e.IdIrregularidad).HasColumnName("idIrregularidad");
            entity.Property(e => e.DescripcionIrregularidad)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcionIrregularidad");
            entity.Property(e => e.FechaDetectada)
                .HasColumnType("datetime")
                .HasColumnName("fechaDetectada");
            entity.Property(e => e.IdEstablecimiento).HasColumnName("idEstablecimiento");
            entity.Property(e => e.IdLote).HasColumnName("idLote");
            entity.Property(e => e.IdResultadoInspeccion).HasColumnName("idResultadoInspeccion");
            entity.Property(e => e.NivelGravedad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nivelGravedad");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdEstablecimientoNavigation).WithMany(p => p.Irregularidads)
                .HasForeignKey(d => d.IdEstablecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Irregular__idEst__37703C52");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Irregularidads)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Irregular__idLot__3864608B");

            entity.HasOne(d => d.IdResultadoInspeccionNavigation).WithMany(p => p.Irregularidads)
                .HasForeignKey(d => d.IdResultadoInspeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Irregular__idRes__395884C4");
        });

        modelBuilder.Entity<ItemsVerificacion>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__ItemsVer__AD19426894AEE37B");

            entity.ToTable("ItemsVerificacion");

            entity.Property(e => e.IdItem).HasColumnName("idItem");
            entity.Property(e => e.CriterioCumplimiento)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("criterioCumplimiento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdLista).HasColumnName("idLista");

            entity.HasOne(d => d.IdListaNavigation).WithMany(p => p.ItemsVerificacions)
                .HasForeignKey(d => d.IdLista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemsVeri__idLis__2FCF1A8A");
        });

        modelBuilder.Entity<ListaVerificacion>(entity =>
        {
            entity.HasKey(e => e.IdLista).HasName("PK__ListaVer__6C8A0FE56459A74E");

            entity.ToTable("ListaVerificacion");

            entity.Property(e => e.IdLista).HasColumnName("idLista");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdNormativa).HasColumnName("idNormativa");
            entity.Property(e => e.NombreLista)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreLista");

            entity.HasOne(d => d.IdNormativaNavigation).WithMany(p => p.ListaVerificacions)
                .HasForeignKey(d => d.IdNormativa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaVeri__idNor__2CF2ADDF");
        });

        modelBuilder.Entity<LotesProducto>(entity =>
        {
            entity.HasKey(e => e.IdLote).HasName("PK__LotesPro__1B91FFCB0DD5ADFA");

            entity.Property(e => e.IdLote).HasColumnName("idLote");
            entity.Property(e => e.CodigoLote)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("codigoLote");
            entity.Property(e => e.DescripcionProducto)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcionProducto");
            entity.Property(e => e.DestinoFinal)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("destinoFinal");
            entity.Property(e => e.FechaProduccion)
                .HasColumnType("datetime")
                .HasColumnName("fechaProduccion");
            entity.Property(e => e.IdEstablecimiento).HasColumnName("idEstablecimiento");

            entity.HasOne(d => d.IdEstablecimientoNavigation).WithMany(p => p.LotesProductos)
                .HasForeignKey(d => d.IdEstablecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LotesProd__idEst__1AD3FDA4");
        });

        modelBuilder.Entity<Normativa>(entity =>
        {
            entity.HasKey(e => e.IdNormativa).HasName("PK__Normativ__531225EA3D23BF5B");

            entity.Property(e => e.IdNormativa).HasColumnName("idNormativa");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaAdmision)
                .HasColumnType("datetime")
                .HasColumnName("fechaAdmision");
            entity.Property(e => e.FechaVigencia)
                .HasColumnType("datetime")
                .HasColumnName("fechaVigencia");
            entity.Property(e => e.NombreNormativa)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreNormativa");
            entity.Property(e => e.Version)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("version");
        });

        modelBuilder.Entity<ResultadosInspeccion>(entity =>
        {
            entity.HasKey(e => e.IdResultado).HasName("PK__Resultad__CE8B42C441D939A2");

            entity.ToTable("ResultadosInspeccion");

            entity.Property(e => e.IdResultado).HasColumnName("idResultado");
            entity.Property(e => e.Cumple).HasColumnName("cumple");
            entity.Property(e => e.IdInspeccion).HasColumnName("idInspeccion");
            entity.Property(e => e.IdItem).HasColumnName("idItem");
            entity.Property(e => e.IdLista).HasColumnName("idLista");
            entity.Property(e => e.Observacion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("observacion");

            entity.HasOne(d => d.IdInspeccionNavigation).WithMany(p => p.ResultadosInspeccions)
                .HasForeignKey(d => d.IdInspeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resultado__idIns__32AB8735");

            entity.HasOne(d => d.IdItemNavigation).WithMany(p => p.ResultadosInspeccions)
                .HasForeignKey(d => d.IdItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resultado__idIte__3493CFA7");

            entity.HasOne(d => d.IdListaNavigation).WithMany(p => p.ResultadosInspeccions)
                .HasForeignKey(d => d.IdLista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resultado__idLis__339FAB6E");
        });

        modelBuilder.Entity<RolePermiso>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__RolePerm__2A49584C247CBE5B");

            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SancionIrregularidad>(entity =>
        {
            entity.HasKey(e => new { e.IdIrregularidad, e.IdSancion }).HasName("PK__SancionI__487FB804B9282385");

            entity.ToTable("SancionIrregularidad");

            entity.Property(e => e.IdIrregularidad).HasColumnName("idIrregularidad");
            entity.Property(e => e.IdSancion).HasColumnName("idSancion");
            entity.Property(e => e.EstadoSancion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estadoSancion");
            entity.Property(e => e.FechaAplicada)
                .HasColumnType("datetime")
                .HasColumnName("fechaAplicada");
            entity.Property(e => e.FechaResolution)
                .HasColumnType("datetime")
                .HasColumnName("fechaResolution");

            entity.HasOne(d => d.IdIrregularidadNavigation).WithMany(p => p.SancionIrregularidads)
                .HasForeignKey(d => d.IdIrregularidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SancionIr__idIrr__41EDCAC5");

            entity.HasOne(d => d.IdSancionNavigation).WithMany(p => p.SancionIrregularidads)
                .HasForeignKey(d => d.IdSancion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SancionIr__idSan__42E1EEFE");
        });

        modelBuilder.Entity<Sancione>(entity =>
        {
            entity.HasKey(e => e.IdSancion).HasName("PK__Sancione__DEE4F57097B76D17");

            entity.Property(e => e.IdSancion).HasColumnName("idSancion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud).HasName("PK__Solicitu__D801DDB84A4E5364");

            entity.ToTable("Solicitud");

            entity.Property(e => e.IdSolicitud).HasColumnName("idSolicitud");
            entity.Property(e => e.EstadoSolicitud)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaAdmitida)
                .HasColumnType("datetime")
                .HasColumnName("fechaAdmitida");
            entity.Property(e => e.FechaAprobada)
                .HasColumnType("datetime")
                .HasColumnName("fechaAprobada");
            entity.Property(e => e.IdUsuarioCliente).HasColumnName("idUsuarioCliente");

            entity.HasOne(d => d.IdUsuarioClienteNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.IdUsuarioCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Solicitud__idUsu__245D67DE");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A6D39B1ECB");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Username, "UQ__Usuario__F3DBC5725DC9BAB7").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaIngreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaIngreso");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
