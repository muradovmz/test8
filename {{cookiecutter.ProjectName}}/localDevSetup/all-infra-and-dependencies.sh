#!/bin/bash

if [[ $1  == "up" ]]
then
  docker-compose  -f docker-compose.network.yml -f docker-compose.infra.yml  up -d
elif [[ $1  == "down" ]]
then
  docker-compose  -f docker-compose.network.yml -f docker-compose.infra.yml   down
else
    echo "Invalid option $1"
fi