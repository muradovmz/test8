## Continuous Integration and Continuous Deployment

Continuous integration (CI) is a software practice that requires frequently committing code to a  repository.

When you commit code to your repository, you can continuously build and test the code to make sure that the commit doesn't introduce errors. Your tests can include 
* code linters (which check style formatting)
* security checks
* unit tests and code coverage
* functional tests


## GitHub Actions as a CI/CD tool

With GitHub Actions you can build end-to-end continuous integration (CI) and continuous deployment (CD) capabilities directly in your repository

You can configure your CI workflow to run when a GitHub event occurs (for example, when new code is pushed to your repository), on a set schedule, or when an external event occurs using the repository dispatch webhook.

GitHub runs your CI tests and provides the results of each test in the pull request, so you can see whether the change in your branch introduces an error. When all CI tests in a workflow pass, the changes you pushed are ready to be reviewed by a team member or merged. When a test fails, one of your changes may have caused the failure.


With Github Actions, this whole process can be automated by adding a single file to your repository. Each Action consists of a workflow definition file that is placed in the .github/workflows/ folder. The workflow is composed of a number of actions that either run a script directly or execute a Docker container.


A typical CI workflow for dotnet projects would include

1. Workflow Triggers
    * Trigger the workflow on push or pull request
    ```
    on:
        push:
            branches: [ master ]
        pull_request:
            branches: [ master ]

    ```
2. WorkFlow Jobs - Build Project Job 
    This job is primarily responsible for
    * Checkout the repository
        ```
        - uses: actions/checkout@v2
        ```
    * Sets up a dotnet  core cli environment for use
        ```
        - uses: actions/setup-dotnet@v1
        with:
            dotnet-version: 3.1.101
        ```
     * Run dotnet restore command
        ```
          - name: Install dependencies
            run: dotnet restore ./{{cookiecutter.ProjectName}}/{{cookiecutter.ProjectName}}Service.sln
        ```
    * Run dotnet build command
        ```
        run: dotnet build $./{{cookiecutter.ProjectName}}/{{cookiecutter.ProjectName}}Service.sln  --configuration Release --no-restore
        ```
     * Run SonarQube analysis action
        ```
         - name: SonarCloud Scan
        uses: sonarsource/sonarcloud-github-action@master
        env:
            GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
            SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
        with:
            projectBaseDir: ./{{cookiecutter.ProjectName}}
        ```
    * Run Snyk Scan for security Vulnerabilities
   
        ```
        - name: Run Snyk to check for vulnerabilities
      uses: snyk/actions/dotnet@master
      env:
        SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
      with:
        command: test
        args: --file=${{ env.SOLUTION_FILE }}  --severity-threshold=high --detection-level=2 
        ```
    * Run dotnet publish command to create the binaries
        ```
        
        - name: Publish
        run: dotnet publish ${{ env.PROJECT_FILE }} -c Release -o ./app/publish
        ```

3. WorkFlow Jobs - Push Images Job
This job will download the binary and create a docker image using the docker.release file and upload the image to the github package repository

    * Build the docker image
        ```
        docker build  --file ./{{cookiecutter.ProjectName}}/localDevSetup/pamentservice/dockerfile.release  -t image .
        ```
    * Log on the github package repository using GITHUB_TOKEN secret confifured in the repository settings
        ```
         - name: Log into registry
        run: echo "${{ secrets.GITHUB_TOKEN }}" | docker login docker.pkg.github.com -u ${{ github.actor }} --password-stdin
        ```
    * Tag and Push docker image
        ```
         docker tag image $IMAGE_ID:$VERSION
         docker push $IMAGE_ID:$VERSION
        ```








### References
1. [A curated list of a GitHub Actions.](https://github.com/sdras/awesome-actions)

2. [Docker images for ASP.NET Core](https://docs.microsoft.com/en-gb/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1)