
# Containerization Approach
Following are the major areas where containerization techniques play crucial role in development and deployment life cycle.
1. System Modernization Process
2. Polyglot Microservices Architectures
3. Distributed Systems Design
4. Cloud agnostic development and deployment strategies

There are many built in advantages with a container platform like Docker. Few of the notable advantages are as follows. 
1. Environment standardization
2. Efficient resource consumption
3. Highly scalable and resilient services
4. Application isolation
5. Consistency in Build and Release pipelines
6. Effective Configuration
7. Rapid Development
8. Quick onboarding of new technical team members
9. Support for container orchestrators

There are many other advantages where containerization concepts deliver value compared to the traditional development methodologies. We can get more advantages from containers when we are trying to achieve Non-functional requirements ([Quality Attributes](https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ee658094%28v%3dpandp.10%29)). 

## Docker Recommendations and Best practices
- One should have proper understanding of Docker container topology to better plan the docker images for application (for example windows containers can only run on Windows machines, but linux containers can work on both Windows, Mac, Linux distros).
    - There can be performance issues when developing applications using docker on local machines because of the VM abstraction through which docker gets the access to host kernel .
    - Even though docker containers are light-weight compared with VMs, but they are less isolated with host OS and more prone to issues.
- Do not complicate the application development with Docker containers unless it is truly required.
    - If we are developing simple web application or API where we do not have critical integrations with different other applications, then containers can be an overkill.
    - Docker helps debugging of large scale systems, but to debug a simple API, docker might be adding extra complexity.
- Even though containers can be used to deliver stateful services with the help of volumes, it is recommended against to this development model in some cases.
    - Avoid maintaining databases and data stores in containers. It is advised to subscribe to cloud vendors who provide data stores as services in different subscription models.
    - In case of on-prem infrastructure, it still advisable to run databases on dedicated servers rather than in Docker containers. Hence reducing the complexity and pitfalls of loosing production data.
- Avoid storing application data on the container. If the container goes down, data goes down.
- Docker recommends the use of volumes over the use of binds, as volumes are created and managed by docker and binds have a lot more potential of failure.
- Avoid manual configuration of services (like load balancers, security etc.) provided by Docker containers, offload this activity to orchestrators like Kubernetes.
- It is recommended to create `Docker Compose` of multi application deployments instead of working with individual Docker files.
    - Each application (typically a Docker compose service) should have its own Docker file. Docker compose should internally refer these docker files. 
- `Dockerfile` best practices can be found at this [official documentation](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/). The most important best practice is to keep the size of the image smaller.
    - Leverage build cache by adding the layers which are not often changing to the top of the dockerfile.
    - Use Multi-stage builds.
    - Run one application per container.
    - Keep (and copy) only required components in the dockerfile layers.
    - Build smallest image possible.
    - Tag images properly with Build/Release numbers.
    - Always use public images from authentic sources.
    - Create Multi-Arch images (based on infrastructure choices).
    - Run the container with least privileged user.
    - Use labels to specify metadata.
    - Add `.dockerignore` file to ignore files from build context.
    - Leverage Docker enterprise features for additional protection


## Recommendations for deploying Docker containers.
- When a change is committed to source control, use CI/CD pipeline to automatically build and tag a Docker image. Store the images in a centralized registry like Docker Hub, Azure Container Registry. Github packages etc.
    - Continuos Delivery techniques should be leveraged to deploy the containers to the target environment.
- Automate the process of signing the docker image using Docker Content Trust (DCT).
- Protect against vulnerabilities by using tools such as [Snyk](https://snyk.io/) in the build pipeline to continuously scan and monitor for vulnerabilities that may exist across all of the Docker image layers that are in use.
- Sensitive information (application secrets, SQL connection information, tokens etc.) should not be stored or mentioned in the docker compose or docker files.
    - The Developer (local environment) Secrets should be configured in `.env` file.
    - Production (or any higher environment) Secrets should be maintained at Key Vault like Azure Key Vault, Amazon Key Management Service, HashiCorp Vault etc. Both the Kay Vault and Application should be configured to have trust (or identity) through which application can read secrets which are provisioned in the vault. To decide upon a configuration approach, please read the [Configuration documentation](configuration.md).


# Containerization of {{cookiecutter.ProjectName}} Service using Docker platform
- We are using Docker compose `3.7`, we should have Docker Engine `18.06.0+`. Find out docker version by using below command.
```
$>> docker --version
```

## Docker compose for {{cookiecutter.ProjectName}} Service
**docker-compose** files can be found **localDevSetup**.
- Docker compose for {{cookiecutter.ProjectName}} service is structured with below services.
    - {{cookiecutter.ProjectName}} Service 
        - `.dockerignore` file is present at the solution level to ignore certain files and folders from containerization.   
    - Bank Service
    - Fraud Service
    - MsSQL Database
    - Logstash
    - Elasticsearch
    - Kibana
    - Vault
    - Spring Cloud Config Server
- Docker compose for code quality checks have below services.
    - SonarQube
- All the services are integrated on the same network bridge - {{cookiecutter.ProjectName}}network.
- Following volumes are used by respective services.
    - elasticsearch-vol
    - vault-vol


## Running Docker compose
Navigate to /localDevSetup` and execute below commands in terminal.

```
$>> docker-compose -f docker-compose.network.yml -f docker-compose.infra.yml up -d
```
### Set up the Configuration for Infrastructure

[Set up the ENV variables for Infrastructure which includes MSSQL and {{cookiecutter.ProjectName}} users for database access, Spring Cloud Config Server user account etc.](configuration.md#Setting-up-the-ENV-variables-for-{{cookiecutter.ProjectName}}-Service-Infrastructure-through-Docker-Compose)

 > To establish a connection to sql server database from {{cookiecutter.ProjectName}} service, we need to create a user at the database server. `localsetup/sqlserver` will create docker image with such default user. All default values already exists in env file.

**NOTE:** docker-compose is executed in detached mode when the command is executed with `-d` switch.  

### [Optional]  Setup the Code Quality Checks Infrastructure
```
$>> docker-compose -f docker-compose.network.yml -f docker-compose.codechecks.yml up -d  
```

###  Once infrastructure in ready, Set up the Configuration for {{cookiecutter.ProjectName}} Service Application

[Set up the ENV variables for Application which includes ASP.NET variables, enabling/disabling Hashicorp Vault, Spring Cloud Config Server integration etc.](configuration.md#Setting-up-the-ENV-variables-for-{{cookiecutter.ProjectName}}-Service-Application-through-Docker-Compose)

# Some useful Docker commands

- Build a Docker images
```
$>> docker build -t imagename:tag .
```

- Run a container
```
$>> docker run -p hostport:containerport --name containername -d imagename:tag
```

- View all containers
```
$>> docker ps -a
```

- View all images
```
$>> docker images -a
```

- Stop and Delete a container
```
$>> docker stop containername
$>> docker rm containername
```

- Delete an image
```
$>> docker rmi imagename
```

- Delete all containers which are not currently running
```
$>> docker rm $(docker ps -a -q)
```

- Delete all images which are untagged 
```
$>> docker rmi $(docker images -a --filter dangling=true)
```

- Follow the logs of a container  
```
$>> docker logs --follow containername
```

- Bash into a container 
```
$>> docker exec -it containerName /bin/bash
```

- Build and run from multiple Docker compose files
```
$>> docker-compose -f docker-compose.network.yml -f docker-compose.infra.yml -f docker-compose.app.yml build
$>> docker-compose -f docker-compose.network.yml -f docker-compose.infra.yml up -d
$>> docker-compose -f docker-compose.network.yml -f docker-compose.app.yml up -d  
```

- Build and run a particular server in docker compose
```
$>> docker-compose up -d --force-recreate --no-deps --build servicename
```

- Build and run a particular server in docker compose (multiple yml files)
```
$>> docker-compose -f docker-compose.network.yml -f docker-compose.app.yml up -d --force-recreate --no-deps --build {{cookiecutter.ProjectName}}service
```
- List all Docker volume
```
$>> docker volume ls
```
- Remove a Docker volume
```
$>> docker volume rm volume-name
```
- Remove all Docker volumes
```
$>> docker volume prune
```
- Find the container based on volume name
```
$>> docker ps -a --filter volume=VOLUME_NAME_OR_MOUNT_POINT
```


