using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Entities;

namespace FormsDal.Contexts
{
    public partial class FormsDynamicDBContext : BaseContext
    {
        public FormsDynamicDBContext()
        {
        }

        public FormsDynamicDBContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DynamicForm> DynamicForms { get; set; } = null!;
        public virtual DbSet<DynamicFormComponent> DynamicFormComponents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DynamicForm>(entity =>
            {
                entity.HasKey(e => e.FormRecordId)
                    .HasName("Form_pkey");

                entity.ToTable("DynamicForm");

                entity.Property(e => e.FormRecordId).ValueGeneratedNever();

                entity.Property(e => e.ActivityGuid).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasMaxLength(14);

                entity.Property(e => e.EvaluatedGuid).HasMaxLength(50);

                entity.Property(e => e.EvaluatorGuid).HasMaxLength(50);

                entity.Property(e => e.FormGuid).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasMaxLength(14);
            });

            modelBuilder.Entity<DynamicFormComponent>(entity =>
            {
                entity.HasKey(e => e.FormComponentRecordId)
                    .HasName("FormComponent_pkey");

                entity.ToTable("DynamicFormComponent");

                entity.Property(e => e.FormComponentRecordId).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasColumnType("character varying");

                entity.Property(e => e.ModelComponentGuid).HasMaxLength(50);

                entity.Property(e => e.Score).HasMaxLength(50);

                entity.HasOne(d => d.FormRecord)
                    .WithMany(p => p.DynamicFormComponents)
                    .HasForeignKey(d => d.FormRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FormRecordId_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
