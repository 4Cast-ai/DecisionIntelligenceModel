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
using FormsDal.Contexts;

namespace Infrastructure.Migrations
{
    public class MigrationEnsured<T> : Migration where T : BaseContext
    {
        private MigrationBuilderEnsured<T> _migrationBuilder;
        public string DefaultCulture { get; set; }

        /// <summary> Setup migrations </summary>
        protected sealed override void Up(MigrationBuilder migrationBuilder)
        {
            _migrationBuilder = new MigrationBuilderEnsured<T>(migrationBuilder);
            this.OnSetUp(_migrationBuilder);
        }

        /// <summary> Remove migrations, that built by setup </summary>
        protected sealed override void Down(MigrationBuilder migrationBuilder)
        {
            this.Down(_migrationBuilder);
        }

        /// <summary> On setup migrations </summary>
        private void OnSetUp(MigrationBuilderEnsured<T> migrationBuilder)
        {
            using var initServiceScope = GeneralContext.CreateServiceScope();
            this.DefaultCulture = GeneralContext.GetConfig<IAppConfig>().DefaultLocale;
            var initServiceProvider = initServiceScope.ServiceProvider;
            this.Init(migrationBuilder, initServiceProvider);
            this.Up(migrationBuilder);
            this.Down(migrationBuilder);
        }

        /// <summary> method "Init", called before "Up" </summary>
        protected virtual void Init(MigrationBuilderEnsured<T> migrationBuilder, IServiceProvider serviceProvider) { }

        /// <summary> method "Up" calling migration </summary>
        protected virtual void Up(MigrationBuilderEnsured<T> migrationBuilder) { }

        /// <summary> method "Down", removing data after "Up" and "Seed"</summary>
        protected virtual void Down(MigrationBuilderEnsured<T> migrationBuilder) { }

        /// <summary> method "Exists", testing existing data by the straight direction to db</summary>
       
    }
}
