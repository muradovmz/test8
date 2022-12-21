#!/bin/bash -e

dotnet test --results-directory $(pwd)/../../tmp/unit-test-results --logger "trx;LogFileName=unit_test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,opencover,json\" /p:CoverletOutput=$(pwd)/../../tmp/unit-test-results/coverage/ /p:Exclude="[xunit.*]*%2c[StackExchange.*]*" ../../src/{{cookiecutter.ProjectName}}.UnitTests --no-restore

dotnet test --results-directory $(pwd)/../../tmp/framework-unit-test-results /p:MergeWith=$(pwd)/../../tmp/unit-test-results/coverage/coverage.json --logger "trx;LogFileName=framework_unit_test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,opencover,json\" /p:CoverletOutput=$(pwd)/../../tmp/framework-unit-test-results/coverage/ /p:Exclude="[xunit.*]*%2c[StackExchange.*]*" ../../src/{{cookiecutter.ProjectName}}.Framework.UnitTests --no-restore

dotnet test ../../src/{{cookiecutter.ProjectName}}.ComponentTests --no-restore