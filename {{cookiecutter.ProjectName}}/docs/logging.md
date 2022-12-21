# Logging Best Practices


### Logging Providers in ASP.NET Core
    1. Console
    2. Debug
    3. EventSource
    4. EventLog
    5. Trace
    6. Event

### Log Levels in ASP.NET Core - The log level indicates the severity or importance.
    1. Trace
    2. Debug
    3. Info
    4. Warning
    5. Error
    6. Critical
    7. None

## Text Logging
Text logging is also called ‘printf debugging’ after the C printf() family of functions.
The problem with text logging files is they are unstructured text data. This makes it hard to query them for any sort of useful information.

Hence, as a best practice always use structured logging.


## What is Structured Logging

Structured logging can be thought of as a stream of key-value pairs for every event logged, instead of just the plain text line of conventional logging.

### Using Structured Logging

1. #### Adding the provider

    To add a provider in an app that uses Generic Host, call the AddApiLogger extension method in Program.cs:

```
     
                .AddApiLogger()
```

2. #### Logging Events
    Logging events for an API call should  produce the trace shown below :
    ```
    info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/1.1 GET http://localhost:5000/api/todo/0
    info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
      Request finished in 84.26180000000001ms 307
    info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/2 GET https://localhost:5001/api/todo/0
    info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
      Executing endpoint 'TodoApiSample.Controllers.TodoController.GetById (TodoApiSample)'
    info: Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker[3]
      Route matched with {action = "GetById", controller = "Todo", page = ""}. Executing controller action with signature Microsoft.AspNetCore.Mvc.IActionResult GetById(System.String) on controller TodoApiSample.Controllers.TodoController (TodoApiSample).
    info: TodoApiSample.Controllers.TodoController[1002]
      Getting item 0
    warn: TodoApiSample.Controllers.TodoController[4000]
      GetById(0) NOT FOUND
    info: Microsoft.AspNetCore.Mvc.StatusCodeResult[1]
      Executing HttpStatusCodeResult, setting HTTP status code 404
    ```


3. #### Logging contexts and correlation Ids


    Logging context allows you to define a scope, so you can trace and correlate a set of events, even across the boundaries of the applications involved.

    Correlation Ids are a mean to establish a link between two or more contexts or applications, but can get difficult to trace. At some point it might be better to handle contexts that cover business concepts or entities, such as an {{cookiecutter.ProjectName}}Context that can be easily identified across different applications, even when using different technologies.

    Here are the minimum context properties that is recommended in addition to the event properties

    These are some of the context properties :

        *ApplicationContext* or *CorrelationId* Is defined on application startup and adds the ApplicationContext or CorrelationId property to all events.

        *SourceContext* Identifies the full name of the class where the event is logged, it's usually defined when creating or injecting the logger.

        *RequestId* Is a typical context that covers all events while serving a request. It's defined by the ASP.NET Core request pipeline.

        *EventId* and event parameters. EventsIds can be defined in a LoggingEvents class


3. #### Setup and Configuration

    Logging provider configuration is provided by one or more configuration providers:

    File formats (INI, JSON, and XML).
    Command-line arguments.
    Environment variables.
    In-memory .NET objects.
    The unencrypted Secret Manager storage.
    An encrypted user store, such as Azure Key Vault.
    Custom providers (installed or created).

    The following example shows the logs configuration in  localDevSetup/configserver/config/{{cookiecutter.ProjectName}}service-development.yml
    ```
    Serilog:
        MinimumLevel:
        Default: Debug
        Override:
        Microsoft: Warning
        System: Warning

    ```

# Recommendations and best practices while choosing centralized logging options
- Logging framework should be capable of supporting different protocols and log formats.
- Aggregation of log data from different sources and different formats should be supported by the logging platform.
- Leverage different Log sink frameworks like Serilog etc., to integrate application with different log providers. 
- Choosing data indices (for example, Elasticsearch) is very important for achieving  high throughput performance.
- Always analyze the time taken to write a log and if possible try to optimize it.
- Make sure to have clear identification of what information needs to be logged (have clear differentiation between metrics and logs).
    - Application should focus on business and technology stack specific logs. 
    - System and server logs should be aggregated and should be kept out of context of the application logging scope.
- Make sure to keep the log size minimal by utilizing compact JSON formatters.
- Do not include any data related to PII in logs.
- It is always recommended to decouple the logging platform with the application code.
    - For example, application can write logs to a log file. A tool like Filebeats can pick up the logs periodically and send them to ELK stack for further analysis. This approach should be preferred compared with HTTP and TCP based logging.
    - Sidecar containers can also be considered to offload logging capabilities in container-based environment like kubernetes.
- The logs for the same request should be correlated using properties like Correlation-ID, Request-ID etc.
- Be cautious when logging objects for their size and sensitivity of information. It is always recommended to log only the required information instead of logging entire object.
- Always categorize logs based on the levels like Warning, Information, Error, Debug etc.
- Make sure to opt for a logging platform which have default high availability and disaster recovery support.
- Tools like Prometheus should be used with Time series data through which different metrics should be analyzed.
- The logging framework should support different security practices.
- It is always recommended to go for managed logging service provider (probably from different cloud vendors) instead of managing the logging infrastructure by ourselves because sometimes it becomes overhead to manage and maintain complex infrastructure with right security and configuration policies.

# Structured Logging with Azure Application Insights

Create an instance of Azure Application Insights using the Azure Portal. Get the `Instrumentation Key` from the Application Insights overview.

> The main advantage of Azure Application Insights is its capability to capture application telemetry along with logs. In short, it provides similar functionality of an ELK stack along with Prometheus and Grafana metrics.

Create `ApplicationInsights` section in **localDevSetup/configserver/config/{{cookiecutter.ProjectName}}service-development.yml**  as shown below.
```
"ApplicationInsights": {
    "InstrumentationKey": "Instrumentation key..."
},
```

Add `Microsoft.ApplicationInsights.AspNetCore` Nuget package to the **{{cookiecutter.ProjectName}}.Api.csproj** as shown below.
```
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.13.1" />
```
- To configure Application insights, goto `ApiConfigurationExtensions --> AddApiLogger` and change the method to accomodate AppInsight.

# Centralized Logging with ELK (Elasticsearch, Logstash and Kibana)
ELK Stack is primarily used to provide streamlined data analytics and insights from different log and metric sources. It also helps in diagnosing various system issues through different metrics and logs. The rich querying and analytical capabilities of ELK not only help identifying typical application issues, but also provide valuable insights on health of the system and its usage.

**Elasticsearch:** A powerful open-source search and analytics engine used for full-text search and for analyzing logs and metrics.

**Logstash:** An open-source tool that ingests and transforms logs and events.

**Kibana:** An open-source visualization and exploration tool for reviewing logs and events.

ELK Stack is technology and platform agnostic and it is open source. We can leverage both cloud and on-prem infrastructure to configure and scale the ELK stack. It is a very good option for systems where logs are unstructured, inconsistent, and inaccessible.

## ELK Stack integration with {{cookiecutter.ProjectName}} Service through Serilog framework
- The ELK infrastructure is provisioned through `Docker Compose`, refer to  [Docker Compose Documentation](containerization.md). 
- ELK Stack is configured using `Spring Cloud Config Server` as mentioned in [Configuration documentation](configuration.md).
- Serilog is the default logger
  - For configuration, [refer here](../src/{{cookiecutter.ProjectName}}.Api/Extension/ApiConfigurationExtensions.cs)
  - The configuration enriches the log information to have information of `LogContext, MachineName, EnvironmentName, ExceptionDetails`.
  - The log outputs are configured to following streams.
     - Console - which will write logs to console window.
     - TCP Sink - which is configured to Logstash (and eventually make it to Elasticsearch and Kibana analytics).
     - HTTP SinK - an alternative to TCP Sink which sends logs to Logstash.
     - `RenderedCompactJsonFormatter` is used to make sure the log output formatter is similar in all log outputs.
  - To use HTTP Sink through logstash, we need additional configuration.
    - we need to install `logstash-input-http` plugin on the logstash container. Uncomment the plugin installation step in the [Logstash dockerfile](../localDevSetup/logstash/Dockerfile) and recreate the containers.
    - We need to configure the `input` configuration at logstash to `http` from `tcp` in [Logstash configuration file](../localDevSetup/logstash/pipeline/logstash.conf).
  
>To view the logs on Kibana, create index in the following format *{{cookiecutter.ProjectName}}s-%{+xxxx.ww}*
