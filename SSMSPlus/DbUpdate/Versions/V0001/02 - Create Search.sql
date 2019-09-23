
CREATE TABLE 'SchemaSearch.Dbs' 
(
	DbId INTEGER NOT NULL PRIMARY KEY,
	Server TEXT NOT NULL,
	DbName TEXT NOT NULL,
	IndexDateUtc DATETIME NOT NULL
);

CREATE UNIQUE INDEX SchemaSearch_Dbs_Server_DbName ON 'SchemaSearch.Dbs' (Server, DbName);

CREATE TABLE 'SchemaSearch.DbObjects' 
(
	DbId INTEGER NOT NULL,
	ObjectId INTEGER NOT NULL,
	Type TEXT NOT NULL,
	SchemaName TEXT NOT NULL,
	Name TEXT NOT NULL,
	ModificationDate DATETIME NOT NULL,
	Definition TEXT NULL,
	ParentObjectId INTEGER NULL,
	PRIMARY KEY (DbId, ObjectId),
	FOREIGN KEY(DbId) REFERENCES 'SchemaSearch.Dbs'(DbId)
)WITHOUT ROWID;

CREATE TABLE 'SchemaSearch.DbColumns' 
(
	DbId INTEGER NOT NULL,
	TableId INTEGER NOT NULL,
	Name TEXT NOT NULL,
	Datatype TEXT NOT NULL,
	Precision TEXT NULL,
	Definition TEXT NULL,
	PRIMARY KEY (DbId, TableId, Name),
	FOREIGN KEY(DbId) REFERENCES 'SchemaSearch.Dbs'(DbId),
	FOREIGN KEY(DbId,TableId) REFERENCES 'SchemaSearch.DbObjects'(DbId,ObjectId)
)WITHOUT ROWID;

CREATE TABLE 'SchemaSearch.DbIndices' 
(
	DbId INTEGER NOT NULL,
	OwnerId INTEGER NOT NULL,
	IndexNumber INTEGER NOT NULL,
	Name TEXT NOT NULL,
	Type TEXT NOT NULL,
	FilterDefinition TEXT NULL,
	IsUnique INTEGER NOT NULL,
	PRIMARY KEY (DbId, OwnerId, IndexNumber),
	FOREIGN KEY(DbId) REFERENCES 'SchemaSearch.Dbs'(DbId),
	FOREIGN KEY(DbId,OwnerId) REFERENCES 'SchemaSearch.DbObjects'(DbId,ObjectId)
)WITHOUT ROWID;

CREATE TABLE 'SchemaSearch.DbIndicesColumns' 
(
	DbId INTEGER NOT NULL,
	OwnerId INTEGER NOT NULL,
	IndexNumber INTEGER NOT NULL,
	IndexColumnId INTEGER NOT NULL,
	OwnerColumnId INTEGER NOT NULL,
	ColumnName TEXT NOT NULL,
	IsDesc INTEGER NOT NULL,
	Included INTEGER NOT NULL,
	PRIMARY KEY (DbId, OwnerId, IndexNumber,IndexColumnId),
	FOREIGN KEY(DbId) REFERENCES 'SchemaSearch.Dbs'(DbId),
	FOREIGN KEY(DbId,OwnerId,IndexNumber) REFERENCES 'SchemaSearch.DbIndices'(DbId,OwnerId,IndexNumber)
)WITHOUT ROWID;

