using Dapper;
using SSMSPlusCore.Database;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusCore.Utils;
using SSMSPlusSearch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Repositories
{
    public class SchemaSearchRepository
    {
        protected readonly Db _db;

        public SchemaSearchRepository(Db db)
        {
            _db = db ?? throw new ArgumentException(nameof(db));
        }

        public async Task<int> DropDbAsync(int dbid)
        {
            var sql = @"
DELETE  FROM 'SchemaSearch.DbIndicesColumns' WHERE dbid = @dbid;
DELETE  FROM 'SchemaSearch.DbIndices' WHERE dbid = @dbid;
DELETE  FROM 'SchemaSearch.DbColumns' WHERE dbid = @dbid;
DELETE  FROM 'SchemaSearch.DbObjects' WHERE dbid = @dbid;
DELETE  FROM 'SchemaSearch.Dbs' WHERE dbid = @dbid;";

            return await _db.ExecuteAsync(sql, new { dbid = dbid });
        }

        public async Task<int> DbExists(DbConnectionString dbConnectionString)
        {
            var sql = "SELECT DbId FROM 'SchemaSearch.Dbs' WHERE Server = @Server AND DbName = @DbName  ORDER BY DbId DESC LIMIT 1 ";

            return await _db.QuerySingleOrDefault<int>(sql, new { Server = dbConnectionString.Server, DbName = dbConnectionString.Database });
        }

        public async Task<int> InsertDb(DbDefinition dbdefinition, DbObject[] dbObjects, DbColumn[] columns, DbIndex[] indices, DbIndexColumn[] indicesColumns)
        {
            return await _db.CommandAsync(async (conn, trn, timeout) =>
             {
                 string sql = @"INSERT INTO 'SchemaSearch.Dbs' (Server,  DbName,     IndexDateUtc) 
                                                       Values (@Server, @DbName,    @IndexDateUtc);";
                 await conn.ExecuteAsync(sql, dbdefinition, trn, timeout);

                 sql = "SELECT last_insert_rowid()";
                 var dbId = await conn.QuerySingleAsync<int>(sql, dbdefinition, trn, timeout);
                 dbObjects.ForEach(p => p.DbId = dbId);
                 columns.ForEach(p => p.DbId = dbId);
                 indices.ForEach(p => p.DbId = dbId);
                 indicesColumns.ForEach(p => p.DbId = dbId);

                 sql = @"INSERT INTO 'SchemaSearch.DbObjects'   (DbId,   ObjectId,   Type,   SchemaName,     Name,   Definition,    ModificationDate, ParentObjectId) 
                                                        Values (@DbId,  @ObjectId,  @Type,  @SchemaName,    @Name,  @Definition,    @ModificationDate,  @ParentObjectId);";
                 await conn.ExecuteAsync(sql, dbObjects, trn, timeout);

                 sql = @"INSERT INTO 'SchemaSearch.DbColumns'   (DbId,   TableId,    Name,   Datatype,   Precision,  Definition) 
                                                        Values (@DbId,  @TableId,   @Name,  @Datatype,  @Precision, @Definition);";
                 await conn.ExecuteAsync(sql, columns, trn, timeout);

                 sql = @"INSERT INTO 'SchemaSearch.DbIndices'   (DbId,   OwnerId,   IndexNumber,  Name,   Type,   FilterDefinition,  IsUnique) 
                                                        Values (@DbId,  @OwnerId,   @IndexNumber, @Name,  @Type,  @FilterDefinition, @IsUnique);";
                 await conn.ExecuteAsync(sql, indices, trn, timeout);

                 sql = @"INSERT INTO 'SchemaSearch.DbIndicesColumns'    (DbId,   OwnerId,   IndexNumber,  IndexColumnId,   OwnerColumnId,   ColumnName,  IsDesc,  Included) 
                                                        Values          (@DbId,  @OwnerId,   @IndexNumber, @IndexColumnId,  @OwnerColumnId,  @ColumnName, @IsDesc, @Included);";

                 await conn.ExecuteAsync(sql, indicesColumns, trn, timeout);

                 return dbId;
             });
        }

        public async Task<ISearchTarget[]> GetObjectsByDb(int dbid)
        {
            var sql = @"SELECT * FROM 'SchemaSearch.DbObjects' WHERE dbid =@dbid";
            var dbObjects = await _db.SelectAsync<DbObject>(sql, new { dbid = dbid });

            sql = @"SELECT * FROM 'SchemaSearch.DbColumns' WHERE dbid =@dbid";
            var dbColumns = await _db.SelectAsync<DbColumn>(sql, new { dbid = dbid });

            sql = @"SELECT * FROM 'SchemaSearch.DbIndices' WHERE dbid =@dbid";
            var dbIndices = await _db.SelectAsync<DbIndex>(sql, new { dbid = dbid });

            sql = @"SELECT * FROM 'SchemaSearch.DbIndicesColumns' WHERE dbid =@dbid";
            var dbIndicesColumns = await _db.SelectAsync<DbIndexColumn>(sql, new { dbid = dbid });

            Dictionary<long, DbObject> dbobjecstByID = dbObjects.ToDictionary(p => p.ObjectId);
            MapDbObjectParents(dbobjecstByID);
            MapdbColumnsParents(dbobjecstByID, dbColumns);
            MapdbIndicesParents(dbobjecstByID, dbIndices);
            MapIndicesColumns(dbIndices, dbIndicesColumns);

            var dbColumnsByTableId = dbColumns.ToLookup(p => p.TableId);
            var dbObjectsTargets = CreateObjectBasedSearchTarget(dbObjects, dbColumnsByTableId);
            var dbColumnsTargets = dbColumns.Select(p => new ColumnSearchTarget(p));
            var dbIndicesTargets = dbIndices.Select(p => new IndexSearchTarget(p)).ToArray();

            var list = new List<ISearchTarget>();
            list.AddRange(dbObjectsTargets);
            list.AddRange(dbColumnsTargets);
            list.AddRange(dbIndicesTargets);
            return list.OrderByDescending(p => p.ModificationDateStr).ToArray();
        }

        private IEnumerable<ISearchTarget> CreateObjectBasedSearchTarget(DbObject[] dbObjects, ILookup<long, DbColumn> dbColumnsByTableId)
        {
            var list = new List<ISearchTarget>();
            foreach (var dbObject in dbObjects)
            {
                var simplifiedType = DbObjectType.Parse(dbObject.Type);

                if (simplifiedType.Category == DbSimplifiedType.Constraint)
                {
                    list.Add(new ConstraintSearchTarget(dbObject));
                }
                else if (simplifiedType.Category == DbSimplifiedType.Table)
                {
                    list.Add(new TableSearchTarget(dbObject, dbColumnsByTableId[dbObject.ObjectId].ToArray()));
                }
                else if (simplifiedType.Category == DbSimplifiedType.Other)
                {
                    list.Add(new OtherSearchTarget(dbObject));
                }
                else list.Add(new ObjectSearchTarget(dbObject));
            }

            return list;
        }

        private static void MapIndicesColumns(DbIndex[] dbIndices, DbIndexColumn[] dbIndicesColumns)
        {
            var columnsByIndexId = dbIndicesColumns.ToLookup(p => ValueTuple.Create(p.OwnerId, p.IndexNumber));
            foreach (var index in dbIndices)
            {
                index.Columns = columnsByIndexId[ValueTuple.Create(index.OwnerId, index.IndexNumber)].ToArray();
                if (index.Columns.Length == 0)
                    throw new Exception(index.Name);
            }
        }

        private void MapDbObjectParents(Dictionary<long, DbObject> dbobjecstByID)
        {
            foreach (var obj in dbobjecstByID.Values)
            {
                if (obj.ParentObjectId.HasValue)
                    obj.Parent = dbobjecstByID[obj.ParentObjectId.Value];
            }
        }

        private void MapdbColumnsParents(Dictionary<long, DbObject> dbobjecstByID, DbColumn[] dbColumns)
        {
            foreach (var column in dbColumns)
            {
                column.Parent = dbobjecstByID[column.TableId];
            }
        }

        private void MapdbIndicesParents(Dictionary<long, DbObject> dbobjecstByID, DbIndex[] dbIndices)
        {
            foreach (var index in dbIndices)
            {
                index.Parent = dbobjecstByID[index.OwnerId];
            }
        }

        private ObjectSearchTarget[] CreateDbObjectsSearchTargets(DbObject[] dbobjecst)
        {
            List<ObjectSearchTarget> list = new List<ObjectSearchTarget>();
            foreach (var obj in dbobjecst)
            {
                var instance = new ObjectSearchTarget(obj);
                list.Add(instance);
            }

            return list.ToArray();
        }

        private ObjectSearchTarget[] CreateDbObjectsSearchTargets(Dictionary<int, DbObject> dbobjecstByID)
        {
            List<ObjectSearchTarget> list = new List<ObjectSearchTarget>();
            foreach (var obj in dbobjecstByID.Values)
            {
                var instance = new ObjectSearchTarget(obj);
                list.Add(instance);
            }

            return list.ToArray();
        }
    }
}
