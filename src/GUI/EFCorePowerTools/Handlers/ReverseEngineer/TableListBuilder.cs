﻿using EFCorePowerTools.Shared.Models;
using ReverseEngineer20;
using ReverseEngineer20.ReverseEngineer;
using System;
using System.Collections.Generic;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class TableListBuilder
    {
        private readonly string _connectionString;
        private readonly DatabaseType _databaseType;
        private readonly SchemaInfo[] _schemas;

        public TableListBuilder(string connectionString, DatabaseType databaseType, SchemaInfo[] schemas)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
            _databaseType = databaseType;
            _schemas = schemas;
        }

        public List<TableInformationModel> GetTableDefinitions(bool useEFCore5)
        {
            var launcher = new EfRevEngLauncher(null, useEFCore5);

            List<TableInformationModel> tables;

            if (_databaseType == DatabaseType.Undefined)
            {
                tables = launcher.GetDacpacTables(_connectionString);
            }
            else
            {
                tables = launcher.GetTables(_connectionString, _databaseType, _schemas);
            }

            foreach (var item in tables)
            {
                if (!item.HasPrimaryKey)
                {
                    item.HasPrimaryKey = true;
                    item.ShowKeylessWarning = true;
                }
            }

            return tables;
        }
    }
}
