# SSMSPlus
SSMS Plus is a productivity extension for SQL Server Management Studio 18.

It extends SSMS with a handful set of features:

    Query Execution History
    Schema Object Search
    Document Export for binary columns


### Table Of Contents

- [Getting started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Download](#download)
  * [Install](#install)
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
SQL Server Management Studio 18

## Download
Grap the latest build archive from the [Releases](https://github.com/akarzazi/SSMSPlus/releases) page.

## Install
Extract the archive content to the SSMS install location :

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Extensions\SSMSPlus`

It should look like the following.
![SSMS Plus Extension dlls](docs/illustrations/install-folder-screen.png?raw=true "SSMS Plus Extension dlls")

**Note:** you might need to extract the SFX as admin, if you don't have write access to the ssms extensions folder
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
* Performance: Provide a **lightweight** plugin to bring to SSMS thoses missing features.
* Performance: React to events in an async and **non blocking** way.
* Minimal:  Focus on **handy** features that matter for the community.
* Stable: Do not implement features using **`Reflection`** heavy stuff.
* Ever Green: Focus on the latest SSMS Major Editions.

### Non Goals
* Implement domain specific features. (ex: integrate with FTP, Azure Services)
* Implement heavy or obstrusive features like intellisence.
* Seek for compatibility with an old edtion of SQL Server or SSMS.


## Debugging

### Debug the SSMS instance

This section explains how to setup Visual Studio to debug the plugin within an SSMS Instance. 

1.  In the Debug section of the main project "SSMSPlus", setup your SSMS path as the startup program.

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe`

![SSMS Plus Debug Startup](docs/illustrations/debug-vs-startup.png?raw=true "Documents Export")

2.  In the VSIX section of the main project "SSMSPlus", configure the deployement path for the plugin. 

`C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Extensions\SSMSPlus`
![SSMS Plus Documents Export](docs/illustrations/debug-vs-copy-vsix.png?raw=true "Documents Export")

3. That's All ! You may start your the debugging session.

### The Demo project

A [Demo project](src/Demo) is available in the sources.

It is handy for faster UI prototyping, since it does not require to launch SSMS. 

### Troubleshooting

A log record in the folder corresponding to `Environment.SpecialFolder.LocalApplicationData + "\SSMS Plus"` which corresponds generally to `C:\Users\<USER>\AppData\Local\SSMS Plus`.

Theses logs are very useful for diagnostics.