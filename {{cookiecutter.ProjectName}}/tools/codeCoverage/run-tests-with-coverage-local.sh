#!/bin/bash -e

# Need to install report generator tool. Pls follow this link : https://www.nuget.org/packages/dotnet-reportgenerator-globaltool

dotnet test --results-directory $(pwd)/../../tmp/unit-test-results --logger "trx;LogFileName=unit_test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,opencover,json\" /p:CoverletOutput=$(pwd)/../../tmp/unit-test-results/coverage/ /p:Exclude="[xunit.*]*%2c[StackExchange.*]*" ../../src/{{cookiecutter.ProjectName}}.UnitTests

dotnet test --results-directory $(pwd)/../../tmp/framework-unit-test-results /p:MergeWith=$(pwd)/../../tmp/unit-test-results/coverage/coverage.json --logger "trx;LogFileName=framework_unit_test_results.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,opencover,json\" /p:CoverletOutput=$(pwd)/../../tmp/framework-unit-test-results/coverage/ /p:Exclude="[xunit.*]*%2c[StackExchange.*]*" ../../src/{{cookiecutter.ProjectName}}.Framework.UnitTests

reportgenerator "-reports:$(pwd)/../../tmp/framework-unit-test-results/coverage/coverage.opencover.xml" "-targetdir:$(pwd)/../../tmp/coverage/reports" "-reporttypes:HtmlInline;Cobertura;Badges;Latex;" -verbosity:Info

dotnet test ../../src/{{cookiecutter.ProjectName}}.ComponentTests
open $(pwd)/../../tmp/coverage/reports/index.htm
