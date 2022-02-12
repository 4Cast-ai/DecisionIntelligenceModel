using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Entities;
//using Model.Data;

namespace FormsDal.Contexts
{
    public partial class FormsManageDBContext : BaseContext
    {
        public FormsManageDBContext()
        {
        }

        public FormsManageDBContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FormsActivityTrace> FormsActivityTraces { get; set; } = null!;
        public virtual DbSet<FormsEvaluatedType> FormsEvaluatedTypes { get; set; } = null!;
        public virtual DbSet<FormsEvaluatorType> FormsEvaluatorTypes { get; set; } = null!;
        public virtual DbSet<FormsFormElementType> FormsFormElementTypes { get; set; } = null!;
        public virtual DbSet<FormsMeasureUnitType> FormsMeasureUnitTypes { get; set; } = null!;
        public virtual DbSet<FormsObjectiveType> FormsObjectiveTypes { get; set; } = null!;
        public virtual DbSet<FormsRecordStatus> FormsRecordStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormsActivityTrace>(entity =>
            {
                entity.HasKey(e => e.ActivityTraceId)
                    .HasName("ActivityTrace_pkey");

                entity.ToTable("FormsActivityTrace");

                entity.Property(e => e.ActivityEndDate).HasMaxLength(14);

                entity.Property(e => e.ActivityGuid).HasMaxLength(50);

                entity.Property(e => e.ActivityName).HasMaxLength(255);

                entity.Property(e => e.ActivityStartDate).HasMaxLength(14);

                entity.Property(e => e.CanSubmitOnce_).HasColumnName("CanSubmitOnce ");

                entity.Property(e => e.CreationDate).HasMaxLength(14);

                entity.Property(e => e.EvaluatedAndEvaluators).HasColumnType("json");

                entity.Property(e => e.Forms).HasColumnType("json");

                entity.Property(e => e.FormsDBName).HasMaxLength(250);

                entity.Property(e => e.FromEffectDate).HasMaxLength(14);

                entity.Property(e => e.ToEffectDate).HasMaxLength(14);

                entity.Property(e => e.UpdateDate).HasMaxLength(14);

                entity.Property(e => e.UpdateUserId).HasMaxLength(50);

                entity.HasOne(d => d.RecordStatusCodeNavigation)
                    .WithMany(p => p.FormsActivityTraces)
                    .HasForeignKey(d => d.RecordStatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RecordStatusCode_FK");
            });

            modelBuilder.Entity<FormsEvaluatedType>(entity =>
            {
                entity.HasKey(e => e.EvaluatedTypeCode)
                    .HasName("EvaluatedType_pkey");

                entity.ToTable("FormsEvaluatedType");

                entity.Property(e => e.EvaluatedTypeCode).ValueGeneratedNever();

                entity.Property(e => e.EvaluatedTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<FormsEvaluatorType>(entity =>
            {
                entity.HasKey(e => e.EvaluatorTypeCode)
                    .HasName("EvaluatorType_pkey");

                entity.ToTable("FormsEvaluatorType");

                entity.Property(e => e.EvaluatorTypeCode).ValueGeneratedNever();

                entity.Property(e => e.EvaluatorTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<FormsFormElementType>(entity =>
            {
                entity.HasKey(e => e.FormElementTypeCode)
                    .HasName("FormElementType_pkey");

                entity.ToTable("FormsFormElementType");

                entity.Property(e => e.FormElementTypeCode).ValueGeneratedNever();

                entity.Property(e => e.FormElementTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<FormsMeasureUnitType>(entity =>
            {
                entity.HasKey(e => e.MeasureUnitTypeCode)
                    .HasName("MeasureUnitType_pkey");

                entity.ToTable("FormsMeasureUnitType");

                entity.Property(e => e.MeasureUnitTypeCode).ValueGeneratedNever();

                entity.Property(e => e.MeasureUnitTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<FormsObjectiveType>(entity =>
            {
                entity.HasKey(e => e.ObjectiveTypeCode)
                    .HasName("ObjectiveType_pkey");

                entity.ToTable("FormsObjectiveType");

                entity.Property(e => e.ObjectiveTypeCode).ValueGeneratedNever();

                entity.Property(e => e.ObjectiveTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<FormsRecordStatus>(entity =>
            {
                entity.HasKey(e => e.RecordStatusCode)
                    .HasName("RecordStatus_pkey");

                entity.ToTable("FormsRecordStatus");

                entity.Property(e => e.RecordStatusCode).ValueGeneratedNever();

                entity.Property(e => e.RecordStatusName).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
