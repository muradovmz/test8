# Continuous Integration
Continuous integration (CI) helps developers merge their code changes back to a shared branch more frequently.
Once a developer’s changes to an application are merged, those changes are validated by automatically building workflows. 
These workflows build and test everything from classes and function to the different modules that comprise the entire app. 
If automated testing discovers a conflict between new and existing code, CI makes it easier to fix those bugs quickly and often.

# Github Actions
GitHub Actions is designed to help simplify workflows with flexible automation and offer easy-to-use CI/CD capabilities built by developers for developers.
Compared with other automation or CI/CD tools, GitHub Actions offers native capabilities right in your GitHub flow. 
It also makes it easy to leverage any of the 10,000+ pre-written and tested automations and CI/CD actions in the GitHub Marketplace 
as well as the ability to write your own with easy-to-use YAML files.

# CI using Git actions in {{cookiecutter.ProjectName}} service
Click the "Actions" tab on your repo. Under the workflows you will find all workflow yml files.
In the editor you can find workflow files at the given path:
[Build Workflow](../.github/workflows/build.yml)
[Release Workflow](../.github/workflows/release.yml)

## Build Workflow

* Executes the workflow on any push operation to the master branch
* Set the environment variable,like the solution file path, and git tokens
* Create a job to be executed with name "build-and-test:"
* Define the OS on which the workflow will run. Here we are using "ubuntu-latest". We can run the job in mac and windows OS as well.
* Define the different steps to be performed in each job

#### Steps in build-and-test: job
 * Checkout github action
 * Set up the dotnet environment with version dotnet 6.0.x
 * Install all the project dependencies
 * Restore and build the solution
 * Run the code coverage test with "run-tests-with-coverage-ci.sh" script
 * Generate the test coverage report using "danielpalme/ReportGenerator-GitHub-Action@5.0.0"
 * Archive the generated test coverage report at ./tmp/coverage/reports
 * Run Trufflehog Scan to check for secrets, private keys and credentials in the repository. 
 * Run Dependency Check  to detect publicly disclosed vulnerabilities contained within a project’s dependencies and generate HTMl report.
 * Archive the dependency check report at the specified path

## Release Workflow
Once the repo is stable, we would want to create a release on the GitHub repository. For this, we use the release-action@v1 action.
* Execute the workflow on any push operation with the "release" tag.
* Sets the environment and jobs, same as for build.yml
* Release source code creates a GitHub release and upload zip artifact to it.
* Steps to execute release workflow 
 ```
  $>>   git tag 'release-*.*.*'
  $>>   git push origin --tags    
 ```
>release-*.*.* this will be the tag u want to create, so any tag start with release- will trigger the build
