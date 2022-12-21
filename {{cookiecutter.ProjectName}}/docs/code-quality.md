# Code Quality Checks

It is always necessary to adhere to the code quality standards, compliance and guidelines from the initial stage of the project. Poor code quality results in many disturbing outcomes like high defect injection rate and density, low defect fix rate, inconsistent design, bad performance, nonreusable code, hard maintenance, multiple regressions etc. The bad code smells will directly impact the confidence of team and the reputation of the organization. To prevent adverse actions and results, it is always recommended and advised to integrate code quality checks at different cycles of software development.

SonarQube is an open source centralized tool suite to measure and analyze the quality of the source code. Some of the advantages of SonarQube are as follows.
- It can be customizable based on project needs through rules.
- It provides good visualizer through which different issues can be analyzed in depth.
- Enforces best coding practices and identifies security issues.
- Identifies duplicate code, memory leaks and lack of test coverage.
- Determines complexities amd design inefficiencies.
- It supports multiple languages and polygot systems.
- It is open sourced and supports multiple metrics like code smells, debt, vulnerabilities etc. 
- It is a centralized system where multiple projects can be analyzed and can share common rulesets.
- Sonarlint extension can work with multiple IDEs.
- Integrates with different CI pipelines and different tools.

# Integration of SonarQube with {{cookiecutter.ProjectName}} Service
On the local developer machine, SonarQube is integrated with {{cookiecutter.ProjectName}} Service through `Docker Compose for Code Checks`, refer to  [Docker Compose Documentation](containerization.md). 

> SonarQube is also integrated through GitHub actions to perform code analysis on every commit.

1. Open browser and navigate to http://localhost:9000/sessions/new to login into SonarQube. 
2. Enter `admin` as username and `admin` as password.
3. Navigate to `Projects` tab. Click on `Create New Project` button.
4. Enter `{{cookiecutter.ProjectName}}service-sonarkey` for both `Project Key` and `Display Name`. Click Setup.
5. Enter `{{cookiecutter.ProjectName}}service-sonartoken` to `generate a token`. Click Generate.
6. Store the generated token.

Navigate to `{{cookiecutter.ProjectName}}/localDevSetup`. Set the following environment variables to analyze {{cookiecutter.ProjectName}} service source code.

```
$>> cd sonarqube
$>> export sonarqube_key='<! {{cookiecutter.ProjectName}} Service Sonar Project Key !>' 
$>> export sonarqube_token='<! {{cookiecutter.ProjectName}} Service Sonar Project Token !>' 
$>> export sonarqube_url='<! {{cookiecutter.ProjectName}} Service Sonar Url, for example http://localhost:9000/ !>' 
```

Execute the script `analyze.sh` as shown below. Make the script executable by running `chmod 755 ./analyze.sh` .

```
$>> ./analyze.sh
```

Navigate to the SonarQube portal and login. We should the analysis of {{cookiecutter.ProjectName}} service.

> Note: We are only doing analysis for {{cookiecutter.ProjectName}} API project and not for other projects.
