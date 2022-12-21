## Colima

We have tested using the [colima](https://github.com/abiosoft/colima) if you do not have docker desktop installed.

1. Install colima by following the instructions [here](https://github.com/abiosoft/colima/blob/main/README.md).
2. Make sure docker desktop is uninstalled to avoid conflicts.
3. Install docker cli and docker-compose cli.

```bash
$ brew install docker # install docker cli
$ brew install docker-compose # docker compose utility
```

4. Start colima by running the following command:

```
$ colima start
$ colima start -c 2 -m 10 #if you want to demo elk stack
```
