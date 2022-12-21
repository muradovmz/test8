
# Configuration Approach

Configuration in ASP.NET Core can be cascaded through different sources. Any variable defined in two sources will be configured based on the order of precedence of source declaration. In below code snippet, the precedence of configuration is as follows.
 - JSON Configuration files like appsettings.json
 - Environment Variables
 - Centralized Configuration like Spring cloud Configuration
 - Key Vault Configuration

# Conventions for configuration variables
For example, if we have config section defined as shown below in `appsettings.json`.
```
  "ConnectionStrings": {
    "{{cookiecutter.ProjectName}}Connection": "Connection info..."
  }
```

If we have to refer this variable in the `environment` section of `docker compose`, we have to use `double underscore (__)` as shown below.
```
environment: 
            - ConnectionStrings__{{cookiecutter.ProjectName}}Connection=${{{cookiecutter.ProjectName}}Connection_ENV_VAL}       
```

If we have to refer this variable in the `Hashicorp Vault`, we have to use `colon (:)` as shown below.
```
ConnectionStrings:{{cookiecutter.ProjectName}}Connection
```

If we have to refer this variable in the `Spring Cloud Config Server`, we can use `yml format` as shown below.
```
ConnectionStrings:
    {{cookiecutter.ProjectName}}Connection: "Connection info..."
```

If we have to refer this variable in the C# Code, we have to use `colon (:)`. `Configuration` is type of `IConfiguration` as shown below.
```
Configuration["ConnectionStrings:{{cookiecutter.ProjectName}}Connection"]
```


# Recommendations and best practices while choosing configuration options
- Build a cascading configuration to ensure proper overriding of config values.
    - Cascading configuration helps in supporting development techniques like containerization, local debugging etc.
- Secure and sensitive configuration values should **not** be committed to source version control.
- Make sure proper conventions are followed while creating and resolving configuration. Grouping of relevant configuration into sections is advisable.
- Store non-secure and immutable configuration in configuration files like `appsettings.json`.
- Environment specific configuration like ASP.NET Core specific variables should be configured in `Environment Variables`.
    - Use `.env` in developer machine to configure environment variables. The `.env` file should not be version controlled.
- Use centralized configuration server like `Spring Cloud Config Server` to store non-secure and environment specific configuration information.
    - Centralized configuration is very helpful in distributed application scenarios where changes to the configuration can be propagated without application downtime.
- Always store confidential secrets, certificates and passwords in secure `Vault` like Hashicorp vault.
- Always prefer Managed service providers like different cloud vendors to provide and support configuration infrastructure.
    - Managed services are reliable and resilient.
- Create proper access policies and identities to ensure security of configuration values.
    - Protection using Managed service identities through service principals is an advisable approach.
- Choosing different configuration providers like files, vault, config server, environment variables and combination of providers should be dependent on the type and scale of the system.
    - Operations and maintenance of the applications plays crucial role in choosing configuration providers.
- Following important points can be beneficial at different phases of software development. 
    - At the time of development, have all the general configuration defined in `appsettings.json` and environment variables defined in `launch.json` of VS Code (or `launchSettings.json` for `dotnet run`) .
    - While testing Docker containers, still have all the general configuration defined in `appsettings.json` but have the environment variables set appropriately. 
        - It is advisable to spin up a container for Vault/Config server to store the keys securely.
        - But if the project is using any managed service (probably from cloud vendors) for vault, we can have those settings defined in environment variables and continue testing in local. This is required to support offline development activities.
    - For higher environments, it is always advisable to use below options in precedence.
        - `appsettings.json`
        - `Environment variables`
        - `Spring Cloud Config Server`
        - `Key Vault` 

For {{cookiecutter.ProjectName}} service, we are using all mentioned configurations - `appsettings.json`, `Environment variables`, `Spring Cloud Configuration`, `Key Vault` to demonstrate the best practices scenarios.

# Setting up the configuration through configuration files

- Configuration is loaded from `appsettings.json` in `JSON` format. 
- By default, Configuration is loaded on application start into `IConfiguration`. If we need to explicitly load the configuration, then we can use `AddJsonFile` extension of `IConfigurationBuilder` in `ConfigureAppConfiguration` extension of `IHostBuilder` in `Program.cs`.
- Config values can be categorized into multiple sections.
- Configuration sections can be mapped to the strongly types classes in `ConfigureServices` method of `Startup.cs`. Please refer `ApiSettingsExtensions.cs`.
# Setting up the configuration through ENV Variables

- Configuration can be set in the `Environment variables` on the machine where application is hosted. 
- By default, Application is configured to use environment variables. If we need to explicitly add them, use `AddEnvironmentVariables` extension of `IConfigurationBuilder` in `ConfigureAppConfiguration` extension of `IHostBuilder` in `Program.cs`.

### Important points to remember while setting up environment variables
The starter kit is dependent on different configuration parameters. Going forward, the environment variables can be set at different places based on the execution tool.
  - In `{{cookiecutter.ProjectName}}.API/Properties/launchSettings.json` or `terminal`, while running the app locally using `dotnet run`. 
  - In `.vscode/launch.json`, while running the app locally using `VS Code Debugger`. 
  - In `localDevSetup/.env`, while running through `Docker compose`. 
  
> #### `.env` and `launch.json` files are not present in the repository, they should be manually created in local machine and should not be committed to repository with sensitive information. `launchSettings.json` is present in the repo and it should not be committed with secret and sensitive information.


## Setting up the ENV variables for {{cookiecutter.ProjectName}} Service Infrastructure through Docker Compose
- While using `Docker compose`, create the `.env` file is present in `service-dotnet-starter\localDevSetup` directory. (Read about [Important points to remember while setting up environment variables](#Important-points-to-remember-while-setting-up-environment-variables))

> Variables with default values placed in `.env` file. Replace them if required with appropriate values.

## Setting up the ENV variables for {{cookiecutter.ProjectName}} Service Application through Docker Compose
While using `Docker compose`, create the `.env` file in `service-dotnet-starter\localDevSetup` directory. Update the following settings to reflect your local development environment. (Read about [Important points to remember while setting up environment variables](#Important-points-to-remember-while-setting-up-environment-variables))

- ASP.NET Core variables
    - **ASPNETCORE_ENVIRONMENT_VAL**
        - Value can be Development, Production etc. Development value is required while running Docker compose in local.
    - **ASPNETCORE_URLS_VAL**
        - The internal container port where {{cookiecutter.ProjectName}} service is hosted (it should be http://*:8080).

- Hashicorp Vault Settings can be obtained by completing [Key Vault Integration set up](#Set-up-configuration-at-Hashicorp-Vault-for-{{cookiecutter.ProjectName}}-Service-through-Docker-container).
    - **Vault_Enabled_VAL**
        - True/False to enable Vault integration.
    - **Vault_Uri_VAL**
        - The URL for the vault, something like http://vault:8200 where domain is the vault service name in the docker compose.
     - **Vault_RoleId_VAL**
        - The role id through which vault can be accessed (for example : c8533179-7f9b-507a-7e72-5c56db93ded6).
    - **Vault_SecretId_VAL**
        - The secret id through which vault can be accessed (for example : cdb04f9f-0d43-934a-3405-6582cbe4321a).

- Spring Cloud Config Server Settings can be obtained by completing [Spring Cloud Config Server Container set up](#Set-up-Spring-Cloud-Config-Server-for-{{cookiecutter.ProjectName}}-Service-through-Docker-container).
    - **Spring_Enabled_VAL**
        - True/False to enable config server integration.
    - **Spring_Application_Name_VAL**
        - Name of the application configuration which client is requesting from config server. It should follow the convention based on configuration on server.
    - **Spring_Cloud_Config_Uri_VAL**
        - The URL for the config server, something like http://configserver:8888 where domain is the vault service name in the docker compose.


# Set up configuration through centralized configuration server
In distributed system design, application configuration plays an important role not only in collaborating different applications into workflows through integrations, but also improves the development teams to deliver new features to customer in more agile way through feature toggles. Configuration helps application to safely turn on/off the experimental features, performance optimizations, workflows versions etc. Traditionally most the application rely on physical files to hold the business and system configuration. Even though the traditional configuration methodologies gave good control of applications runtime dynamics to developers, the modern externalizing configuration approach with a centralized config server provide much better control on applications without downtime.

## Recommendations and best practices while choosing a centralized configuration server
- The config server should support different configuration backends like filesystem, git version control, vault, Redis, JDBC etc.
- Config server should support different config formats JSON, YML, Properties etc.
- Config sever should be capable of notifying applications about configuration changes, so that applications can pull the configuration without downtime. (Currently this is not possible with Steeltoe .NET package)
- Config server should be capable of securing the configuration through different authentication mechanisms like basic authentication, oauth protocols etc.
- Conventions based configuration for different environments should be supported.
- Encryption and Decryption should be supported for the configuration.
- Config server should be customizable either through configuration or through code.
- It should provide health metrics and logs.

## Set up Spring Cloud Config Server for {{cookiecutter.ProjectName}} Service through Docker container

Spring cloud config server infrastructure is provisioned through `Docker Compose`, refer to  [Docker Compose Documentation](containerization.md). 
 - Spring cloud config server be disabled/enabled by setting `Spring_Enabled_VAL` flag to `false/true` in `.env` file.
 - Config server is capable of working with filesystem and git based backends.
    - For {{cookiecutter.ProjectName}} service, we will use filesystem backend which is configured in `docker-compose.infra.yml` with `SPRING_PROFILES_ACTIVE=native`.
 - Config server is provisioned with basic authentication. The username and password is configured through `Spring_Cloud_Config_Username_VAL` and `Spring_Cloud_Config_Password_VAL` of `.env` file.

 > The {{cookiecutter.ProjectName}} service configuration is loaded in `localDevSetup\configserver\config\{{cookiecutter.ProjectName}}service-development.yml`. The files follows a convention of `{applicationname}-{profile}.yml`.

Navigate to `localDevSetup\configserver\config\{{cookiecutter.ProjectName}}service-development.yml` and change the setting accordingly.
Default Configuration can be found below.
```
AppSettings:
    Source: {{cookiecutter.ProjectName}}Source
LogstashConfiguration:
    Enabled: true
    Uri: tcp://localhost:8080
Jaeger: 
    ServiceName: jaeger
    AgentHost: localhost
    AgentPort: 6831
WebClientSettings:
    Timeout: 100
Vault:
    Enabled: true
Serilog: 
    MinimumLevel:
        Default: Debug
        Override:
            Microsoft: Warning
            System: Warning
```

## {{cookiecutter.ProjectName}} Service code changes for integration with Config Server
Install `Steeltoe.Extensions.Configuration.ConfigServerCore` nuget package on {{cookiecutter.ProjectName}} service as shown below.
```
<PackageReference Include="Steeltoe.Extensions.Configuration.ConfigServerCore" Version= "2.1.0"/>
```
> Refer `ApiConfigurationExtensions.cs` for code details


