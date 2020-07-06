-- Clear all indices
DELETE FROM 'SchemaSearch.DbIndicesColumns'
DELETE FROM 'SchemaSearch.DbIndices'
DELETE FROM 'SchemaSearch.DbColumns'
DELETE FROM 'SchemaSearch.DbObjects'
DELETE FROM 'SchemaSearch.Dbs'

-- Add scale as INTEGER & switch precision to INTEGER
DROP TABLE IF EXISTS 'SchemaSearch.DbColumns' 
CREATE TABLE 'SchemaSearch.DbColumns' 
(
	DbId INTEGER NOT NULL,
	TableId INTEGER NOT NULL,
	Name TEXT NOT NULL,
	Datatype TEXT NOT NULL,
	Precision INTEGER NULL,
	Scale INTEGER NULL,
	Definition TEXT NULL,
	PRIMARY KEY (DbId, TableId, Name),
	FOREIGN KEY(DbId) REFERENCES 'SchemaSearch.Dbs'(DbId),
	FOREIGN KEY(DbId,TableId) REFERENCES 'SchemaSearch.DbObjects'(DbId,ObjectId)
)WITHOUT ROWID;