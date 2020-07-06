namespace SSMSPlusSearch.Services
{
    using Dapper;

    using SSMSPlusCore.Utils;
    using SSMSPlusCore.SqlServer;
    using SSMSPlusSearch.Entities;
    using SSMSPlusSearch.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SSMSPlusCore.Integration.Connection;

    public class DbIndexer : IDbIndexer
    {
        private SchemaSearchRepository _schemaSearchRepository;

        private AsyncLock _asyncLock = new AsyncLock();

        public DbIndexer(SchemaSearchRepository schemaSearchRepository)
        {
            _schemaSearchRepository = schemaSearchRepository;
        }

        public async Task<int> ReIndexAsync(DbConnectionString dbConnectionString)
        {
            using (await _asyncLock.LockAsync())
            {
                var dbid = await _schemaSearchRepository.DbExists(dbConnectionString);
                if (dbid > 0)
                {
                    await _schemaSearchRepository.DropDbAsync(dbid);
                }

                return await IndexAsync(dbConnectionString);
            }
        }

        public async Task<int> DbExistsAsync(DbConnectionString dbConnectionString)
        {
            return await _schemaSearchRepository.DbExists(dbConnectionString);
        }

        public async Task<int> IndexAsync(DbConnectionString dbConnectionString)
        {
            if (DbHelper.IsSystemDb(dbConnectionString.Database))
            {
                throw new Exception("Cannot index system db: " + dbConnectionString.Database);
            }

            // await Task.

            using (await _asyncLock.LockAsync())
            {
                await Task.Delay(6000);

                var dbId = await DbExistsAsync(dbConnectionString);
                if (dbId > 0)
                    return dbId;

                string dbObjectsQuery = @"
WITH DefinitionInfo (object_id, definition)  
AS  
(  
	SELECT m.object_id , m.definition from sys.sql_modules as m 
	UNION
	SELECT c.object_id, 'CHECK ' + c.definition from sys.check_constraints as c 
		
	UNION
	SELECT dc.object_id, 'DEFAULT' + dc.definition     + ' FOR [' + c.name + ']'
	FROM sys.default_constraints as dc 
		INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
	UNION
	SELECT s.object_id, s.base_object_name from sys.synonyms  as s
) 
SELECT o.object_id AS 'objectId' , o.type_desc as 'type', schema_name(o.schema_id) AS 'schemaName', o.name, DefinitionInfo.definition, o.modify_date as ModificationDate, Parent.object_id as parentObjectId
FROM sys.objects AS o
LEFT JOIN DefinitionInfo ON DefinitionInfo.object_id = o.object_id
LEFT JOIN sys.objects AS Parent ON Parent.object_id = o.parent_object_id 
WHERE o.is_ms_shipped = 0";

                string columnsQuery = @"
SELECT c.id as 'TableId', c.name,  t.name AS 'datatype', c.prec AS 'precision', c.scale AS 'scale', cc.definition
FROM sys.syscolumns AS c
INNER JOIN sys.objects AS o ON c.id = o.object_id
INNER JOIN sys.types AS t ON c.xusertype = t.user_type_id
LEFT JOIN sys.computed_columns AS cc ON cc.object_id = o.object_id AND cc.column_id = c.colid
WHERE o.type = 'U' and o.is_ms_shipped = 0
ORDER BY c.id, c.colid";

                string indicesQuery = @"
SELECT 
	t.object_id ownerId,
	i.index_id 'indexNumber',
	i.[name],
	i.type_desc 'type',
	i.filter_definition FilterDefinition,
    i.is_unique IsUnique
FROM sys.indexes i
INNER JOIN  sys.objects t ON t.object_id = i.object_id
WHERE t.is_ms_shipped = 0 AND index_id > 0
ORDER BY t.object_id, i.index_id";

                string indicesColumnsQuery = @"
SELECT 
	OwnerId = ind.object_id,
	IndexNumber = ind.index_id,
	IndexColumnId = ic.index_column_id,
	OwnerColumnId = col.column_id,
	ColumnName = col.name,
	IsDesc = ic.is_descending_key,
    Included = ic.is_included_column
FROM		sys.indexes ind 
INNER JOIN  sys.index_columns ic	ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
INNER JOIN  sys.columns col			ON ic.object_id = col.object_id and ic.column_id = col.column_id 
INNER JOIN  sys.objects t			ON ind.object_id = t.object_id 
WHERE  
	t.is_ms_shipped = 0 
ORDER BY 
    ind.object_id, ind.index_id, ic.index_column_id;";

                using (var connection = new SqlConnection(dbConnectionString.ConnectionString))
                {
                    var dbobjects = (await connection.QueryAsync<DbObject>(dbObjectsQuery)).ToArray();
                    var dbColumns = (await connection.QueryAsync<DbColumn>(columnsQuery)).ToArray();
                    var dbIndices = (await connection.QueryAsync<DbIndex>(indicesQuery)).ToArray();
                    var dbIndicesColumns = (await connection.QueryAsync<DbIndexColumn>(indicesColumnsQuery)).ToArray();

                    //  await Task.Delay(5000);
                    var dbDefinition = new DbDefinition()
                    {
                        DbName = dbConnectionString.Database,
                        Server = dbConnectionString.Server,
                        IndexDateUtc = DateTime.UtcNow
                    };
                    return await _schemaSearchRepository.InsertDb(dbDefinition, dbobjects, dbColumns, dbIndices, dbIndicesColumns);
                }
            }
        }
    }
}
