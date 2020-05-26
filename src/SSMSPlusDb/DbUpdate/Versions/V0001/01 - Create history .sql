﻿CREATE TABLE 'History.QueryItem' 
(
	Id INTEGER NOT NULL PRIMARY KEY,
	ExecutionDateUtc DATETIME NOT NULL,
	Query TEXT NOT NULL,
	Server TEXT NOT NULL,
	Database TEXT NOT NULL
);