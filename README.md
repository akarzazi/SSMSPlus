# SSMSPlus
SQL Server Management Studio (SSMS) Extension

SSMS Plus is a productivity extension for SQL Server Management Studio 18.

It bring the following features:

    Query Execution History
    Schema Object Search
    Document Export for binary columns

Read more about [Features](#Features)

## Prerequisites
SQL Server Management Studio 18

## Download
Grap the latest build archive from the [Releases](https://github.com/akarzazi/SSMSPlus/releases) page.

## Install
Extract the archive content to the SSMS install location :

C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Extensions

It should look like the following.
![Alt text](docs/illustrations/install-folder-screen.png?raw=true "Title")

## Usage
A new top menu "SSMS Plus" will be displayed.

![Alt text](docs/illustrations/menu-screen.png?raw=true "Title")

## <a id="Features"></a>Features ##

### Query History

Every time you execute a query through the editor, SSMS Plus will save that query.

You can find your query history from the main menu :

![Alt text](docs/illustrations/history-screen.png?raw=true "Title")

### Schema Object Search

Use this tool to find quickly any user object in the database.

![Alt text](docs/illustrations/schema-search-screen.png?raw=true "Title")

### Documents Export

This tool is handy when you want to preview or export files from the database.

The query is expecting two columns corresponding respectively to file name and file content.

![Alt text](docs/illustrations/document-export-screen.png?raw=true "Title")


