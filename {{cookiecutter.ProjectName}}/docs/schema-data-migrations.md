# Datastore Migrations Approach
Following are the major areas where migrations play crucial role in development and deployment life cycle.
1. Schema changes (specifically DDL statements like create/alter tables, indexes etc.).
2. Stored procedures and functions
3. Seed data (seeding master data like countries, zip codes etc.)
4. Data transformation because of schema changes (for example, restructuring a table into multiple tables etc.)
5. ETL Pipelines (migrating transactional data from on-prem to cloud etc.)

**NOTE:** ETL Migrations (#5 in the above list) is out of scope of this Starter kit.

The primary key factors which play a major role in selection of a migration strategy are as follows.
1. System topology (polyglot)
2. Datastore choices
3. Deployment strategies

In short, migration strategies vary between different use cases. The migration strategy opted for an application which is dependent on NoSQL store cannot be leveraged for an application with a SQL backend.

At a strategy level, migrations can be created using one of the below options.
1. Dotnet EF Core migrations
2. SQL Scripts containing Stored procedures, Functions etc.
2. Generate SQL scripts (or commands) through custom utilities from data files like CSV, flat files etc.

## SQL Migration Strategy Recommendations
- It is highly recommended to opt for a migration strategy where human intervention factor is very minimal. 
- It is advisable to create EF Core migrations for schema changes where structural modifications needs to be deployed.
    - Dotnet EF Tools can be leveraged to automate incremental deployments.
- Stored procedures and functions should be written and versioned as SQL files. 
    - EF Core custom migrations should be leveraged to deploy these SQL Files. 
    - Whenever a SP or Function is changed, developer should create a custom migration and commit the migration to the source code repository. This way that migration can be deployed automatically through Dotnet EF Tools.
- Generation of SQL scripts (or commands) from data files like CSV, flat file etc., is recommended for seed data migrations.
    - Custom utilities can be created with any choice of technology or language. These utilities can read the seed data from the data files, transform the data as per the target data structure and finally create the script file in a format which can be deployed to the target.
    - These script files should be versioned based on Release, Build and Date of creation.
    - Deployment platforms should be capable of picking up these scripts and deploy them to targets through deployment pipelines.
- SQL Scripts should be created by the developers whenever existing datastore is changed (for example splitting a table into multiple tables based on Normalization forms).
    - EF Core custom migrations should be leveraged to deploy these SQL Files. This way that migration can be deployed automatically through Dotnet EF Tools.
- Every migration should have `Up` and `Down` strategies to support commit and rollback scenarios.

## Recommendations for deploying SQL migrations
- On local development machine, automatic deployment of migrations should be configured on application start. This will ensure exceptions or misleading behaviours are caught as early as possible. 
    - Automating the DB deployment on application start is possible with Dotnet EF Tools. 
    - If migrations cannot be executed in app start (for example, running custom seed data scripts), it is advised to create a bash/powershell script which will automate the process.
    - This custom script should keep track of SQL scripts it ran, so that it can always work with differential database updates. 
- For production workloads, migrations should be deployed through CI/CD pipeline through Dotnet EF Tools. This will facilitate error and hassle free deployments of distributed system where different applications are deployed to environments. 
    - Custom tasks should be incorporated in the deployment pipeline to deploy the seed data scripts.
    - This will also helps in deploying the migrations to multiple regions.

## Recommendations for deploying NoSQL migrations
- NoSQL Schema changes can be deployed at runtime through the respective datastore SDKs.
    - Centralizing the application data model through API like `CreateIfNotExists` is preferred approach for making NoSQL schema changes.
- Seed data can be imported through custom utilities written with the help of respective datastore SDKs.

## Custom Data Ingestion API
- Custom data ingestion API should be developed to perform exclusive data management operations.
    - This API can be leveraged by Admin users to perform basic data management tasks.
    - Custom applications can be developed using this API to expose data interface for other internal applications.
    - Security of this API should be planned ahead of implementation.

# Code Sample for EF Core Migrations 
 In this section, we will see how to use Dotnet EF Tools to create migrations for Schema changes and SQL Scripts (for Stored Procedures, Functions, custom scripts to change data). This official documentation can be found at [Dotnet ef migrations documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli).

## Setting up environment to create database migrations
- Make sure the SQLServer Connection is set at `ConnectionStrings__{{cookiecutter.ProjectName}}Connection` environment variable.
```
$>> export ConnectionStrings__{{cookiecutter.ProjectName}}Connection='Connection string goes here...'
```

- Install `Dotnet EF Tools` by executing below command in **VS Code terminal** (under {{cookiecutter.ProjectName}}.Persistence).
```
$>> dotnet tool install dotnet-ef --version 6.0.1
```

## How to create new migrations using Dotnet EF Tools

- Make changes (create or update data model) to the `{{cookiecutter.ProjectName}}DBContext`. 
- While in **{{cookiecutter.ProjectName}}.Persistence** folder, create a migration by executing below command.

```
dotnet ef --startup-project ../{{cookiecutter.ProjectName}}.Api/ migrations add MigrationName
```

## Execution of migrations using Dotnet EF Tools
While in **{{cookiecutter.ProjectName}}.Persistence** folder, migrations can be executed through below command.

```
dotnet ef database update
```

## Dotnet EF Tools Migrations for SQL Scripts
- Create the data script in **Migrations/SQLScripts** (of {{cookiecutter.ProjectName}}Service). The naming convention which can be followed is `{TableName}Data-{mmddyyyy}-{index (order in the day)}`. For example `BankInfoData-03312020-1.sql`

- **NOTE**: **SQLScripts** folder should be marked with **CopyToOutputDirectory** to true in the **csproj** file.

- Create the migration as shown below. It is not necessary to give SqlFileName as the name of the migration. 
```
dotnet ef --startup-project ../{{cookiecutter.ProjectName}}.Api/ migrations add SqlFileName

For example `dotnet ef --startup-project ../{{cookiecutter.ProjectName}}.Api/ migrations add BankInfoData-03312020-1`
```

- Open generated Migration class and write below code to integrate the migration with SQL file.
```
    var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Migrations/SQLScripts/BankInfoData-03312020-1.sql"); 
    migrationBuilder.Sql(File.ReadAllText(sqlFile));
```

- Test the migration in local by executing below command.
```
dotnet ef database update
```

# Code Sample for SQL Script based Seed data Migrations 

## To be implemented (mostly covered in CI/CD section)