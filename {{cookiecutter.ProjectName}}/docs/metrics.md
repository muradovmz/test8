# Capturing Application Telemetry

Telemetry is the automatic recording and transmission of information to remotely track the health and usage of an system (can be a server, software application, database etc.). The captured information can be analyzed and monitored to make real-time decisions about the availability and behavior of a system. Alerts can be configured to notify different stakeholders with the up-to-date state of the system. 

Telemetry helps in troubleshooting and identifying the issues in the distributed systems where one application (or server) pitfall can potential cause the entire system outage. Telemetry also helps in identifying the usage patterns of the application like pages (or parts of the application) where users are spending most of the time, which ads are being clicked most frequently, during what time of the day does the system gets most of the traffic etc.

Some of the best practices for capturing telemetry as listed below.

1. Always opt for pull based telemetry capturing tools like Prometheus. Push based mechanism will cause additional overhead on the system which is being monitored.
2. Monitoring tools should support multiple and different systems like database, servers, application etc.
3. Make sure to capture below metrics.
    - Application Performance counters
    - Business Domain related metrics
    - Health and System metrics
4. Create necessary alerts for the mission critical metrics.
    - Make sure to opt for a monitoring tool which got compatibility with different notification systems like email, slack etc.
5. Common best practices for capturing and analyzing telemetry can be found at https://docs.microsoft.com/en-us/azure/architecture/best-practices/monitoring.

**NOTE:** Prometheus is not a tool to give the point in time details of exceptions or requests or some other metrics. The data gets extrapolates as mentioned in this [link](https://labs.consol.de/monitoring/2016/08/13/counting-errors-with-prometheus.html).


# Configuring Prometheus and Grafana Docker Containers
On the local developer machine, Prometheus and Grafana are integrated with {{cookiecutter.ProjectName}} Service through `Docker Compose for Application Metrics`, refer to  [Docker Compose Documentation](containerization.md). 

Prometheus and Grafana are configured as described below:

1. Prometheus configuration file can be found at `/localDevSetup/prometheus/prometheus.yml`. This file is mounted to prometheus container at Docker compose file.
    - The configuration consists of that {{cookiecutter.ProjectName}} service target ({{cookiecutter.ProjectName}}service:8080) which is used to scrape the metrics.
    - It also contains the scrape interval (defaulted to 15 secs).
2. Grafana is configured with Prometheus using `/localDevSetup/grafana/provisioning/datasources/datasource.yml`. This file is mounted to prometheus container at Docker compose file.
    - The configuration consists of Prometheus as a data source with basic configuration like URL, IsDefault, Editable, BasicAuth (disabled).
3. Grafana dashboard is configured using `/localDevSetup/grafana/provisioning/dashboards/dashboard.yml`. The dashboards folder is mounted to the Docker container.
    - The configuration consists of basic dashboard configuration like the Dashboard JSON file path, editable, disableDeletion etc.
    - Different Grafana dashboards can be configured using different JSON files. For {{cookiecutter.ProjectName}} Service, we created a Dashboard JSON File and placed it at `/localDevSetup/grafana/provisioning/dashboards/dashboard.json`. [Grafana 10427 dashboard](https://grafana.com/grafana/dashboards/10427) is taken as the base JSON and few more panels (mentioned below) are added on top of it.

> Grafana dashboard (dashboard.json) is pre-configured with following panels.
> - {{cookiecutter.ProjectName}} API calls served.
> - Request Duration within which 95% of {{cookiecutter.ProjectName}} Requests are served.
> - Bank API calls served.
>
> Grafana dashboards use PromQL to query Prometheus metrics. For more details on how we came up with queries for above panels, refer this [resource](https://prometheus.io/docs/practices/histograms/).


# Integration of {{cookiecutter.ProjectName}} Service with Prometheus

- Add Prometheus ASP.NET Core Nuget package `prometheus-net.AspNetCore`
- Refer `AddApiMetrics` method [here](../src/{{cookiecutter.ProjectName}}.Api/Extension/ApplicationBuilderExtensions.cs)
- With above setup in place, run the {{cookiecutter.ProjectName}} service, we can navigate to http://localhost:5000/metrics to see the default HTTP Request/Response metrics.

# Exploring and Configuring Grafana Dashboards
Go to http://localhost:3000/login, login with below details.
```
Username: admin
Password: admin
```

Grafana will prompt to change the password, do it accordingly. On home page, we should see `{{cookiecutter.ProjectName}}service` dashboard, click on it and explore the panels including custom panels (as mentioned in previous section).

We can also add new panels and configure to project different metrics. Once new panels are added (or existing panels updated), save the dashboard using the `save` option (at the top right corner). Copy the JSON to the clipboard and update the `/localDevSetup/grafana/provisioning/dashboards/dashboard.json`, so that the changes will get reflected whenever we provision a new Docker container.
