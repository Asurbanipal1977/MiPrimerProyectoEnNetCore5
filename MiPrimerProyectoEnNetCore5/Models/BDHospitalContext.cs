using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class BDHospitalContext : DbContext
    {
        public BDHospitalContext()
        {
        }

        public BDHospitalContext(DbContextOptions<BDHospitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Boton> Boton { get; set; }
        public virtual DbSet<Cita> Cita { get; set; }
        public virtual DbSet<CitaMedicamento> CitaMedicamento { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<Especialidad> Especialidad { get; set; }
        public virtual DbSet<EstadoCita> EstadoCita { get; set; }
        public virtual DbSet<FormaFarmaceutica> FormaFarmaceutica { get; set; }
        public virtual DbSet<HistorialCita> HistorialCita { get; set; }
        public virtual DbSet<Medicamento> Medicamento { get; set; }
        public virtual DbSet<Paciente> Paciente { get; set; }
        public virtual DbSet<Pagina> Pagina { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Sede> Sede { get; set; }
        public virtual DbSet<Sexo> Sexo { get; set; }
        public virtual DbSet<TipoSangre> TipoSangre { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<TipoUsuarioPagina> TipoUsuarioPagina { get; set; }
        public virtual DbSet<TipoUsuarioPaginaBoton> TipoUsuarioPaginaBoton { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=GIGABYTE-SABRE\\SQLEXPRESS;database=BDHospital;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boton>(entity =>
            {
                entity.HasKey(e => e.Iidboton)
                    .HasName("PK__BOTON__90C2312F68927075");

                entity.Property(e => e.Iidboton).HasColumnName("IIDBOTON");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.Iidcita)
                    .HasName("PK_Consulta");

                entity.Property(e => e.Iidcita).HasColumnName("IIDCITA");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcionenfermedad)
                    .HasColumnName("DESCRIPCIONENFERMEDAD")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Dfechacita)
                    .HasColumnName("DFECHACITA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dfechainicioenfermedad)
                    .HasColumnName("DFECHAINICIOENFERMEDAD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Diagnostico)
                    .HasColumnName("DIAGNOSTICO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estatura)
                    .HasColumnName("ESTATURA")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Examenfisico)
                    .HasColumnName("EXAMENFISICO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Examenlaboratorio)
                    .HasColumnName("EXAMENLABORATORIO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Iiddoctorasignado).HasColumnName("IIDDOCTORASIGNADO");

                entity.Property(e => e.Iidestadocita).HasColumnName("IIDESTADOCITA");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidsede).HasColumnName("IIDSEDE");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Peso)
                    .HasColumnName("PESO")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Precioatencion)
                    .HasColumnName("PRECIOATENCION")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Totalpagar)
                    .HasColumnName("TOTALPAGAR")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IiddoctorasignadoNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iiddoctorasignado)
                    .HasConstraintName("FK__Cita__IIDDOCTORA__01142BA1");

                entity.HasOne(d => d.IidestadocitaNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidestadocita)
                    .HasConstraintName("FK__Cita__IIDESTADOC__7E37BEF6");

                entity.HasOne(d => d.IidpersonaNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidpersona)
                    .HasConstraintName("FK__Cita__IIDPERSONA__2739D489");

                entity.HasOne(d => d.IidsedeNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidsede)
                    .HasConstraintName("FK__Cita__IIDSEDE__7F2BE32F");

                entity.HasOne(d => d.IidusuarioNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.Iidusuario)
                    .HasConstraintName("FK__Cita__IIDUSUARIO__7C4F7684");
            });

            modelBuilder.Entity<CitaMedicamento>(entity =>
            {
                entity.HasKey(e => e.Iidcitamedicamento)
                    .HasName("PK__citamedi__E4CCE514844E03D5");

                entity.Property(e => e.Iidcitamedicamento)
                    .HasColumnName("IIDCITAMEDICAMENTO")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.Iidcita).HasColumnName("IIDCITA");

                entity.Property(e => e.Iidmedicamento).HasColumnName("IIDMEDICAMENTO");

                entity.Property(e => e.Precio)
                    .HasColumnName("PRECIO")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IidcitaNavigation)
                    .WithMany(p => p.CitaMedicamento)
                    .HasForeignKey(d => d.Iidcita)
                    .HasConstraintName("FK__citamedic__IIDCI__03F0984C");

                entity.HasOne(d => d.IidmedicamentoNavigation)
                    .WithMany(p => p.CitaMedicamento)
                    .HasForeignKey(d => d.Iidmedicamento)
                    .HasConstraintName("FK__citamedic__IIDME__04E4BC85");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Iiddoctor);

                entity.Property(e => e.Iiddoctor).HasColumnName("IIDDOCTOR");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Fechacontrato)
                    .HasColumnName("FECHACONTRATO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iidespecialidad).HasColumnName("IIDESPECIALIDAD");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidsede).HasColumnName("IIDSEDE");

                entity.Property(e => e.Sueldo)
                    .HasColumnName("SUELDO")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IidespecialidadNavigation)
                    .WithMany(p => p.Doctor)
                    .HasForeignKey(d => d.Iidespecialidad)
                    .HasConstraintName("FK_Doctor_Especialidad");

                entity.HasOne(d => d.IidpersonaNavigation)
                    .WithMany(p => p.Doctor)
                    .HasForeignKey(d => d.Iidpersona)
                    .HasConstraintName("FK__Doctor__IIDPERSO__693CA210");

                entity.HasOne(d => d.IidsedeNavigation)
                    .WithMany(p => p.Doctor)
                    .HasForeignKey(d => d.Iidsede)
                    .HasConstraintName("FK_Doctor_Clinica");
            });

            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.Iidespecialidad)
                    .HasName("PK_TipoEspecialidad");

                entity.Property(e => e.Iidespecialidad).HasColumnName("IIDESPECIALIDAD");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoCita>(entity =>
            {
                entity.HasKey(e => e.Iidestado)
                    .HasName("PK__ESTACITA__CB0F471FE45B7978");

                entity.Property(e => e.Iidestado)
                    .HasColumnName("IIDESTADO")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Vdescripcion)
                    .HasColumnName("VDESCRIPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vnombre)
                    .HasColumnName("VNOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormaFarmaceutica>(entity =>
            {
                entity.HasKey(e => e.Iidformafarmaceutica);

                entity.Property(e => e.Iidformafarmaceutica).HasColumnName("IIDFORMAFARMACEUTICA");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HistorialCita>(entity =>
            {
                entity.HasKey(e => e.Iidhistorialcita)
                    .HasName("PK__Historia__35A9F434FABE77C7");

                entity.Property(e => e.Iidhistorialcita).HasColumnName("IIDHISTORIALCITA");

                entity.Property(e => e.Dfecha)
                    .HasColumnName("DFECHA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iidcita).HasColumnName("IIDCITA");

                entity.Property(e => e.Iidestado).HasColumnName("IIDESTADO");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Vobservacion)
                    .HasColumnName("VOBSERVACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IidcitaNavigation)
                    .WithMany(p => p.HistorialCita)
                    .HasForeignKey(d => d.Iidcita)
                    .HasConstraintName("FK__Historial__IIDCI__07C12930");

                entity.HasOne(d => d.IidestadoNavigation)
                    .WithMany(p => p.HistorialCita)
                    .HasForeignKey(d => d.Iidestado)
                    .HasConstraintName("FK__Historial__IIDES__08B54D69");

                entity.HasOne(d => d.IidusuarioNavigation)
                    .WithMany(p => p.HistorialCita)
                    .HasForeignKey(d => d.Iidusuario)
                    .HasConstraintName("FK__Historial__IIDUS__09A971A2");
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.HasKey(e => e.Iidmedicamento);

                entity.Property(e => e.Iidmedicamento).HasColumnName("IIDMEDICAMENTO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Concentracion)
                    .HasColumnName("CONCENTRACION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Iidformafarmaceutica).HasColumnName("IIDFORMAFARMACEUTICA");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Precio)
                    .HasColumnName("PRECIO")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Presentacion)
                    .HasColumnName("PRESENTACION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Stock).HasColumnName("STOCK");

                entity.HasOne(d => d.IidformafarmaceuticaNavigation)
                    .WithMany(p => p.Medicamento)
                    .HasForeignKey(d => d.Iidformafarmaceutica)
                    .HasConstraintName("FK__Medicamen__IIDFO__36B12243");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.Iidpaciente)
                    .HasName("PK_Pacientes_1");

                entity.Property(e => e.Iidpaciente).HasColumnName("IIDPACIENTE");

                entity.Property(e => e.Alergias)
                    .HasColumnName("ALERGIAS")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Antecedentesquirurgicos)
                    .HasColumnName("ANTECEDENTESQUIRURGICOS")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Cuadrovacunas)
                    .HasColumnName("CUADROVACUNAS")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Enfermedadescronicas)
                    .HasColumnName("ENFERMEDADESCRONICAS")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidtiposangre).HasColumnName("IIDTIPOSANGRE");

                entity.HasOne(d => d.IidtiposangreNavigation)
                    .WithMany(p => p.Paciente)
                    .HasForeignKey(d => d.Iidtiposangre)
                    .HasConstraintName("FK__Paciente__IIDTIP__17036CC0");
            });

            modelBuilder.Entity<Pagina>(entity =>
            {
                entity.HasKey(e => e.Iidpagina)
                    .HasName("PK__Pagina__8E759E4E24D07BF3");

                entity.Property(e => e.Iidpagina).HasColumnName("IIDPAGINA");

                entity.Property(e => e.Accion)
                    .HasColumnName("ACCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Controlador)
                    .HasColumnName("CONTROLADOR")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mensaje)
                    .HasColumnName("MENSAJE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Iidpersona)
                    .HasName("PK_Paciente");

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Apmaterno)
                    .HasColumnName("APMATERNO")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Appaterno)
                    .HasColumnName("APPATERNO")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Bdoctor).HasColumnName("BDOCTOR");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Bpaciente).HasColumnName("BPACIENTE");

                entity.Property(e => e.Btieneusuario).HasColumnName("BTIENEUSUARIO");

                entity.Property(e => e.Direccion)
                    .HasColumnName("DIRECCION")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fechanacimiento)
                    .HasColumnName("FECHANACIMIENTO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Foto)
                    .HasColumnName("FOTO")
                    .IsUnicode(false);

                entity.Property(e => e.Iidsexo).HasColumnName("IIDSEXO");

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefonocelular)
                    .HasColumnName("TELEFONOCELULAR")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Telefonofijo)
                    .HasColumnName("TELEFONOFIJO")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IidsexoNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.Iidsexo)
                    .HasConstraintName("FK_Paciente_Sexo");

                entity.HasOne(d => d.IidusuarioNavigation)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.Iidusuario)
                    .HasConstraintName("FK__Persona__IIDUSUA__282DF8C2");
            });

            modelBuilder.Entity<Sede>(entity =>
            {
                entity.HasKey(e => e.Iidsede)
                    .HasName("PK_Clinica");

                entity.Property(e => e.Iidsede).HasColumnName("IIDSEDE");

                entity.Property(e => e.Bhabilitado)
                    .HasColumnName("BHABILITADO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Direccion)
                    .HasColumnName("DIRECCION")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sexo>(entity =>
            {
                entity.HasKey(e => e.Iidsexo);

                entity.Property(e => e.Iidsexo).HasColumnName("IIDSEXO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoSangre>(entity =>
            {
                entity.HasKey(e => e.Iidtiposangre);

                entity.Property(e => e.Iidtiposangre).HasColumnName("IIDTIPOSANGRE");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasKey(e => e.Iidtipousuario);

                entity.Property(e => e.Iidtipousuario).HasColumnName("IIDTIPOUSUARIO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoUsuarioPagina>(entity =>
            {
                entity.HasKey(e => e.Iidtipousuariopagina)
                    .HasName("PK__TipoUsua__59B815B9209568AC");

                entity.Property(e => e.Iidtipousuariopagina).HasColumnName("IIDTIPOUSUARIOPAGINA");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Iidpagina).HasColumnName("IIDPAGINA");

                entity.Property(e => e.Iidtipousuario).HasColumnName("IIDTIPOUSUARIO");

                entity.Property(e => e.Iidvista).HasColumnName("IIDVISTA");

                entity.HasOne(d => d.IidpaginaNavigation)
                    .WithMany(p => p.TipoUsuarioPagina)
                    .HasForeignKey(d => d.Iidpagina)
                    .HasConstraintName("FK__TipoUsuar__IIDPA__75A278F5");

                entity.HasOne(d => d.IidtipousuarioNavigation)
                    .WithMany(p => p.TipoUsuarioPagina)
                    .HasForeignKey(d => d.Iidtipousuario)
                    .HasConstraintName("FK__TipoUsuar__IIDTI__74AE54BC");
            });

            modelBuilder.Entity<TipoUsuarioPaginaBoton>(entity =>
            {
                entity.HasKey(e => e.Iidtipousuariopaginaboton)
                    .HasName("PK__TipoUsua__7882D20C06997AB0");

                entity.Property(e => e.Iidtipousuariopaginaboton).HasColumnName("IIDTIPOUSUARIOPAGINABOTON");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Iidboton).HasColumnName("IIDBOTON");

                entity.Property(e => e.Iidtipousuariopagina).HasColumnName("IIDTIPOUSUARIOPAGINA");

                entity.HasOne(d => d.IidbotonNavigation)
                    .WithMany(p => p.TipoUsuarioPaginaBoton)
                    .HasForeignKey(d => d.Iidboton)
                    .HasConstraintName("FK__TipoUsuar__IIDBO__797309D9");

                entity.HasOne(d => d.IidtipousuariopaginaNavigation)
                    .WithMany(p => p.TipoUsuarioPaginaBoton)
                    .HasForeignKey(d => d.Iidtipousuariopagina)
                    .HasConstraintName("FK__TipoUsuar__IIDTI__787EE5A0");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Iidusuario);

                entity.Property(e => e.Iidusuario).HasColumnName("IIDUSUARIO");

                entity.Property(e => e.Bhabilitado).HasColumnName("BHABILITADO");

                entity.Property(e => e.Contraseña)
                    .HasColumnName("CONTRASEÑA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Iidpersona).HasColumnName("IIDPERSONA");

                entity.Property(e => e.Iidtipousuario).HasColumnName("IIDTIPOUSUARIO");

                entity.Property(e => e.Nombreusuario)
                    .HasColumnName("NOMBREUSUARIO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IidpersonaNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Iidpersona)
                    .HasConstraintName("FK__Usuario__IIDPERS__14270015");

                entity.HasOne(d => d.IidtipousuarioNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Iidtipousuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usuario__IIDTIPO__68487DD7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
