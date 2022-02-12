using Infrastructure.Core;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using FormsDal.Contexts;

namespace Infrastructure.Migrations
{
    public class MigrationBuilderEnsured<T> : MigrationBuilder where T : BaseContext
    {
        private string _namespace = "public";
        private MigrationBuilder _migrationBuilder;
        private List<string> _existedTables = new List<string>();

        public MigrationBuilderEnsured(MigrationBuilder migrationBuilder)
            : base(migrationBuilder.ActiveProvider)
        {
            this.Operations.AddRange(migrationBuilder.Operations);
            _migrationBuilder = migrationBuilder;
        }

        public override OperationBuilder<AddColumnOperation> AddColumn<T>(string name, string table, string type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, string? schema = null, bool nullable = false, object? defaultValue = null, string? defaultValueSql = null, string? computedColumnSql = null, bool? fixedLength = null, string? comment = null, string? collation = null, int? precision = null, int? scale = null, bool? stored = null)
        {
            if (!Exists(_namespace, table, new string[] { name }))
                return _migrationBuilder.AddColumn<T>(name, table, type, unicode, maxLength, rowVersion, schema, nullable, defaultValue, defaultValueSql, computedColumnSql, fixedLength, comment);
            return null;
        }

        public override OperationBuilder<AddForeignKeyOperation> AddForeignKey(string name, string table, string column, string principalTable, string schema = null, string principalSchema = null, string principalColumn = null, ReferentialAction onUpdate = ReferentialAction.NoAction, ReferentialAction onDelete = ReferentialAction.NoAction)
        {
            return _migrationBuilder.AddForeignKey(name, table, column, principalTable, schema, principalSchema, principalColumn, onUpdate, onDelete);
        }

        public override OperationBuilder<AddForeignKeyOperation> AddForeignKey(string name, string table, string[] columns, string principalTable, string schema = null, string principalSchema = null, string[] principalColumns = null, ReferentialAction onUpdate = ReferentialAction.NoAction, ReferentialAction onDelete = ReferentialAction.NoAction)
        {
            return _migrationBuilder.AddForeignKey(name, table, columns, principalTable, schema, principalSchema, principalColumns, onUpdate, onDelete);
        }

        public override OperationBuilder<AddPrimaryKeyOperation> AddPrimaryKey(string name, string table, string column, string schema = null)
        {
            return _migrationBuilder.AddPrimaryKey(name, table, column, schema);
        }

        public override OperationBuilder<AddPrimaryKeyOperation> AddPrimaryKey(string name, string table, string[] columns, string schema = null)
        {
            return _migrationBuilder.AddPrimaryKey(name, table, columns, schema);

        }

        public override OperationBuilder<AddUniqueConstraintOperation> AddUniqueConstraint(string name, string table, string column, string schema = null)
        {
            return _migrationBuilder.AddUniqueConstraint(name, table, column, schema);
        }

        public override OperationBuilder<AddUniqueConstraintOperation> AddUniqueConstraint(string name, string table, string[] columns, string schema = null)
        {
            return _migrationBuilder.AddUniqueConstraint(name, table, columns, schema);
        }

        public override AlterOperationBuilder<AlterColumnOperation> AlterColumn<T>(string name, string table, string? type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, string? schema = null, bool nullable = false, object? defaultValue = null, string? defaultValueSql = null, string? computedColumnSql = null, Type? oldClrType = null, string? oldType = null, bool? oldUnicode = null, int? oldMaxLength = null, bool oldRowVersion = false, bool oldNullable = false, object? oldDefaultValue = null, string? oldDefaultValueSql = null, string? oldComputedColumnSql = null, bool? fixedLength = null, bool? oldFixedLength = null, string? comment = null, string? oldComment = null, string? collation = null, string? oldCollation = null, int? precision = null, int? oldPrecision = null, int? scale = null, int? oldScale = null, bool? stored = null, bool? oldStored = null)
        {
            return _migrationBuilder.AlterColumn<T>(name, table, type, unicode, maxLength, rowVersion, schema, nullable, defaultValue, defaultValueSql, computedColumnSql, oldClrType, oldType, oldUnicode, oldMaxLength, oldRowVersion, oldNullable, oldDefaultValue, oldDefaultValueSql, oldComputedColumnSql, fixedLength, oldFixedLength, comment, oldComment);
        }

        public override AlterOperationBuilder<AlterDatabaseOperation> AlterDatabase(string? collation = null, string? oldCollation = null)
        {
            return _migrationBuilder.AlterDatabase();
        }

        public override AlterOperationBuilder<AlterSequenceOperation> AlterSequence(string name, string schema = null, int incrementBy = 1, long? minValue = null, long? maxValue = null, bool cyclic = false, int oldIncrementBy = 1, long? oldMinValue = null, long? oldMaxValue = null, bool oldCyclic = false)
        {
            return _migrationBuilder.AlterSequence(name, schema, incrementBy, minValue, maxValue, cyclic, oldIncrementBy, oldMinValue, oldMaxValue, oldCyclic);
        }

        public override AlterOperationBuilder<AlterTableOperation> AlterTable(string name, string schema = null, string comment = null, string oldComment = null)
        {
            return _migrationBuilder.AlterTable(name, schema, comment, oldComment);
        }

        public override OperationBuilder<AddCheckConstraintOperation> CreateCheckConstraint(string name, string table, string sql, string? schema = null)
        {
            return _migrationBuilder.CreateCheckConstraint(name, table, sql, schema);
        }

        public override OperationBuilder<CreateIndexOperation> CreateIndex(string name, string table, string column, string schema = null, bool unique = false, string filter = null)
        {
            return _migrationBuilder.CreateIndex(name, table, column, schema, unique, filter);
        }

        public override OperationBuilder<CreateIndexOperation> CreateIndex(string name, string table, string[] columns, string schema = null, bool unique = false, string filter = null)
        {
            return _migrationBuilder.CreateIndex(name, table, columns, schema, unique, filter);
        }

        public override OperationBuilder<CreateSequenceOperation> CreateSequence(string name, string schema = null, long startValue = 1, int incrementBy = 1, long? minValue = null, long? maxValue = null, bool cyclic = false)
        {
            return _migrationBuilder.CreateSequence(name, schema, startValue, incrementBy, minValue, maxValue, cyclic);
        }

        public override OperationBuilder<CreateSequenceOperation> CreateSequence<T>(string name, string schema = null, long startValue = 1, int incrementBy = 1, long? minValue = null, long? maxValue = null, bool cyclic = false)
        {
            return _migrationBuilder.CreateSequence<T>(name, schema, startValue, incrementBy, minValue, maxValue, cyclic);
        }

        public override CreateTableBuilder<TColumns> CreateTable<TColumns>(string name, Func<ColumnsBuilder, TColumns> columns, string schema = null, Action<CreateTableBuilder<TColumns>> constraints = null, string comment = null)
        {
            if (!Exists(_namespace, name))
                return _migrationBuilder.CreateTable(name, columns, schema, constraints, comment);
            return null;
        }

        public override OperationBuilder<DeleteDataOperation> DeleteData(string table, string keyColumn, object keyValue, string schema = null)
        {
            return _migrationBuilder.DeleteData(table, keyColumn, keyValue, schema);
        }

        public override OperationBuilder<DeleteDataOperation> DeleteData(string table, string[] keyColumns, object[] keyValues, string schema = null)
        {
            return _migrationBuilder.DeleteData(table, keyColumns, keyValues, schema);
        }

        public override OperationBuilder<DeleteDataOperation> DeleteData(string table, string keyColumn, object[] keyValues, string schema = null)
        {
            return _migrationBuilder.DeleteData(table, keyColumn, keyValues, schema);
        }

        public override OperationBuilder<DeleteDataOperation> DeleteData(string table, string[] keyColumns, object[,] keyValues, string schema = null)
        {
            return _migrationBuilder.DeleteData(table, keyColumns, keyValues, schema);
        }

        public override OperationBuilder<DropCheckConstraintOperation> DropCheckConstraint(string name, string table, string schema = null)
        {
            return _migrationBuilder.DropCheckConstraint(name, table, schema);
        }

        public override OperationBuilder<DropColumnOperation> DropColumn(string name, string table, string schema = null)
        {
            return _migrationBuilder.DropColumn(name, table, schema);
        }

        public override OperationBuilder<DropForeignKeyOperation> DropForeignKey(string name, string table, string schema = null)
        {
            return _migrationBuilder.DropForeignKey(name, table, schema);
        }

        public override OperationBuilder<DropIndexOperation> DropIndex(string name, string table = null, string schema = null)
        {
            return _migrationBuilder.DropIndex(name, table, schema);
        }

        public override OperationBuilder<DropPrimaryKeyOperation> DropPrimaryKey(string name, string table, string schema = null)
        {
            return _migrationBuilder.DropPrimaryKey(name, table, schema);
        }

        public override OperationBuilder<DropSchemaOperation> DropSchema(string name)
        {
            return _migrationBuilder.DropSchema(name);
        }

        public override OperationBuilder<DropSequenceOperation> DropSequence(string name, string schema = null)
        {
            return _migrationBuilder.DropSequence(name, schema);
        }

        public override OperationBuilder<DropTableOperation> DropTable(string name, string schema = null)
        {
            return _migrationBuilder.DropTable(name, schema);
        }

        public override OperationBuilder<DropUniqueConstraintOperation> DropUniqueConstraint(string name, string table, string schema = null)
        {
            return _migrationBuilder.DropUniqueConstraint(name, table, schema);
        }

        public override OperationBuilder<EnsureSchemaOperation> EnsureSchema(string name)
        {
            return _migrationBuilder.EnsureSchema(name);
        }

        public override bool Equals(object obj)
        {
            return _migrationBuilder.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _migrationBuilder.GetHashCode();
        }

        public override OperationBuilder<InsertDataOperation> InsertData(string table, string[] columns, string[] colTypes, object[] values, string schema = default)
        {
            if (!Exists(_namespace, table, columns, values))
                return _migrationBuilder.InsertData(table, columns, colTypes, values, schema);
            return null;
        }


        public override OperationBuilder<InsertDataOperation> InsertData(string table, string[] columns, object[] values, string schema = default)
        {
            if (!Exists(_namespace, table, columns, values))
                return _migrationBuilder.InsertData(table, columns, values, schema);
            return null;
        }

        public override OperationBuilder<RenameColumnOperation> RenameColumn(string name, string table, string newName, string schema = null)
        {
            return _migrationBuilder.RenameColumn(name, table, newName, schema);
        }

        public override OperationBuilder<RenameIndexOperation> RenameIndex(string name, string newName, string table = null, string schema = null)
        {
            return _migrationBuilder.RenameIndex(name, newName, table, schema);
        }

        public override OperationBuilder<RenameSequenceOperation> RenameSequence(string name, string schema = null, string newName = null, string newSchema = null)
        {
            return _migrationBuilder.RenameSequence(name, schema, newName, newSchema);
        }

        public override OperationBuilder<RenameTableOperation> RenameTable(string name, string schema = null, string newName = null, string newSchema = null)
        {
            return _migrationBuilder.RenameTable(name, schema, newName, newSchema);
        }

        public override OperationBuilder<RestartSequenceOperation> RestartSequence(string name, long startValue = 1, string schema = null)
        {
            return _migrationBuilder.RestartSequence(name, startValue, schema);
        }

        public override OperationBuilder<SqlOperation> Sql(string sql, bool suppressTransaction = false)
        {
            return _migrationBuilder.Sql(sql, suppressTransaction);
        }

        public override string ToString()
        {
            return _migrationBuilder.ToString();
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string keyColumn, object keyValue, string column, object value, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumn, keyValue, column, value, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string keyColumn, object keyValue, string[] columns, object[] values, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumn, keyValue, columns, values, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string[] keyColumns, object[] keyValues, string column, object value, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumns, keyValues, column, value, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string[] keyColumns, object[] keyValues, string[] columns, object[] values, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumns, keyValues, columns, values, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string keyColumn, object[] keyValues, string column, object[] values, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumn, keyValues, column, values, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string keyColumn, object[] keyValues, string[] columns, object[,] values, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumn, keyValues, columns, values, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string[] keyColumns, object[,] keyValues, string column, object[] values, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumns, keyValues, column, values, schema);
        }

        public override OperationBuilder<UpdateDataOperation> UpdateData(string table, string[] keyColumns, object[,] keyValues, string[] columns, object[,] values, string schema = null)
        {
            return _migrationBuilder.UpdateData(table, keyColumns, keyValues, columns, values, schema);
        }

        /// <summary> method "Exists", testing existing data by the straight direction to db</summary>
        public bool Exists(string tableSchema, string tableName, string[] columnName = null, object[] columnValue = null)
        {
            using var completeServiceScope = GeneralContext.CreateServiceScope();
            var services = completeServiceScope.ServiceProvider;
            using var dbContext = services.GetService<T>();
            using var conn = dbContext.Database.GetDbConnection();

            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();

            bool exists = _existedTables.Exists(x => x == tableName);
            using var command = conn.CreateCommand();

            if (!exists)
            {
                // exist table
                command.CommandText = @$"SELECT EXISTS(SELECT FROM pg_catalog.pg_class c JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace WHERE n.nspname = '{tableSchema}' AND c.relname = '{tableName}')";
                var result = command.ExecuteScalar();
                exists = Convert.ToBoolean(result);
                if (exists) _existedTables.Add(tableName);
            }

            // exist column
            if (exists && columnName != null && columnName.Length > 0)
            {
                foreach (var col in columnName)
                {
                    command.CommandText = @$"SELECT EXISTS(SELECT FROM information_schema.columns WHERE table_schema = '{tableSchema}' AND table_name = '{tableName}' AND column_name = '{col}')";
                    exists = Convert.ToBoolean(command.ExecuteScalar());
                }
                
                // exist value
                if (exists && columnValue != null && columnValue.Length > 0)
                {
                    string query = "";
                    for (int i = 0; i < columnValue.Length; i++)
                    {
                        if (columnValue[i] != null && columnValue[i].GetType() == typeof(string))
                            query += @$"""{columnName[i]}"" = '{columnValue[i]}' AND ";
                        else if(columnValue[i] == null)
                            query += @$"""{columnName[i]}"" is null AND ";
                        else
                            query += @$"""{columnName[i]}"" = {columnValue[i]} AND ";
                    }
                    query = query.Substring(0, query.LastIndexOf("AND ")).TrimEnd();

                    command.CommandText = @$"SELECT exists (SELECT 1 FROM ""{tableName}"" WHERE {query} LIMIT 1)";
                    exists = Convert.ToBoolean(command.ExecuteScalar());
                }
            }

            return exists;
        }

        /// <summary> temp Exists method </summary>
        private bool Exists_(string tableName)
        {
            using var completeServiceScope = GeneralContext.CreateServiceScope();
            var services = completeServiceScope.ServiceProvider;
            using var dbContext = services.GetService<T>();
            var count = dbContext.Database.ExecuteSqlRaw("SELECT FROM pg_catalog.pg_class c JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace WHERE n.nspname = 'public' AND c.relname = @p0)", tableName);
            return count > 0;
        }
    }
}
