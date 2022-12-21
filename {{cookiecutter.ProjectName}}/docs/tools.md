### Code Coverage
Code coverage is the percentage of code which is covered by automated tests. Code coverage measurement simply determines which statements in a body of code have been executed through a test run, and which statements have not. ... Code coverage is part of a feedback loop in the development process.

Follow the steps for test coverage and report generation
>Install report generator tool. Pls follow this link : https://www.nuget.org/packages/dotnet-reportgenerator-globaltool
```
 $>> cd tools/codeCoverage
 $>> ./run-tests-with-coverage-local.sh
```
This performs the code coverage and generates HTML report

>run-tests-with-coverage-ci.sh performs the code coverage on Github actions and has reference in [build.yml](../.github/workflows/build.yml)

### Setup Talisman.

Talisman is a tool that installs a hook to your repository to ensure that potential secrets or sensitive information do not leave the developer's workstation. It validates the outgoing changeset for things that look suspicious - such as potential SSH keys, authorization tokens, private keys etc.
To enable secret scanner locally we need to run tools/talisman/talisman-precommit.sh file.
It setups the talisman secret scanner in our local system.
This runs the scanner everytime before commiting the changes to git.
```
$>> sh tools/talisman/talisman-precommit.sh
```
#### Refer [talisman git repo](https://github.com/thoughtworks/talisman) for details


### Set Up Permission For .sh Files
Run the command to set correct permissions on all required `.sh` files in the project, so that we don't need to run chmod multiple times
```
$>> cd tools
$>> chmod 755 ./set-up-permissions-for-all-scripts.sh
$>> ./set-up-permissions-for-all-scripts.sh
```