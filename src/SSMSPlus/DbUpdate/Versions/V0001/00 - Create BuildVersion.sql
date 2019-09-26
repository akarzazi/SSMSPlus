CREATE TABLE 'SSMSPlus.BuildVersion' 
(
	BuildNumber INTEGER NOT NULL PRIMARY KEY,
	InstallDateUtc DATETIME NOT NULL
);

CREATE TABLE 'SSMSPlus.BuildVersionScripts' 
(
	BuildNumber INTEGER NOT NULL,
	ScriptName TEXT NOT NULL,
	ScriptContent TEXT NOT NULL,
	InstallDateUtc DATETIME NOT NULL,
	PRIMARY KEY (BuildNumber, ScriptName),
	FOREIGN KEY(BuildNumber) REFERENCES 'SSMSPlus.BuildVersion'(BuildNumber)
);