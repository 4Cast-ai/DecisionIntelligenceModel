using AutoMapper.Configuration;
using Infrastructure.Core;
using Infrastructure.Interfaces;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Migrations
{
    public abstract class MigrationEnsured : Migration
    {
        private MigrationBuilderEnsured _migrationBuilder;
        public string DefaultCulture { get; set; }

        /// <summary> Setup migrations </summary>
        protected sealed override void Up(MigrationBuilder migrationBuilder)
        {
            _migrationBuilder = new MigrationBuilderEnsured(migrationBuilder);
            this.OnSetUp(_migrationBuilder);
        }

        /// <summary> Remove migrations, that built by setup </summary>
        protected sealed override void Down(MigrationBuilder migrationBuilder)
        {
            this.Down(_migrationBuilder);
        }

        /// <summary> On setup migrations </summary>
        private void OnSetUp(MigrationBuilderEnsured migrationBuilder)
        {
            using var initServiceScope = GeneralContext.CreateServiceScope();
            this.DefaultCulture = GeneralContext.GetConfig<IAppConfig>().DefaultLocale;
            var initServiceProvider = initServiceScope.ServiceProvider;
            this.Init(migrationBuilder, initServiceProvider);
            this.Up(migrationBuilder);
            this.Down(migrationBuilder);
        }

        /// <summary> method "Init", called before "Up" </summary>
        protected virtual void Init(MigrationBuilderEnsured migrationBuilder, IServiceProvider serviceProvider) { }

        /// <summary> method "Up" calling migration </summary>
        protected abstract void Up(MigrationBuilderEnsured migrationBuilder);

        /// <summary> method "Down", removing data after "Up" and "Seed"</summary>
        protected virtual void Down(MigrationBuilderEnsured migrationBuilder) { }

        /// <summary> method "Exists", testing existing data by the straight direction to db</summary>
       
    }
}
