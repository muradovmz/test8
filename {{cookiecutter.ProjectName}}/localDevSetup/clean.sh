#!/bin/bash

docker ps -q | xargs docker stop
docker ps -aqf status=exited | xargs docker rm -v
docker volume prune -f
docker images -aqf dangling=true | xargs docker rmi
