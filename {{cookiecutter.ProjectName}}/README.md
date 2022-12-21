# Dotnet Starter
[![Build & Test](https://github.com/Regional-IT-India/catalyst-service-dotnetcore-starter/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/Regional-IT-India/catalyst-service-dotnetcore-starter/actions/workflows/build.yml)

![Status](https://github.com/twCatalyst/getting-started/blob/main/badges/stable.svg)

## Table Of Contents

- [Introduction](#introduction)
    - [Architecture](#architecture)
    - [Aspects](#aspects)
- [Setup instructions](#setup-instructions)
    - [Prerequisites](#prerequisites)
    - [Step 1 : Clone the repo](#step-1--clone-the-repo)
    - [Step 2 : Initialize the Infrastructure using Docker Compos](#step-2--initialize-the-infrastructure-using-docker-compose)
    - [Step 3 : Configure the Infrastructure](#step-3--configure-the-infrastructure)
    - [Step 3 : Configure the Infrastructure](#step-3--configure-the-infrastructure)
    - [Step 4 : Run {{cookiecutter.ProjectName}} Service](#step-4--run-{{cookiecutter.ProjectName}}-service)
    - [Step 5 : [Optional] Configure Keycloak For Authentication](#step-5--optional-configure-keycloak-for-authentication)
    - [Step 6 : Testing](#step-6--testing)
    - [Step 7 : Clean Up](#step-7--clean-up)
- [Appendix](#Appendix)

## Introduction

* .NET catalyst project has been build in with all established and recommended practices. It covers following aspects
    * Spread common pattern and practices followed in the community
    * An opinionated code structure along with recommended coding practices
    * A set of recommended libraries
    * Provide default extendable implementation with tools such as code coverage, documentation, metrics, logging and
      security
    * A decent local dev set up
    * Optimise iteration 0 and bootstrap time for a project while not compromises of established practices

### Architecture

For architecture design designs and considerations, please refer [architecture doc](docs/architecture.md)

### Aspects

**.NET catalyst project has covered following high level aspects of a microservice**

- [x] Developer Machine setup
- [x] Clean Architecture, DDD and CQRS based implementation
- [x] Authentication and Authorization
- [x] Metrics visualization
- [x] Application Secret Management
- [x] Centralized configuration
- [x] Log aggregation and visualization
- [x] Centralized Error Handling
- [x] Structured logging
- [x] Metrics collection & custom metrics
- [x] Distributed Tracing
- [x] Efficient Validations
- [x] Data Mappers
- [x] Swagger API documentation
- [x] Unit tests
- [x] Component Tests
- [x] Dependencies vulnerability checks
- [x] CI pipeline

## Setup instructions

### Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- **Docker and Docker Compose**

### **Step 1 : Clone the repo**

* `git clone git@github.com:Regional-IT-India/catalyst-service-dotnetcore-starter.git`

### **Step 2 : Initialize the Infrastructure using Docker Compose**

* Default environment values are present at `/localDevSetup/.env` file (Modify them as per the requirement).
* If docker desktop is not installed, then you may use [**colima**](./docs/colima.md).
* Run the below commands to set correct permissions on all required `.sh` files in the project.
    * `cd tools`
    * `chmod 755 ./set-up-permissions-for-all-scripts.sh`
    * `./set-up-permissions-for-all-scripts.sh`
    * `cd ..`
* Run below command to run all containers for all required infra, integration and dependencies
    * `cd localDevSetup`
    * `./all-infra-and-dependencies.sh up`
* Once above command is successful, we can access tools at below addresses
    * Keycloak : http://localhost:8000/auth/
    * Config Server
        * http://localhost:8888/{{cookiecutter.ProjectName}}service/development
        * http://localhost:8888/{{cookiecutter.ProjectName}}service/demo
    * Vault : http://localhost:8200/ui/vault/secrets
    * Jaeger : http://localhost:16686/search
    * Grafana
        * http://localhost:3000/
        * Credential (admin, admin)
    * Prometheus : http://localhost:9090/
    * Kibana : http://localhost:5601/
* Please refer [Containerization Approach](docs/containerization.md) for details on custom configurations and best
  practices.

### **Step 3 : Configure the Infrastructure**

* **Configure Vault**
    * {{cookiecutter.ProjectName}} Service configuration is configured at Vault using `localDevSetup/vault/configure.sh` script.
  
    * Run below commands
        * `cd vault`
        * `./configure.sh`
     
        Note: After running the above command, you will find UNSEAL_KEY, ROOT_TOKEN, ROLE_ID, SECRET_ID in the console output. Please take a note of these values.
        * `Unseal Key` and `Root Token` will be required to access Vault from UI and to unseal Vault.
        * `Role Id` and `Secret Id` will be required to access app secrets from Vault.
        
        Run the following command to push the secrets to vault.
        * `export KEY=<your-16-characters-key>`
        * `export VAULT_TOKEN=<your-vault-root-token>` # generated by `./configure.sh`
        * `./setup.sh`
        * `cd ..`
      Please refer [Vault](docs/vault.md) for more information on vault configuration, best practices and how to access
          vault UI.

### **Step 4 : Run {{cookiecutter.ProjectName}} Service**

* Ensure `Vault` is unseal, Please refer [Configure Vault](#step-3--configure-the-infrastructure)
* **4.1 : Through Docker**
    * Goto to file [.env](localDevSetup/.env) file, and update following values
        * `Vault_RoleId_VAL` and `Vault_SecretId_VAL` with values you stored from vault set up.
        * Run `./app.sh up`
            * This will build `{{cookiecutter.ProjectName}}Service` docker and run it at `localhost:5000`
* **4.2 : Through Dotnet Command**
    * Goto [{{cookiecutter.ProjectName}}.Api](src/{{cookiecutter.ProjectName}}.Api) by `cd src/{{cookiecutter.ProjectName}}.Api`
    * Goto to file appsettings.json at `src/{{cookiecutter.ProjectName}}.Api/appsettings.json` file, and update following values
        * `Vault_RoleId` and `Vault_SecretId` with values you stored from vault set up.
        * Update `Cloud.Config.UserName` with `admin` and `Cloud.Config.Password` with `admin123`.
        * Run `dotnet build && dotnet run`
            * [launchSettings.json](/src/{{cookiecutter.ProjectName}}.Api/Properties/launchSettings.json) has
              default `"ASPNETCORE_ENVIRONMENT": "Development"`. Ensure, it is there, else configuration will not picked
              form config server.
            * `{{cookiecutter.ProjectName}}Service` is available `localhost:5000`
    * Due to docker networking, few integration like Jaeger and Prometheus may not work when app is running in non docker mode.  

### **Step 5 : [Optional] Configure Keycloak For Authentication**

* ` Keycloak` container is already up in [Setp-2](#step-2--initialize-the-infrastructure-using-docker-compose), but by
  default auth in not enabled in the app.
* If you want to have authentication, please refer [KeyCloak Setup](docs/keycloak-gettingstarted.md) to setup
  authentication
* If Running app in docker, go to [.env](localDevSetup/.env) file, and update following values
    * Set `Auth_Enabled_VAL=true`
    * Update `Auth_OAuthClientSecret_VAL` with valid value form `Keycloak` server.
    * Run `./app.sh down` and then `./app.sh up`
* If Running app through  `dotnet run`
    * Update `Auth` section in appsettings.json at `src/{{cookiecutter.ProjectName}}.Api/appsettings.json` and rerun the app.

### **Step 6 : Testing**

For testing of {{cookiecutter.ProjectName}} API, please refer [Testing {{cookiecutter.ProjectName}} Service API documentation](docs/test-{{cookiecutter.ProjectName}}-service.md).

### **Step 7 : Clean Up**

To clean up any docker resources

* Go to `localDevSetup`
* To clean up infra, run `./all-infra-and-dependencies.sh down`
* To clean up app, run `./app.sh down`
* To clean up all containers, volumes, run `./clean.sh`

## Appendix

- For OpenAPI Specification, please refer [OpenAPI Specification documentation](docs/openapi-spec.md).
- Datastore (both schema and data) migration approaches are explained
  at [Schema and Data migrations documentation](docs/schema-data-migrations.md).
- Containerization approaches are explained at [Containerization documentation](docs/containerization.md).
- For structured Logging Practices with ELK Stack, please refer [Logging documentation](docs/logging.md).
- For different application configuration approaches including Environment Variables, Config, please
  refer [Configuration documentation](docs/configuration.md).
- For different code quality metrics analysis using tools like SonarQube, Snyk, please refer [Code Quality documentation](docs/code-quality.md).
- For common coding best practices, please refer [Common Practices documentation](docs/common-practices.md).
- Approach for capturing application telemetry data can be found at [Metrics documentation](docs/metrics.md).
- For general troubleshooting, please refer [Troubleshooting](docs/troubleshooting.md)
- There are few defaults tools and scripts added to catalyst for general set up, please refer [Tools](docs/tools.md).
- For CI workflows using github actions, please refer [CI-Workflows](docs/ci-workflows.md).
- For Jaeger, please refer [Metrics](docs/metrics.md)
- To explore, how to set up different authentication schemes, please refer
    - [Azure-AD Set Up](docs/azureAD-gettingstarted.md)
    - [Auth0 Set Up](docs/auth0-gettingstarted.md)

