# SSMSPlus
SSMS Plus is a productivity extension for SQL Server Management Studio 18 / 19.

It extends SSMS with a handful set of features:

    Query Execution History
    Schema Object Search
    Document Export for binary columns


### Table Of Contents

- [Getting started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Download](#download)
  * [Install](#install)
  * [Update](#update)
  * [Launch](#launch)
- [Features](#features)
  * [Query History](#query-history)
  * [Schema Object Search](#schema-object-search)
  * [Documents Export](#documents-export)
- [Contributing](#contributing)
  * [How can I contribute ?](#how-can-i-contribute--)
  * [Project Goals](#project-goals)
    + [Goals](#goals)
    + [Non Goals](#non-goals)
  * [Debugging](#debugging)
    + [Debug the SSMS instance](#debug-the-ssms-instance)
    + [The Demo project](#the-demo-project)
    + [Troubleshooting](#troubleshooting)


# Getting started
## Prerequisites
SQL Server Management Studio 18 / 19

## Download
### For SSMS 19

Get the latest build from the [Releases](https://github.com/akarzazi/SSMSPlus/releases) page.

### For SSMS 18

Get this release: 
https://github.com/akarzazi/SSMSPlus/releases/tag/4.0

## Install

> SQL Server Management Studio extensions cannot be installed via VSIX Installer under SSMS 19.x. See
> https://docs.microsoft.com/en-us/sql/ssms/install-extensions-in-sql-server-management-studio-ssms?view=sql-server-ver15|

The release is an SFX package, extract the archive content to the SSMS install location.

On the extract dialog, fill the path of the extensions directory.

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Extensions`

![SSMS Plus Extension dlls](docs/illustrations/install-sfx-extract.png?raw=true "SSMS Plus Extension dlls")

**Note:** you might need to extract the SFX as admin

It should create an `SSMSPlus` folder like the following.
![SSMS Plus Extension dlls](docs/illustrations/install-folder-screen.png?raw=true "SSMS Plus Extension dlls")

## Update

Close SQL Server Management Studio.

Delete the SSMSPlus extension directory:

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Extensions\SSMSPlus`

Follow the install steps.

## Launch
A new top menu "SSMS Plus" will be available.

![SSMS Plus Menu](docs/illustrations/menu-screen.png?raw=true "SSMS Plus Menu")

# Features

## Query History

Every time you execute a query through the editor, SSMS Plus will save that query.

You'll find your query history through the main menu

![SSMS Plus Query History](docs/illustrations/history-screen.png?raw=true "Query History")

## Schema Object Search

Use this tool to find quickly any user object in the database.

![SSMS Plus Schema Object Search](docs/illustrations/schema-search-screen.png?raw=true "Schema Object Search")

## Documents Export

This tool is handy when you want to preview or export files from the database.

The query is expecting two columns corresponding respectively to file name and file content.

![SSMS Plus Documents Export](docs/illustrations/document-export-screen.png?raw=true "Documents Export")

# Contributing

We're glad to know you're interested in the project.

Your contributions are welcome !

## How can I contribute ?

You can contribute in the following ways : 

* Report an issue / Suggest a feature.
* Create a pull request.

## Project Goals
### Goals
* Performance: Provide a **lightweight** plugin to bring to SSMS those missing features.
* Performance: React to events in an async and **non blocking** way.
* Minimal:  Focus on **handy** features that matter for the community.
* Stable: Do not implement features using **`Reflection`** heavy stuff.
* Ever Green: Focus on the latest SSMS Major Editions.

### Non Goals
* Implement domain specific features. (ex: integrate with FTP, Azure Services)
* Implement heavy features like intellisense.
* Seek for compatibility with an old edition of SQL Server or SSMS.


## Debugging

### Debug the SSMS instance

This section explains how to setup Visual Studio to debug the plugin within an SSMS Instance. 

1.  In the Debug section of the main project "SSMSPlus", setup your SSMS path as the startup program.

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Ssms.exe`

![SSMS Plus Debug Startup](docs/illustrations/debug-vs-startup.png?raw=true "Documents Export")

2.  In the VSIX section of the main project "SSMSPlus", configure the deployment path for the plugin. 

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Extensions\SSMSPlus`
![SSMS Plus Documents Export](docs/illustrations/debug-vs-copy-vsix.png?raw=true "Documents Export")

3. That's All ! You may start your the debugging session.

### The Demo project

A [Demo project](src/Demo) is available in the sources.

It is handy for faster UI prototyping, since it does not require to launch SSMS. 

### Troubleshooting

A log record in the folder corresponding to `Environment.SpecialFolder.LocalApplicationData + "\SSMS Plus"` which corresponds generally to `C:\Users\<USER>\AppData\Local\SSMS Plus`.

Theses logs are very useful for diagnostics.
