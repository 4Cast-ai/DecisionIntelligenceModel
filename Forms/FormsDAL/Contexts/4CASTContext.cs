using System;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Entities;

namespace Model.Entities
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
                : base(options)
        {
        }

        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<ActivityTemplate> ActivityTemplate { get; set; }
        public virtual DbSet<ActivityTemplateDescription> ActivityTemplateDescription { get; set; }
        public virtual DbSet<AtInFt> AtInFt { get; set; }
        public virtual DbSet<CalculateScore> CalculateScore { get; set; }
        public virtual DbSet<CalenderRollup> CalenderRollup { get; set; }
        //public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<ConvertionTable> ConvertionTable { get; set; }
        public virtual DbSet<Description> Description { get; set; }
        public virtual DbSet<EntityType> EntityType { get; set; }
        public virtual DbSet<EntityDescription> EntityDescription { get; set; }
        //public virtual DbSet<EstimatedOrganizationObject> EstimatedOrganizationObject { get; set; }
        public virtual DbSet<ActivityEntity> ActivityEntity { get; set; }
        public virtual DbSet<ActivityEstimator> ActivityEstimator { get; set; }

        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<FormElement> FormElement { get; set; }
        public virtual DbSet<FormElementConnection> FormElementConnection { get; set; }
        public virtual DbSet<FormElementType> FormElementType { get; set; }
        public virtual DbSet<FormStatus> FormStatus { get; set; }
        public virtual DbSet<FormTemplate> FormTemplate { get; set; }
        public virtual DbSet<FormTemplateStructure> FormTemplateStructure { get; set; }
        public virtual DbSet<MeasuringUnit> MeasuringUnit { get; set; }
        public virtual DbSet<ModelComponent> ModelComponent { get; set; }
        public virtual DbSet<ModelComponentSource> ModelComponentSource { get; set; }
        public virtual DbSet<ModelComponentStatus> ModelComponentStatus { get; set; }
        public virtual DbSet<ModelComponentType> ModelComponentType { get; set; }
        public virtual DbSet<ModelDescription> ModelDescription { get; set; }
        public virtual DbSet<ModelStructure> ModelStructure { get; set; }
        public virtual DbSet<OrgModelPolygon> OrgModelPolygon { get; set; }
        //public virtual DbSet<OrganizationObject> OrganizationObject { get; set; }
        //public virtual DbSet<OrganizationObjectConnection> OrganizationObjectConnection { get; set; }

        //public virtual DbSet<EntityActivityTemplate> EntityActivityTemplate { get; set; }

        //public virtual DbSet<OrganizationStructure> OrganizationStructure { get; set; }
        public virtual DbSet<OutSourceScore> OutSourceScore { get; set; }
        public virtual DbSet<PermissionTypes> PermissionTypes { get; set; }
        public virtual DbSet<ReportType> ReportType { get; set; }
        public virtual DbSet<RoleItems> RoleItems { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RollupMethod> RollupMethod { get; set; }
        public virtual DbSet<SavedReportData> SavedReportData { get; set; }
        public virtual DbSet<SavedReports> SavedReports { get; set; }
        public virtual DbSet<Score> Score { get; set; }
        public virtual DbSet<SystemJobTitles> SystemJobTitles { get; set; }
        public virtual DbSet<TemplateSettings> TemplateSettings { get; set; }

        public virtual DbSet<Threshold> Threshold { get; set; }
        public virtual DbSet<ThresholdOriginCondition> ThresholdOriginCondition { get; set; }
        public virtual DbSet<ThresholdsDestinationCondition> ThresholdDestinationCondition { get; set; }
        public virtual DbSet<ThresholdLevels> ThresholdLevels { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPreference> UserPreference { get; set; }

        public virtual DbSet<UserType> UserType { get; set; }

        public virtual DbSet<ActivityFile> ActivityFile { get; set; }
        public virtual DbSet<OrganizationUnion> OrganizationUnion { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }


        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseNpgsql("User ID=postgres;Password=123456;Server=192.168.12.15;Port=5432;Database=CRPM_Moshe;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.ActivityGuid)
                    .HasName("Activity_pkey");

                entity.Property(e => e.ActivityGuid)
                    //.HasColumnName("activity_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ActivityTemplateGuid)
                    //.HasColumnName("activity_template_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate)
                    //.HasColumnName("create_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    //.HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.EndDate)
                    //.HasColumnName("end_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    //.HasColumnName("name")
                    .HasMaxLength(255);

                //entity.Property(e => e.OrgObjGuid)
                //    //.HasColumnName("org_obj_guid")
                //    .HasMaxLength(50);

                entity.Property(e => e.StartDate)
                    //.HasColumnName("start_date")
                    .HasMaxLength(14)
                    .IsFixedLength();


                //entity.Property(e => e.)
                //  //.HasColumnName("name")
                //  .HasMaxLength(255);
                //entity.Property(e => e.EstimatedUnits)
                entity.HasOne(d => d.ActivityTemplateGu)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.ActivityTemplateGuid)
                    .HasConstraintName("FK_Activities_Activity_template");



            });

            modelBuilder.Entity<ActivityTemplate>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateGuid)
                    .HasName("PK_Activity_Template");

                entity.ToTable("ActivityTemplate");

                entity.Property(e => e.ActivityTemplateGuid)
                //.HasColumnName("activity_template_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.EntityType);

                entity.Property(e => e.CreateDate)
                    //.HasColumnName("create_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    //.HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    //.HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ProfessionalRecommendations)
                    //.HasColumnName("professional_recommendations")
                    .HasMaxLength(255);
                entity.Property(e => e.IsPoll);

                entity.HasOne(d => d.EntityTypeId)
               .WithMany(p => p.ActivityTemplate)
               .HasForeignKey(d => d.EntityType)
               .HasConstraintName("FK_ActivityTemplate_Entity_Type");
            });


            modelBuilder.Entity<ActivityTemplateDescription>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateDescriptionId)
                    .HasName("PK_Activity_Template_Description");

                entity.ToTable("ActivityTemplateDescription");

                entity.Property(e => e.ActivityTemplateDescriptionId).UseIdentityColumn();

                entity.Property(e => e.ActivityTemplateGuid).IsRequired().HasMaxLength(50);

                entity.HasOne(d => d.ActivityTemplateGu)
                  .WithMany(p => p.ActivityTemplateDescription)
                  .HasForeignKey(d => d.ActivityTemplateGuid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_ActivityTemplateDescription_ActivityTemplate");

                entity.HasOne(d => d.DescriptionGu)
                .WithMany(p => p.ActivityTemplateDescription)
                .HasForeignKey(d => d.DescriptionGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityTemplateDescription_Description");

            });


            modelBuilder.Entity<AtInFt>(entity =>
            {
                entity.HasKey(e => new { e.ActivityTemplateGuid, e.FormTemplateGuid })
                    .HasName("AtInFt_pkey");

                entity.ToTable("AtInFt");

                entity.Property(e => e.ActivityTemplateGuid)
                    //.HasColumnName("activity_template_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.FormTemplateGuid)
                    //.HasColumnName("form_template_guid")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActivityTemplateGu)
                    .WithMany(p => p.AtInFt)
                    .HasForeignKey(d => d.ActivityTemplateGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AtInFt_ActivityTemplate");

                entity.HasOne(d => d.FormTemplateGu)
                    .WithMany(p => p.AtInFt)
                    .HasForeignKey(d => d.FormTemplateGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AT_In_FT_Form_Template");
            });

            modelBuilder.Entity<CalculateScore>(entity =>
            {
                entity.HasKey(e => e.ScoreId)
                    .HasName("CalculateScore_pkey");

                entity.ToTable("CalculateScore");

                entity.Property(e => e.ScoreId)
                    //.HasColumnName("score_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ActivityGuid).IsRequired()
                    //.HasColumnName("activity_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.CalculatedDate)
                    //.HasColumnName("calculated_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.CalculatedScore);
                //.HasColumnName("calculated_score");

                entity.Property(e => e.ConvertionScore);
                //.HasColumnName("convertion_score");

                entity.Property(e => e.FormElementGuid)
                    //.HasColumnName("form_element_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.FormGuid)
                    //.HasColumnName("form_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentComment)
                    //.HasColumnName("model_component_comment")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.EntityType);

                entity.Property(e => e.EntityGuid)
                 .HasMaxLength(50);

                //entity.Property(e => e.OrgObjGuid)
                //    //.HasColumnName("org_obj_guid")
                //    .HasMaxLength(50);

                entity.Property(e => e.OriginalScore);
                //.HasColumnName("original_score");

                entity.Property(e => e.ReportGuid)
                    //.HasColumnName("report_guid")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActivityGu)
                    .WithMany(p => p.CalculateScore)
                    .HasForeignKey(d => d.ActivityGuid)
                    .HasConstraintName("FK_Calculate_Score_Activity");

                entity.HasOne(d => d.FormElementGu)
                    .WithMany(p => p.CalculateScore)
                    .HasForeignKey(d => d.FormElementGuid)
                    .HasConstraintName("FK_Calculate_Score_Form_Element");

                entity.HasOne(d => d.FormGu)
                    .WithMany(p => p.CalculateScore)
                    .HasForeignKey(d => d.FormGuid)
                    .HasConstraintName("FK_Calculate_Score_Form");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.CalculateScore)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .HasConstraintName("FK_Calculate_Score_Model_Component");


                //entity.HasOne(d => d.OrgObjGu)
                //    .WithMany(p => p.CalculateScore)
                //    .HasForeignKey(d => d.OrgObjGuid)
                //    .HasConstraintName("FK_Calculate_Score_Organization_Object");

                entity.HasOne(d => d.EntityTypeId)
                    .WithMany(p => p.CalculateScores)
                    .HasForeignKey(d => d.EntityType)
                    .HasConstraintName("FK_Calculate_Score_Entity_Type");

                entity.HasOne(d => d.Score)
                    .WithOne(p => p.InverseScore)
                    .HasForeignKey<CalculateScore>(d => d.ScoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calculate_Score_Score");
            });

            modelBuilder.Entity<CalenderRollup>(entity =>
            {
                entity.ToTable("CalenderRollup");

                entity.Property(e => e.CalenderRollupId);
                //.HasColumnName("calender_rollup_id");

                entity.Property(e => e.CalenderRollupName)
                    .IsRequired()
                    //.HasColumnName("calender_rollup_name")
                    .HasMaxLength(50);
            });

            //modelBuilder.Entity<Candidate>(entity =>
            //{
            //    entity.HasKey(e => e.UserGuid)
            //        .HasName("Candidate_pkey");

            //    entity.Property(e => e.UserGuid)
            //        //.HasColumnName("user_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.AreaCode).HasColumnType("character varying");

            //    entity.Property(e => e.Birthdate).HasColumnType("date");

            //    entity.Property(e => e.EndDateRank).HasColumnType("date");

            //    entity.Property(e => e.EndDateUnit).HasColumnType("date");

            //    entity.Property(e => e.EnrichmentId).HasColumnType("character varying");

            //    entity.Property(e => e.IdValue)
            //        .IsRequired()
            //        .HasMaxLength(50);

            //    entity.Property(e => e.MinuyText).HasColumnType("character varying");

            //    entity.Property(e => e.ObviousGood).HasColumnType("character varying");

            //    entity.Property(e => e.ReadyKidum).HasColumnType("character varying");

            //    entity.Property(e => e.Slabs).HasColumnType("character varying");

            //    entity.Property(e => e.StartDateRank).HasColumnType("date");

            //    entity.Property(e => e.StartDateUnit).HasColumnType("date");

            //    entity.Property(e => e.ZgrantDate).HasColumnType("date");

            //    entity.HasOne(d => d.UserGu)
            //        .WithOne(p => p.Candidate)
            //        .HasForeignKey<Candidate>(d => d.UserGuid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Candidate_User");
            //});

            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.UserGuid)
                   .HasName("UserPreference_pkey");

                entity.Property(e => e.UserGuid)
                    .HasMaxLength(50);
                entity.Property(e => e.UserTheme).HasDefaultValueSql("1");
                entity.Property(e => e.UserLayOut).HasDefaultValueSql("1");

                entity.HasOne(d => d.User)
                  .WithOne(p => p.UserPreference)
                  .HasForeignKey<UserPreference>(d => d.UserGuid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Candidate_User");


            });

            modelBuilder.Entity<ConvertionTable>(entity =>
            {
                entity.HasKey(e => new { e.ModelComponentGuid, e.LevelId })
                    .HasName("Convertion_table_pkey");

                entity.ToTable("ConvertionTable");

                entity.Property(e => e.ModelComponentGuid).IsRequired()
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.LevelId).IsRequired();
                //.HasColumnName("level_id");

                entity.Property(e => e.ConversionTableCreateDate)
                    //.HasColumnName("conversion_table_create_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.ConversionTableFinalScore);
                //.HasColumnName("conversion_table_final_score");

                entity.Property(e => e.ConversionTableModifiedDate)
                    //.HasColumnName("conversion_table_modified_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.ConversionTableScoreOrder)
                    //.HasColumnName("conversion_table_score_order")
                    .HasMaxLength(255);

                entity.Property(e => e.ConversionTableStatus)
                    //.HasColumnName("conversion_table_status")
                    .HasMaxLength(255);

                entity.Property(e => e.EndRange);
                //.HasColumnName("end_range");

                entity.Property(e => e.EndRangeScoreDisplayed);
                //.HasColumnName("end_range_score_displayed");

                entity.Property(e => e.StartRange);
                //.HasColumnName("start_range");

                entity.Property(e => e.StartRangeScoreDisplayed);
                //.HasColumnName("start_range_score_displayed");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.ConvertionTable)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Convertion_tables_Model_Component");
            });

            modelBuilder.Entity<Description>(entity =>
            {
                entity.HasKey(e => e.DescriptionGuid)
                    .HasName("Description_pkey");

                entity.ToTable("Description");

                entity.Property(e => e.DescriptionGuid).UseIdentityColumn();
                //.HasColumnName("description_guid");

                entity.Property(e => e.Creator);
                //.HasColumnName("creator");

                entity.Property(e => e.CreatorUserGuid)
                    .IsRequired()
                    //.HasColumnName("creator_user_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.Modify);
                //.HasColumnName("modify");

                entity.Property(e => e.ModifyUserGuid)
                    //.HasColumnName("modify_user_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    //.HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Remark)
                    //.HasColumnName("remark")
                    .HasMaxLength(255);
            });


            modelBuilder.Entity<EntityType>(entity =>
            {
                entity.HasKey(e => e.EntityTypeId)
                    .HasName("Entity_Type_pkey");

                entity.ToTable("EntityType");

                entity.Property(e => e.EntityTypeId);

                entity.Property(e => e.EntityTypeName)
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<ActivityEntity>(entity =>
            {
                entity.HasKey(e => e.ActivityEntityId)
                    .HasName("PK_Activity_Entity");

                entity.ToTable("ActivityEntity");

                entity.Property(e => e.ActivityEntityId).IsRequired().UseIdentityColumn();

                entity.Property(e => e.EntityGuid).IsRequired().HasMaxLength(50);

                entity.Property(e => e.EntityType).IsRequired();

                entity.Property(e => e.ActivityGuid).IsRequired().HasMaxLength(50);

                entity.HasOne(d => d.EntityTypeId)
                .WithMany(p => p.ActivityEntity)
                .HasForeignKey(d => d.EntityType)
                .HasConstraintName("FK_Entity_In_Activity_Entity_Type");

                entity.HasOne(d => d.ActivityGu)
                .WithMany(p => p.ActivityEntity)
                .HasForeignKey(d => d.ActivityGuid)
                .HasConstraintName("FK_Entity_In_Activity_Activity_Guid");
            });
            modelBuilder.Entity<ActivityEstimator>(entity =>
            {
                entity.HasKey(e => new { e.ActivityEntity, e.EstimatedGuid, e.EstimatedType })
                 .HasName("PK_Activity_Estimator");

                entity.ToTable("ActivityEstimator");

                entity.Property(e => e.ActivityEntity);

                entity.Property(e => e.EstimatedGuid).HasMaxLength(50);

                entity.Property(e => e.EstimatedType);

                entity.HasOne(d => d.ActivityEntityId)
                .WithMany(p => p.ActivityEstimator)
                .HasForeignKey(d => d.ActivityEntity)
                .HasConstraintName("FK_Activity_Estimator_Activity_Entity_Id");

                entity.HasOne(d => d.EstimatedTypeId)
               .WithMany(p => p.ActivityEstimator)
               .HasForeignKey(d => d.EstimatedType)
               .HasConstraintName("FK_Activity_Estimator_Estimated_Type");

            });

            //modelBuilder.Entity<EstimatedOrganizationObject>(entity =>
            //{
            //    entity.HasKey(e => new { e.OrgObjGuid, e.ActivityGuid, e.OrgObjEstimatedGuid })
            //        .HasName("Estimated_Organization_Object_pkey");

            //    entity.ToTable("EstimatedOrganizationObject");

            //    entity.Property(e => e.OrgObjGuid)
            //        //.HasColumnName("org_obj_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.ActivityGuid)
            //        //.HasColumnName("activity_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.OrgObjEstimatedGuid)
            //        //.HasColumnName("org_obj_estimated_guid")
            //        .HasMaxLength(50);

            //    entity.HasOne(d => d.ActivityGu)
            //        .WithMany(p => p.EstimatedOrganizationObject)
            //        .HasForeignKey(d => d.ActivityGuid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Estimated_Organization_Object_Activity");

            //    entity.HasOne(d => d.OrgObjEstimatedGu)
            //        .WithMany(p => p.EstimatedOrganizationObjectOrgObjEstimatedGu)
            //        .HasForeignKey(d => d.OrgObjEstimatedGuid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Estimated_Organization_Object_Organization_Object1");

            //    entity.HasOne(d => d.OrgObjGu)
            //        .WithMany(p => p.EstimatedOrganizationObjectOrgObjGu)
            //        .HasForeignKey(d => d.OrgObjGuid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Estimated_Organization_Object_Organization_Object");
            //});

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasKey(e => e.FormGuid)
                    .HasName("Form_pkey");

                entity.Property(e => e.FormGuid)
                    //.HasColumnName("form_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ActivityGuid)
                    //.HasColumnName("activity_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ApproveDate)
                    //.HasColumnName("approve_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.ApproveUserGuid)
                    //.HasColumnName("approve_user_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.FormTemplateGuid)
                    //.HasColumnName("form_template_guid")
                    .HasMaxLength(50);

                //entity.Property(e => e.OrgObjGuid)
                //    //.HasColumnName("org_obj_guid")
                //    .HasMaxLength(50);
                entity.Property(e => e.EntityType);

                entity.Property(e => e.EntityGuid)
                 .HasMaxLength(50);

                entity.Property(e => e.Status);
                //.HasColumnName("status");

                entity.Property(e => e.UserInCourse)
                    .HasMaxLength(50);

                entity.HasOne(d => d.ActivityGu)
                    .WithMany(p => p.Form)
                    .HasForeignKey(d => d.ActivityGuid)
                    .HasConstraintName("FK_Forms_Activities");

                entity.HasOne(d => d.ApproveUserGu)
                    .WithMany(p => p.Form)
                    .HasForeignKey(d => d.ApproveUserGuid)
                    .HasConstraintName("FK_Forms_Users");

                entity.HasOne(d => d.FormTemplateGu)
                    .WithMany(p => p.Form)
                    .HasForeignKey(d => d.FormTemplateGuid)
                    .HasConstraintName("FK_Forms_Form_templates");

                //entity.HasOne(d => d.OrgObjGu)
                //    .WithMany(p => p.Form)
                //    .HasForeignKey(d => d.OrgObjGuid)
                //    .HasConstraintName("FK_Form_Organization_Object");

                entity.HasOne(d => d.EntityTypeId)
                  .WithMany(p => p.Forms)
                  .HasForeignKey(d => d.EntityType)
                  .HasConstraintName("FK_Form_Guid_Type");

                entity.HasOne(d => d.UserInCour)
                    .WithMany(p => p.FormCourse)
                    .HasForeignKey(d => d.UserInCourse)
                    .HasConstraintName("FK_User_binding");

                entity.HasOne(d => d.StatusNavigation)
                   .WithMany(p => p.Form)
                   .HasForeignKey(d => d.Status)
                   .HasConstraintName("FK_Form_Form_Status");
            });

            modelBuilder.Entity<FormElement>(entity =>
            {
                entity.HasKey(e => e.FormElementGuid)
                    .HasName("Form_Element_pkey");

                entity.ToTable("Form_Element");

                entity.Property(e => e.FormElementGuid)
                    //.HasColumnName("form_element_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.FormElementTitle)
                    //.HasColumnName("form_element_title")
                    .HasMaxLength(50);

                entity.Property(e => e.FormElementType);
                //.HasColumnName("form_element_type");

                entity.HasOne(d => d.FormElementTypeNavigation)
                    .WithMany(p => p.FormElement)
                    .HasForeignKey(d => d.FormElementType)
                    .HasConstraintName("FK_Form_Element_Form_Element_Type");
            });

            modelBuilder.Entity<FormElementConnection>(entity =>
            {
                entity.HasKey(e => new { e.FormElementGuid, e.ModelComponentGuid })
                    .HasName("Form_Element_Connection_pkey");

                entity.ToTable("FormElementConnection");

                entity.Property(e => e.FormElementGuid)
                    //.HasColumnName("form_element_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.HasOne(d => d.FormElementGu)
                    .WithMany(p => p.FormElementConnection)
                    .HasForeignKey(d => d.FormElementGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Form_Element_Connection_Form_Element");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.FormElementConnection)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Form_Element_Connection_Model_Component");
            });

            modelBuilder.Entity<FormElementType>(entity =>
            {
                entity.HasKey(e => e.FormElementTypeGuid)
                    .HasName("Form_Element_Type_pkey");

                entity.ToTable("FormElementType");

                entity.Property(e => e.FormElementTypeGuid);
                //.HasColumnName("form_element_type_guid");

                entity.Property(e => e.Name)
                    //.HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FormStatus>(entity =>
            {
                entity.ToTable("FormStatus");

                entity.Property(e => e.FormStatusId);
                //.HasColumnName("form_status_id");

                entity.Property(e => e.Name)
                    //.HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FormTemplate>(entity =>
            {
                entity.HasKey(e => e.FormTemplateGuid)
                    .HasName("Form_Template_pkey");

                entity.ToTable("FormTemplate");

                entity.Property(e => e.FormTemplateGuid)
                    //.HasColumnName("form_template_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate)
                    //.HasColumnName("create_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.CreatorUserGuid)
                    //.HasColumnName("creator_user_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    //.HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.ModifiedDate)
                    //.HasColumnName("modified_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    //.HasColumnName("name")
                    .HasMaxLength(255);

                entity.HasOne(d => d.CreatorUserGu)
                    .WithMany(p => p.FormTemplate)
                    .HasForeignKey(d => d.CreatorUserGuid)
                    .HasConstraintName("FK_Form_templates_Users");
            });

            modelBuilder.Entity<FormTemplateStructure>(entity =>
            {
                entity.ToTable("FormTemplateStructure");

                entity.Property(e => e.FormTemplateStructureId);
                //.HasColumnName("form_template_structure_id");

                entity.Property(e => e.FormElementGuid)
                    //.HasColumnName("form_element_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.FormTemplateGuid)
                    .IsRequired()
                    //.HasColumnName("form_template_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.Order);
                //.HasColumnName("order");

                entity.HasOne(d => d.FormElementGu)
                    .WithMany(p => p.FormTemplateStructure)
                    .HasForeignKey(d => d.FormElementGuid)
                    .HasConstraintName("FK_Form_Template_Structure_Form_Element");

                entity.HasOne(d => d.FormTemplateGu)
                    .WithMany(p => p.FormTemplateStructure)
                    .HasForeignKey(d => d.FormTemplateGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Form_Template_Structure_Form_Template");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.FormTemplateStructure)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .HasConstraintName("FK_Form_Template_Structure_Model_Component");
            });

            modelBuilder.Entity<MeasuringUnit>(entity =>
            {
                entity.ToTable("MeasuringUnit");

                entity.Property(e => e.MeasuringUnitId);
                //.HasColumnName("measuring_unit_id");

                entity.Property(e => e.MeasuringUnitName)
                    .IsRequired()
                    //.HasColumnName("measuring_unit_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModelComponent>(entity =>
            {
                entity.HasKey(e => e.ModelComponentGuid)
                    .HasName("Model_Component_pkey");

                entity.ToTable("ModelComponent");

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate)
                    .IsRequired()
                    //.HasColumnName("create_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.MetricCalenderRollup);
                //.HasColumnName("metric_calender_rollup");

                entity.Property(e => e.MetricCommentObligationLevel);
                //.HasColumnName("metric_comment_obligation_level");

                entity.Property(e => e.MetricExpiredPeriod)
                    //.HasColumnName("metric_expired_period")
                    .HasMaxLength(25);

                entity.Property(e => e.MetricExpiredPeriodSecondary)
                   //.HasColumnName("metric_expired_period_secondary")
                   .HasMaxLength(25);

                entity.Property(e => e.MetricFormula)
                    //.HasColumnName("metric_formula")
                    .HasMaxLength(255);

                entity.Property(e => e.MetricGradualDecreasePeriod);
                //.HasColumnName("metric_gradual_decrease_period");

                entity.Property(e => e.MetricGradualDecreasePrecent);
                //.HasColumnName("metric_gradual_decrease_precent");

                entity.Property(e => e.MetricIsVisible);
                //.HasColumnName("metric_is_visible");

                entity.Property(e => e.MetricMeasuringUnit);
                //.HasColumnName("metric_measuring_unit");

                entity.Property(e => e.MetricMinimumFeeds);
                //.HasColumnName("metric_minimum_feeds");

                entity.Property(e => e.MetricNotDisplayIfIrrelevant);
                //.HasColumnName("metric_not_display_if_irrelevant");

                entity.Property(e => e.MetricRequired);
                //.HasColumnName("metric_required");

                entity.Property(e => e.MetricRollupMethod);
                //.HasColumnName("metric_rollup_method");

                entity.Property(e => e.MetricSource);
                //.HasColumnName("metric_source");

                entity.Property(e => e.ModelComponentOrder);
                //.HasColumnName("model_component_order");

                entity.Property(e => e.ModifiedDate)
                    .IsRequired()
                    //.HasColumnName("modified_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedUserGuid)
                    //.HasColumnName("modified_user_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    //.HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ProfessionalInstruction);
                //.HasColumnName("professional_instruction")

                entity.Property(e => e.ModelDescriptionText)
                   //.HasColumnName("professional_instruction")
                   .HasMaxLength(255);

                entity.Property(e => e.ShowOrigionValue);
                //.HasColumnName("show_origion_value");

                entity.Property(e => e.Source);
                //.HasColumnName("source");

                entity.Property(e => e.Status);
                //.HasColumnName("status");

                entity.Property(e => e.Weight);
                //.HasColumnName("weight");

                entity.Property(e => e.TemplateType);

                entity.Property(e => e.CalcAsSum);

                entity.Property(e => e.GroupChildren);

                entity.HasOne(d => d.MetricCalenderRollupNavigation)
                    .WithMany(p => p.ModelComponent)
                    .HasForeignKey(d => d.MetricCalenderRollup)
                    .HasConstraintName("FK_Model_Component_Calender_Rollup");

                entity.HasOne(d => d.MetricMeasuringUnitNavigation)
                    .WithMany(p => p.ModelComponent)
                    .HasForeignKey(d => d.MetricMeasuringUnit)
                    .HasConstraintName("FK_Model_Component_Measuring_Unit");

                entity.HasOne(d => d.MetricRollupMethodNavigation)
                    .WithMany(p => p.ModelComponent)
                    .HasForeignKey(d => d.MetricRollupMethod)
                    .HasConstraintName("FK_Model_Component_Rollup_Method");

                entity.HasOne(d => d.MetricSourceNavigation)
                    .WithMany(p => p.ModelComponentMetricSourceNavigation)
                    .HasForeignKey(d => d.MetricSource)
                    .HasConstraintName("FK_Model_Component_Model_Component_Source1");

                entity.HasOne(d => d.ModifiedUserGu)
                    .WithMany(p => p.ModelComponent)
                    .HasForeignKey(d => d.ModifiedUserGuid)
                    .HasConstraintName("FK_Model_Component_User");

                entity.HasOne(d => d.SourceNavigation)
                    .WithMany(p => p.ModelComponentSourceNavigation)
                    .HasForeignKey(d => d.Source)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Component_Model_Component_Source");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.ModelComponent)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_Model_Component_Model_Component_Status");
            });

            modelBuilder.Entity<ModelComponentSource>(entity =>
            {
                entity.ToTable("ModelComponentSource");

                entity.Property(e => e.ModelComponentSourceId);
                //.HasColumnName("model_component_source_id");

                entity.Property(e => e.ModelComponentSourceName)
                    .IsRequired()
                    //.HasColumnName("model_component_source_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModelComponentStatus>(entity =>
            {
                entity.ToTable("ModelComponentStatus");

                entity.Property(e => e.ModelComponentStatusId);
                //.HasColumnName("model_component_status_id");

                entity.Property(e => e.ModelComponentStatusName)
                    .IsRequired()
                    //.HasColumnName("model_component_status_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModelComponentType>(entity =>
            {
                entity.HasKey(e => e.TypeGuid)
                    .HasName("Model_Component_Type_pkey");

                entity.ToTable("ModelComponentType");

                entity.Property(e => e.TypeGuid);
                //.HasColumnName("type_guid");

                entity.Property(e => e.TypeName)
                    //.HasColumnName("type_name")
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<ModelDescription>(entity =>
            {
                entity.HasKey(e => e.ModelDescriptionId)
                 .HasName("PK_Model_Description");

                entity.ToTable("ModelDescription");

                entity.Property(e => e.ModelDescriptionId).UseIdentityColumn();

                entity.Property(e => e.DescriptionGuid);

                entity.Property(e => e.ModelComponentGuid)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DescriptionGu)
                    .WithMany(p => p.ModelDescription)
                    .HasForeignKey(d => d.DescriptionGuid)
                    .HasConstraintName("FK_Model_Description_Description");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.ModelDescription)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Description_Models");
            });

            modelBuilder.Entity<ModelStructure>(entity =>
            {
                entity.ToTable("ModelStructure");

                entity.Property(e => e.Id);
                //.HasColumnName("id");

                entity.Property(e => e.ModelComponentGuid)
                    .IsRequired()
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentOrigionGuid)
                    //.HasColumnName("model_component_origion_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentParentGuid)
                    .IsRequired()
                    //.HasColumnName("model_component_parent_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentType);
                //.HasColumnName("model_component_type");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.ModelStructureModelComponentGu)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Structure_Model_Component");

                entity.HasOne(d => d.ModelComponentOrigionGu)
                    .WithMany(p => p.ModelStructureModelComponentOrigionGu)
                    .HasForeignKey(d => d.ModelComponentOrigionGuid)
                    .HasConstraintName("FK_Model_Structure_Model_Component2");

                entity.HasOne(d => d.ModelComponentParentGu)
                    .WithMany(p => p.ModelStructureModelComponentParentGu)
                    .HasForeignKey(d => d.ModelComponentParentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Model_Structure_Model_Component1");

                entity.HasOne(d => d.ModelComponentTypeNavigation)
                    .WithMany(p => p.ModelStructure)
                    .HasForeignKey(d => d.ModelComponentType)
                    .HasConstraintName("FK_Model_Structure_Model_Component_Type");
            });

            modelBuilder.Entity<OrgModelPolygon>(entity =>
            {
                entity.HasKey(e => new { e.UnitGuid, e.ModelComponentGuid, e.PolygonGuid })
                    .HasName("Org_Model_Polygon_pkey");

                entity.ToTable("OrgModelPolygon");

                entity.Property(e => e.UnitGuid)
                    //.HasColumnName("org_obj_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.PolygonGuid)
                    //.HasColumnName("polygon_guid")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.OrgModelPolygon)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("model_component_guid_pkey");

                entity.HasOne(d => d.UnitGu)
                    .WithMany(p => p.OrgModelPolygon)
                    .HasForeignKey(d => d.UnitGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("unit_guid_pkey");
            });

            //modelBuilder.Entity<OrganizationObject>(entity =>
            //{
            //    entity.HasKey(e => e.OrgObjGuid)
            //        .HasName("Organization_Object_pkey");

            //    entity.ToTable("OrganizationObject");

            //    entity.Property(e => e.OrgObjGuid)
            //        //.HasColumnName("org_obj_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        //.HasColumnName("name")
            //        .HasMaxLength(255);

            //    entity.Property(e => e.Order);
            //    //.HasColumnName("order");

            //    entity.Property(e => e.Remark)
            //        //.HasColumnName("remark")
            //        .HasMaxLength(255);

            //    entity.HasOne(d => d.OrganizationTypeNavigation)
            //        .WithMany(p => p.OrgObjType)
            //        .HasForeignKey(d => d.OrgObjType)
            //        .HasConstraintName("FK_OrganizationObject_OrganizationType");
            //});

            //modelBuilder.Entity<OrganizationObjectConnection>(entity =>
            //{
            //    entity.HasKey(e => e.OrgObjConnGuid)
            //        .HasName("Organization_Object_Connection_pkey");

            //    entity.ToTable("OrganizationObjectConnection");

            //    entity.Property(e => e.OrgObjConnGuid);
            //    //.HasColumnName("org_obj_conn_guid");

            //    entity.Property(e => e.ActivityTemplateGuid)
            //        //.HasColumnName("activity_template_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.DescriptionGuid);
            //    //.HasColumnName("description_guid");

            //    entity.Property(e => e.OrgObjGuid)
            //        .IsRequired()
            //        //.HasColumnName("org_obj_guid")
            //        .HasMaxLength(50);

            //    //entity.HasOne(d => d.ActivityTemplateGu)
            //    //    .WithMany(p => p.OrganizationObjectConnection)
            //    //    .HasForeignKey(d => d.ActivityTemplateGuid)
            //    //    .HasConstraintName("FK_Organization_Object_Connection_Activity_template");

            //    //entity.HasOne(d => d.DescriptionGu)
            //    //    .WithMany(p => p.OrganizationObjectConnection)
            //    //    .HasForeignKey(d => d.DescriptionGuid)
            //    //    .HasConstraintName("FK_Organization_Object_Connection_Description");

            //    //entity.HasOne(d => d.ModelComponentGu)
            //    //    .WithMany(p => p.OrganizationObjectConnection)
            //    //    .HasForeignKey(d => d.ModelComponentGuid)
            //    //    .HasConstraintName("FK_Organization_Object_Connection_Model_Component");

            //    //entity.HasOne(d => d.OrgObjGu)
            //    //    .WithMany(p => p.OrganizationObjectConnection)
            //    //    .HasForeignKey(d => d.OrgObjGuid)
            //    //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    //    .HasConstraintName("FK_Organization_Object_Connection_Organization_Object");
            //});
            modelBuilder.Entity<EntityDescription>(entity =>
            {
                entity.HasKey(e => e.EntityDescriptionId)
                    .HasName("PK_Entity_Description");

                entity.ToTable("EntityDescription");

                entity.Property(e => e.EntityDescriptionId).UseIdentityColumn(); ;

                entity.Property(e => e.EntityGuid).HasMaxLength(50); ;
                //.HasColumnName("entity_description_guid");

                entity.Property(e => e.DescriptionGuid);

                entity.HasOne(d => d.DescriptionGu)
                  .WithMany(p => p.EntityDescription)
                  .HasForeignKey(d => d.DescriptionGuid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Entity_Description");

            });

            //modelBuilder.Entity<EntityActivityTemplate>(entity =>
            //{
            //    entity.HasKey(e => e.EntityActivityTamplateId)
            //        .HasName("PK_Entity_Activity_Template");

            //    entity.ToTable("EntityActivityTemplate");

            //    entity.Property(e => e.EntityActivityTamplateId).UseIdentityColumn(); 

            //    entity.Property(e => e.EntityGuid).HasMaxLength(50);
            //    //.HasColumnName("entity_description_guid");

            //    entity.Property(e => e.ActivityTemplateGuid).HasMaxLength(50);

            //    entity.HasOne(d => d.ActivityTemplateGu)
            //      .WithMany(p => p.EntityActivityTemplate)
            //      .HasForeignKey(d => d.ActivityTemplateGuid)
            //      .OnDelete(DeleteBehavior.ClientSetNull)
            //      .HasConstraintName("FK_Entity_Activity_Template_Guid");

            //});

            //modelBuilder.Entity<OrganizationStructure>(entity =>
            //{
            //    entity.HasKey(e => e.OrgObjGuid)
            //        .HasName("Organization_Structure_pkey");

            //    entity.ToTable("OrganizationStructure");

            //    entity.Property(e => e.OrgObjGuid)
            //        //.HasColumnName("org_obj_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.OrgObjParentGuid)
            //        //.HasColumnName("org_obj_parent_guid")
            //        .HasMaxLength(50);

            //    entity.HasOne(d => d.OrgObjGu)
            //        .WithOne(p => p.OrganizationStructureOrgObjGu)
            //        .HasForeignKey<OrganizationStructure>(d => d.OrgObjGuid)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Organization_Structure_Organization_Object");

            //    entity.HasOne(d => d.OrgObjParentGu)
            //        .WithMany(p => p.OrganizationStructureOrgObjParentGu)
            //        .HasForeignKey(d => d.OrgObjParentGuid)
            //        .HasConstraintName("FK_Organization_Structure_Organization_Object1");
            //});

            modelBuilder.Entity<OutSourceScore>(entity =>
            {
                entity.HasKey(e => new { e.UserGuid, e.ModelComponentGuid, e.EventDate })
                    .HasName("OutSource_Score_pkey");

                entity.ToTable("OutSourceScore");

                entity.Property(e => e.UserGuid)
                    //.HasColumnName("user_guid")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasColumnType("character varying");

                entity.Property(e => e.EventDate)
                    //.HasColumnName("event_date")
                    .HasColumnType("date");

                entity.Property(e => e.Score)
                    .IsRequired()
                    //.HasColumnName("score")
                    .HasColumnType("character varying");

                entity.Property(e => e.FormType)
                 .IsRequired();

                entity.Property(e => e.AverageScore);

                entity.Property(e => e.EvaluatingCount);

                entity.Property(e => e.CandidateUnit);

                entity.Property(e => e.CandidateRank);

                entity.Property(e => e.CandidateRole);
                entity.Property(e => e.TextAnswerQuestion);
                entity.Property(e => e.TextAnswerSummary);
            });

            modelBuilder.Entity<PermissionTypes>(entity =>
            {
                entity.HasKey(e => e.PermissionTypeId)
                    .HasName("PermissionTypes_pkey");

                entity.Property(e => e.PermissionTypeId).ValueGeneratedNever();

                entity.Property(e => e.PermissionTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ReportType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("Report_Type_pkey");

                entity.ToTable("ReportType");

                entity.Property(e => e.TypeId);
                //.HasColumnName("type_id");

                entity.Property(e => e.Name)
                    //.HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RoleItems>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.RoleItemId })
                    .HasName("PK_RoleId_RolItemId");
            });

            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PermissionTypeId })
                    .HasName("RolePermissions_pkey");

                entity.HasOne(d => d.PermissionType)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermisionTypes");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UnitGuid)
                    .IsRequired()
                    //.HasColumnName("org_obj_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate)
                    .IsRequired()
                    .HasMaxLength(14);

                entity.Property(e => e.UpdateUserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.UnitGu)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.UnitGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Organization_Object");

                //entity.HasOne(d => d.UpdateUser)
                //    .WithMany(p => p.Roles)
                //    .HasForeignKey(d => d.UpdateUserId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Users");
            });

            modelBuilder.Entity<RollupMethod>(entity =>
            {
                entity.ToTable("RollupMethod");

                entity.Property(e => e.RollupMethodId);
                //.HasColumnName("rollup_method_id");

                entity.Property(e => e.RollupMethodName)
                    .IsRequired()
                    //.HasColumnName("rollup_method_name")
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<SavedReportData>(entity =>
            {
                entity.ToTable("SavedReportData");

                entity.Property(e => e.Id);
                //.HasColumnName("id");

                entity.Property(e => e.ReportData)
                    .IsRequired()
                    //.HasColumnName("report_data")
                    .HasColumnType("json");

                entity.Property(e => e.ReportGuid)
                    .IsRequired()
                    //.HasColumnName("report_guid")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SavedReports>(entity =>
            {
                entity.HasKey(e => e.ReportGuid)
                    .HasName("Saved_reports_pkey");

                entity.ToTable("SavedReports");

                entity.Property(e => e.ReportGuid)
                    //.HasColumnName("report_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.CandidateUserGuid)
                   .HasMaxLength(50);



                entity.Property(e => e.CalculatedDates)
                    .IsRequired()
                    //.HasColumnName("calculated_dates")
                    .HasColumnType("character varying");

                entity.Property(e => e.IsPrimary);
                //.HasColumnName("is_primary");

                entity.Property(e => e.IsSecondary);
                //.HasColumnName("is_secondary");

                entity.Property(e => e.IsWatch);
                //.HasColumnName("is_watch");

                entity.Property(e => e.ModelComponentGuid)
                    .IsRequired()
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    //.HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Order);
                //.HasColumnName("order");

                entity.Property(e => e.ReportType);
                //.HasColumnName("report_type");

                entity.Property(e => e.TemplateType);
                //.HasColumnName("report_type");

                entity.Property(e => e.UserGuid)
                    .IsRequired()
                    //.HasColumnName("user_guid")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.SavedReports)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Saved_reports_Model_Component");

                entity.HasOne(d => d.ReportTypeNavigation)
                    .WithMany(p => p.SavedReports)
                    .HasForeignKey(d => d.ReportType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Saved_reports_Report_Type");

                entity.HasOne(d => d.UserGu)
                    .WithMany(p => p.SavedReports)
                    .HasForeignKey(d => d.UserGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Saved_reports_User");

                //entity.HasOne(d => d.ReportG)
                //   .WithMany(p => p.SavedReports)
                //   .HasForeignKey(d => d)
                //   .OnDelete(DeleteBehavior.ClientSetNull)
                //   .HasConstraintName("FK_SavedReportConnectionInterface_Saved_reports");

            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.Property(e => e.ScoreId)
                    //.HasColumnName("score_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ActivityGuid)
                    //.HasColumnName("activity_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ConvertionScore);
                //.HasColumnName("convertion_score");

                entity.Property(e => e.FormElementGuid)
                    //.HasColumnName("form_element_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.FormGuid)
                    //.HasColumnName("form_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelComponentComment)
                    //.HasColumnName("model_component_comment")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelComponentGuid)
                    //.HasColumnName("model_component_guid")
                    .HasMaxLength(50);

                //entity.Property(e => e.OrgObjGuid)
                //    //.HasColumnName("org_obj_guid")
                //    .HasMaxLength(50);

                entity.Property(e => e.EntityType);

                entity.Property(e => e.EntityGuid)
                 .HasMaxLength(50);

                entity.Property(e => e.OriginalScore);
                //.HasColumnName("original_score");

                entity.Property(e => e.Status);
                //.HasColumnName("status");

                entity.HasOne(d => d.ActivityGu)
                    .WithMany(p => p.Score)
                    .HasForeignKey(d => d.ActivityGuid)
                    .HasConstraintName("FK_Score_Activity");

                entity.HasOne(d => d.FormElementGu)
                    .WithMany(p => p.Score)
                    .HasForeignKey(d => d.FormElementGuid)
                    .HasConstraintName("FK_Score_Form_Element");

                entity.HasOne(d => d.FormGu)
                    .WithMany(p => p.Score)
                    .HasForeignKey(d => d.FormGuid)
                    .HasConstraintName("FK_Score_Form");

                entity.HasOne(d => d.ModelComponentGu)
                    .WithMany(p => p.Score)
                    .HasForeignKey(d => d.ModelComponentGuid)
                    .HasConstraintName("FK_Score_Model_Component");

                //entity.HasOne(d => d.OrgObjGu)
                //    .WithMany(p => p.Score)
                //    .HasForeignKey(d => d.OrgObjGuid)
                //    .HasConstraintName("FK_Score_Organization_Object");

                entity.HasOne(d => d.EntityTypeId)
                  .WithMany(p => p.Scores)
                  .HasForeignKey(d => d.EntityType)
                  .HasConstraintName("FK_Score_Entity_Type");

                entity.HasOne(d => d.ScoreNavigation)
                    .WithOne(p => p.InverseScoreNavigation)
                    .HasForeignKey<Score>(d => d.ScoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Score_Score");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Score)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_Score_Form_Status");
            });

            modelBuilder.Entity<SystemJobTitles>(entity =>
            {
                entity.HasKey(e => e.UserAdminPermission)
                    .HasName("System_Job_titles_pkey");

                entity.ToTable("SystemJobTitles");

                entity.Property(e => e.UserAdminPermission)
                    //.HasColumnName("user_admin_permission")
                    .ValueGeneratedNever();

                entity.Property(e => e.JobTitleName)
                    .IsRequired()
                    //.HasColumnName("job_title_name")
                    .HasMaxLength(255);
            });



            modelBuilder.Entity<TemplateSettings>(entity =>
            {
                entity.HasKey(e => e.TemplateType)
                      .HasName("TemplateType_pkey");

                entity.Property(e => e.TemplateType);

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    //.HasColumnName("job_title_name")
                    .HasMaxLength(255);

                entity.Property(e => e.ModelLevel);
                entity.Property(e => e.NumOfChildInLevel2);
                entity.Property(e => e.NumOfChildInLevel3);
                entity.Property(e => e.NumOfChildInLevel4);


            });




            modelBuilder.Entity<Threshold>(entity =>
            {
                entity.HasKey(e => e.ThresholdGuid)
                    .HasName("Threshold_pkey");

                entity.Property(e => e.ThresholdGuid)
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate)
                   .HasMaxLength(14)
                   .IsFixedLength();

                entity.Property(e => e.ModifiedDate)
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.ModelComponentOriginGuid)
                    .HasMaxLength(50);

                entity.Property(e => e.OriginCondition);

                entity.Property(e => e.OriginScore);

                entity.Property(e => e.OriginLevel);

                entity.Property(e => e.IsOriginLevel);

                entity.Property(e => e.ModelComponentDestinationGuid)
                   .HasMaxLength(50);

                entity.Property(e => e.DestinationCondition);

                entity.Property(e => e.DestinationScore);

                entity.Property(e => e.DestinationLevel);

                entity.Property(e => e.IsDestinationLevel);

                entity.Property(e => e.AutoMessage)
                    .HasMaxLength(1000);

                entity.Property(e => e.FreeMessage)
                   .HasMaxLength(1000);

                entity.Property(e => e.Recommendations)
                    .HasMaxLength(1000);

                entity.Property(e => e.Explanations)
                   .HasMaxLength(1000);

                entity.HasOne(d => d.ModelComponentOriginGu)
                   .WithMany(p => p.ThresholdOrigin)
                   .HasForeignKey(d => d.ModelComponentOriginGuid)
                   .HasConstraintName("FK_Threshold_Model_Component_Origin");

                entity.HasOne(d => d.ModelComponentDestinationGu)
                   .WithMany(p => p.ThresholdDestination)
                   .HasForeignKey(d => d.ModelComponentDestinationGuid)
                   .HasConstraintName("FK_Threshold_Model_Component_Destination");

                entity.HasOne(d => d.OriginConditionGu)
                    .WithMany(p => p.ThresholdOrigin)
                    .HasForeignKey(d => d.OriginCondition)
                    .HasConstraintName("FK_Thresholds_Condition_Origin");

                entity.HasOne(d => d.DestinationConditionGu)
                    .WithMany(p => p.ThresholdDestination)
                    .HasForeignKey(d => d.DestinationCondition)
                    .HasConstraintName("FK_Thresholds_Condition_Destination");
            });

            modelBuilder.Entity<ThresholdLevels>(entity =>
            {
                entity.HasKey(e => e.LevelId)
                .HasName("ThresholdLevels_pkey");

                entity.ToTable("ThresholdLevels");

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<ThresholdOriginCondition>(entity =>
            {
                entity.HasKey(e => e.OriginConditionId)
                    .HasName("ThresholdOriginCondition_pkey");

                entity.ToTable("ThresholdOriginCondition");

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<ThresholdsDestinationCondition>(entity =>
            {
                entity.HasKey(e => e.DestinationConditionId)
                    .HasName("ThresholdsDestinationCondition_pkey");

                entity.ToTable("ThresholdsDestinationCondition");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            //modelBuilder.Entity<Unit>(entity =>
            //{
            //    entity.HasKey(e => e.UnitGuid)
            //        .HasName("Unit_pkey");

            //    entity.Property(e => e.UnitGuid)
            //        //.HasColumnName("unit_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.IsEstimateUnit);
            //        //.HasColumnName("is_estimate_unit");

            //    entity.Property(e => e.PolygonGuid)
            //        //.HasColumnName("polygon_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.UnitCatalogId)
            //        //.HasColumnName("unit_catalog_id")
            //        .HasMaxLength(255);

            //    entity.Property(e => e.UnitCreateDate)
            //        //.HasColumnName("unit_create_date")
            //        .HasMaxLength(14)
            //        .IsFixedLength();

            //    entity.Property(e => e.UnitDescription)
            //        //.HasColumnName("unit_description")
            //        .HasMaxLength(1000);

            //    entity.Property(e => e.UnitId);
            //    //.HasColumnName("unit_id");

            //    entity.Property(e => e.UnitModifiedDate)
            //        //.HasColumnName("unit_modified_date")
            //        .HasMaxLength(14)
            //        .IsFixedLength();

            //    entity.Property(e => e.UnitName)
            //        //.HasColumnName("unit_name")
            //        .HasMaxLength(255);

            //    entity.Property(e => e.UnitParentGuid)
            //        //.HasColumnName("unit_parent_guid")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.UnitStatus)
            //        //.HasColumnName("unit_status")
            //        .HasMaxLength(255);

            //    entity.Property(e => e.EntityTypeGuid)
            //        //.HasColumnName("unit_type_guid")
            //        .HasMaxLength(50);

            //    entity.HasOne(d => d.UnitParentGu)
            //        .WithMany(p => p.InverseUnitParentGu)
            //        .HasForeignKey(d => d.UnitParentGuid)
            //        .HasConstraintName("FK_Units_Units");

            //    entity.HasOne(d => d.EntityTypeGu)
            //        .WithMany(p => p.Unit)
            //        .HasForeignKey(d => d.EntityTypeGuid)
            //        .HasConstraintName("FK_Units_Unit_types");
            //});
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.UnitGuid)
                    .HasName("PK_UnitGuid");
                entity.ToTable("Unit");

                entity.Property(e => e.SerialNum).UseIdentityColumn();
                entity.Property(e => e.UnitGuid)
                    .HasMaxLength(50);

                entity.Property(e => e.UnitName)
                 .HasMaxLength(255);

                entity.Property(e => e.Order);

                entity.Property(e => e.ParentUnitGuid)
                    //.HasColumnName("unit_catalog_id")
                    .HasMaxLength(50);

                entity.Property(e => e.ManagerUnitGuid)
                   .HasMaxLength(50);


                entity.Property(e => e.DefaultModelGuid)
                    //.HasColumnName("unit_create_date")
                    .HasMaxLength(50);




                entity.HasOne(d => d.UnitParentGu)
                    .WithMany(p => p.InverseUnitParentGu)
                    .HasForeignKey(d => d.ParentUnitGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unit_Parent_Unit_Guid");

                entity.HasOne(d => d.DefaultModelGu)
                   .WithMany(p => p.UnitConnection)
                   .HasForeignKey(d => d.DefaultModelGuid)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Unit_Default_Model_Guid");


                entity.HasOne(d => d.ManagerUnitGu)
                   .WithMany(p => p.ManagerUnit)
                   .HasForeignKey(d => d.ManagerUnitGuid)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Unit_Manager_Unit_Guid");

                //entity.HasOne(d => d.EntityTypeGu)
                //    .WithMany(p => p.Unit)
                //    .HasForeignKey(d => d.EntityTypeGuid)
                //    .HasConstraintName("FK_Units_Unit_types");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PersonGuid)
                    .HasName("PK_PersonGuid");
                entity.ToTable("Person");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255);
                entity.Property(e => e.LastName)
                   .HasMaxLength(255);
                entity.Property(e => e.Id)
                   .HasMaxLength(50);
                entity.Property(e => e.PersonNumber);
                entity.Property(e => e.UnitGuid)
                   .HasMaxLength(50);
                entity.Property(e => e.DirectManagerGuid)
                   .HasMaxLength(50);
                entity.Property(e => e.professionalManagerGuid)
                   .HasMaxLength(50);
                entity.Property(e => e.JobtitleGuid)
                   .HasMaxLength(50);
                entity.Property(e => e.Gender);
                entity.Property(e => e.BeginningOfWork);
                entity.Property(e => e.Email1)
                  .HasMaxLength(255);
                entity.Property(e => e.Email2)
                  .HasMaxLength(255);
                entity.Property(e => e.Phone1)
                  .HasMaxLength(255);
                entity.Property(e => e.Phone2)
                .HasMaxLength(255);
                entity.Property(e => e.Street)
                .HasMaxLength(255);
                entity.Property(e => e.City)
                .HasMaxLength(255);
                entity.Property(e => e.State);
                entity.Property(e => e.Country);
                entity.Property(e => e.ZipCode)
                .HasMaxLength(255);
                entity.Property(e => e.DateOfBirth);
                entity.Property(e => e.Status);
                entity.Property(e => e.ChildrenNum);
                entity.Property(e => e.Degree)
               .HasMaxLength(255);
                entity.Property(e => e.Institution)
               .HasMaxLength(255);
                entity.Property(e => e.Profession)
               .HasMaxLength(255);
                entity.Property(e => e.Car);
                entity.Property(e => e.Manufactor)
               .HasMaxLength(255);
                entity.Property(e => e.PlateNum)
               .HasMaxLength(50);
                entity.Property(e => e.EducationFund);
                entity.Property(e => e.LastSalaryUpdate);
                entity.Property(e => e.Files);


                entity.HasOne(d => d.UnitGu)
                .WithMany(p => p.PersonUnit)
                .HasForeignKey(d => d.UnitGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_Unit_Guid");

                entity.HasOne(d => d.DirectManagerGu)
                .WithMany(p => p.DirectManager)
                .HasForeignKey(d => d.DirectManagerGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_Direct_Manager_Guid");

                entity.HasOne(d => d.professionalManagerGu)
               .WithMany(p => p.professionalManager)
               .HasForeignKey(d => d.professionalManagerGuid)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Person_professional_Manager_Guid");

                entity.HasOne(d => d.JobtitleGu)
               .WithMany(p => p.PesronJobTitle)
               .HasForeignKey(d => d.JobtitleGuid)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Person_Job_title_Guid");

                entity.HasOne(d => d.GenderGu)
               .WithMany(p => p.PersonGender)
               .HasForeignKey(d => d.Gender)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Person_Gender");

                entity.HasOne(d => d.StatusGu)
               .WithMany(p => p.PersonStatus)
               .HasForeignKey(d => d.Status)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Person_Status");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserGuid)
                    .HasName("User_pkey");

                //entity.HasIndex(e => e.UnitGuid);

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserName)
                    .HasName("UserName")
                    .IsUnique();

                entity.Property(e => e.UserGuid)
                    //.HasColumnName("user_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.JobTitleGuid)
                    //.HasColumnName("job_title_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.UnitGuid)
                    //.HasColumnName("org_obj_guid")
                    .HasMaxLength(50);

                entity.Property(e => e.UserAdminPermission);
                //.HasColumnName("user_admin_permission");

                entity.Property(e => e.UserBusinessPhone)
            //.HasColumnName("user_business_phone")
            .HasMaxLength(255);

                entity.Property(e => e.UserCreateDate)
                    //.HasColumnName("user_create_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.UserEmail)
                    //.HasColumnName("user_email")
                    .HasMaxLength(255);

                entity.Property(e => e.UserFirstName)
                    //.HasColumnName("user_first_name")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    //.HasColumnName("user_id")
                    .HasMaxLength(50);

                entity.Property(e => e.UserLastName)
                    //.HasColumnName("user_last_name")
                    .HasMaxLength(255);

                entity.Property(e => e.UserMobilePhone)
                    //.HasColumnName("user_mobile_phone")
                    .HasMaxLength(255);

                entity.Property(e => e.UserModifiedDate)
                    //.HasColumnName("user_modified_date")
                    .HasMaxLength(14)
                    .IsFixedLength();

                entity.Property(e => e.UserName)
                    //.HasColumnName("user_name")
                    .HasMaxLength(255);

                entity.Property(e => e.UserNotes)
                    //.HasColumnName("user_notes")
                    .HasMaxLength(255);

                entity.Property(e => e.UserPassword)
                    //.HasColumnName("user_password")
                    .HasMaxLength(255);

                entity.Property(e => e.UserStatus)
                    //.HasColumnName("user_status")
                    .HasMaxLength(255);

                entity.Property(e => e.UserType).HasDefaultValueSql("1");

                entity.HasOne(d => d.UnitGu)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UnitGuid)
                    .HasConstraintName("FK_Users_Unit");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Roles_Id");

                entity.HasOne(d => d.UserTypeNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserType_User");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasKey(e => e.UserTypeId)
                    .HasName("User_Type_pkey");

                entity.ToTable("UserType");

                entity.Property(e => e.UserTypeId)
                    //.HasColumnName("UserType")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ActivityFile>(entity =>
            {
                entity.HasKey(e => e.ActivityFileGuid)
                    .HasName("ActivityFile_pkey");

                entity.Property(e => e.ActivityGuid)
                    .HasMaxLength(50);

                entity.Property(e => e.FileName)
                   .HasMaxLength(250)
                   .IsFixedLength();

                entity.Property(e => e.Content);

                entity.HasOne(d => d.Activity_gu)
                  .WithMany(p => p.ActivityFile)
                  .HasForeignKey(d => d.ActivityGuid)
                  .HasConstraintName("FK_ActivityFile_Activities");
            });

            modelBuilder.Entity<OrganizationUnion>(entity =>
            {
                entity.HasKey(e => e.OrganizationUnionId)
                    .HasName("OrganizationUnion_pkey");

                //entity.Property(e => e.OrganizationUnionId)
                //    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrganizationUnionGuid)
           .HasMaxLength(50);

                entity.Property(e => e.OrgObjGuid)
                    .HasMaxLength(50);

                entity.Property(e => e.ParentOrgObjGuid)
                   .HasMaxLength(50);

                entity.Property(e => e.Order);

                entity.Property(e => e.Name)
                  .HasMaxLength(100);

                entity.Property(e => e.Description);

                entity.Property(e => e.ModifiedDate);

                entity.HasOne(d => d.OrgObjGu)
                  .WithMany(p => p.OrganizationUnion1)
                  .HasForeignKey(d => d.OrgObjGuid)
                  .HasConstraintName("FK_OrganizationUnion_OrganizationObject1");

                entity.HasOne(d => d.ParentOrgObjGu)
                  .WithMany(p => p.OrganizationUnion2)
                  .HasForeignKey(d => d.ParentOrgObjGuid)
                  .HasConstraintName("FK_OrganizationUnion_OrganizationObject2");
            });




            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.GenderId)
                   .HasName("PK_GenderId");
                entity.ToTable("Gender");

                entity.Property(e => e.GenderName)
               .HasMaxLength(50);

            });
            modelBuilder.HasSequence<int>("Form_Template_Structure_form_template_structure_id_seq");



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
