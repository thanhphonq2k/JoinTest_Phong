# Multiple DbContext Demo

A sample project to show multiple EF DbContext working together.

## How To Run

* Open project in Visual Studio 2017+.
* Select Mgm.Web as start project.

### Run Migrations

* Open Package Manager Console, select Mgm.EntityFramework as default project.
* Run command to create first db: Update-Database -ConfigurationTypeName "Mgm.Migrations.Configuration"
* Run command to create second db: Update-Database -ConfigurationTypeName "Mgm.MigrationsSecond.Configuration"

### Run Application

* Run application with F5.
* You can see peole and course list.
* You can create a new person here.

### Notes

* MSDTC (Distributed Transaction Coordinator) should be running on your computer.
