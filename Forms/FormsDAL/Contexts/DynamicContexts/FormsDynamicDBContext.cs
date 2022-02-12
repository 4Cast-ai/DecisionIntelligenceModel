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

        public virtual DbSet<DynamicActivityTrace> DynamicActivityTraces { get; set; } = null!;
        public virtual DbSet<DynamicEntityType> DynamicEntityTypes { get; set; } = null!;
        public virtual DbSet<DynamicForm> DynamicForms { get; set; } = null!;
        public virtual DbSet<DynamicFormStatus> DynamicFormStatuses { get; set; } = null!;
        public virtual DbSet<DynamicRecordStatus> DynamicRecordStatuses { get; set; } = null!;
        public virtual DbSet<DynamicScore> DynamicScores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DynamicActivityTrace>(entity =>
            {
                entity.HasKey(e => e.ActivityGuid)
                    .HasName("ActivityTrace_PK");

                entity.ToTable("DynamicActivityTrace");

                entity.HasIndex(e => e.RecordStatusCode, "IX_FormsActivityTrace_RecordStatusCode");

                entity.Property(e => e.ActivityGuid).HasMaxLength(50);

                entity.Property(e => e.ActivityEndDate).HasMaxLength(14);

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
                    .WithMany(p => p.DynamicActivityTraces)
                    .HasForeignKey(d => d.RecordStatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RecordStatusCode_FK");
            });

            modelBuilder.Entity<DynamicEntityType>(entity =>
            {
                entity.HasKey(e => e.EntityTypeCode)
                    .HasName("EvaluatedType_pkey");

                entity.ToTable("DynamicEntityType");

                entity.Property(e => e.EntityTypeCode).ValueGeneratedNever();

                entity.Property(e => e.EntityTypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<DynamicForm>(entity =>
            {
                entity.HasKey(e => e.FormGuid)
                    .HasName("Form_pkey");

                entity.ToTable("DynamicForm");

                entity.Property(e => e.FormGuid).HasMaxLength(50);

                entity.Property(e => e.ActivityGuid).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasMaxLength(14);

                entity.Property(e => e.EvaluatedGuid).HasMaxLength(50);

                entity.Property(e => e.EvaluatorGuid).HasMaxLength(50);

                entity.Property(e => e.FormTemplateGuid).HasMaxLength(50);

                entity.Property(e => e.LastUpdateUserGuid).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasMaxLength(14);

                entity.HasOne(d => d.EvaluatedTypeNavigation)
                    .WithMany(p => p.DynamicFormEvaluatedTypeNavigations)
                    .HasForeignKey(d => d.EvaluatedType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EvaluatedType");

                entity.HasOne(d => d.EvaluatorTypeNavigation)
                    .WithMany(p => p.DynamicFormEvaluatorTypeNavigations)
                    .HasForeignKey(d => d.EvaluatorType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EvluatorTypeFK");

                entity.HasOne(d => d.FormStatusNavigation)
                    .WithMany(p => p.DynamicForms)
                    .HasForeignKey(d => d.FormStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FormStatus_FK");
            });

            modelBuilder.Entity<DynamicFormStatus>(entity =>
            {
                entity.HasKey(e => e.FormStatusCode)
                    .HasName("FormStatus_pkey");

                entity.ToTable("DynamicFormStatus");

                entity.Property(e => e.FormStatusCode).ValueGeneratedNever();

                entity.Property(e => e.FormStatusName).HasMaxLength(255);
            });

            modelBuilder.Entity<DynamicRecordStatus>(entity =>
            {
                entity.HasKey(e => e.RecordStatusCode)
                    .HasName("RecordStatus_pkey");

                entity.ToTable("DynamicRecordStatus");

                entity.Property(e => e.RecordStatusCode).ValueGeneratedNever();

                entity.Property(e => e.RecordStatusName).HasMaxLength(255);
            });

            modelBuilder.Entity<DynamicScore>(entity =>
            {
                entity.HasKey(e => e.DynamicScoresID)
                    .HasName("FormComponent_pkey");

                entity.Property(e => e.DynamicScoresID)
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, 99999999L);

                entity.Property(e => e.Comment).HasColumnType("character varying");

                entity.Property(e => e.FormGuid).HasMaxLength(50);

                entity.Property(e => e.ModelComponentGuid).HasMaxLength(50);

                entity.HasOne(d => d.FormGu)
                    .WithMany(p => p.DynamicScores)
                    .HasForeignKey(d => d.FormGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FormGuid_FK");
            });

            modelBuilder.HasSequence("ActivityTraceIdSeq").StartsAt(3);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
