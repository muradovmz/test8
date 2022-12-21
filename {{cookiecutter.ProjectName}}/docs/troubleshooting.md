## Troubleshooting steps

* In-case there is some issue in containers creation,like containers showing unhealthy or getting exited, please ensure that sufficient memory and disk space is allocated to docker 
  * To check docker information use the command ``sudo docker info``
  * To increase the disk space, memory and CPU for colima, use the command ``colima start -c 2 -m 10``
  * For docker desktop on Mac, refer  [Disk utilization in Docker for Mac](https://docs.docker.com/desktop/mac/space/)


* If you want to pass few secrets during application run, we can use [launch settings](/src/{{cookiecutter.ProjectName}}.Api/Properties/launchSettings.json)
  * Add required secret or custom local set config, which can't be commit in `appsettings.json` file
    * ``"environmentVariables": {"Vault__RoleId": "Your Secret"}``
    * To ignore this class in your commit
      * Either add it `.gitignore` 
      * Or run ``git update-index --assume-unchanged src/{{cookiecutter.ProjectName}}.Api/Properties/launchSettings.json``