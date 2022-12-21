
# OpenAPI Specification

The **OpenAPI Specification** is a community-driven open specification within the [OpenAPI Initiative](https://www.openapis.org/), a Linux Foundation Collaborative Project.

The OpenAPI Specification (OAS) defines a standard, programming language-agnostic interface description for REST APIs, which allows both humans and computers to discover and understand the capabilities of a service without requiring access to source code, additional documentation, or inspection of network traffic. When properly defined via OpenAPI, a consumer can understand and interact with the remote service with a minimal amount of implementation logic. Similar to what interface descriptions have done for lower-level programming, the OpenAPI Specification removes guesswork in calling a service.

The documentation for **OAS 3.0.3** can be found [here](http://spec.openapis.org/oas/v3.0.3).

**OpenAPI Generator** allows generation of API client libraries (SDK generation), server stubs, documentation and configuration automatically given an OpenAPI Spec (both 2.0 and 3.0 are supported). OpenAPI Generator documentation can be found [here](https://github.com/OpenAPITools/openapi-generator).


# Recommendations and best practices while working with OpenAPI Specification
- It is recommended to have every API developed as part of the project adhering to the OAS standards and should generate OAS document (JSON or YAML) which brings the uniformity and ease of integration with different other systems and applications.
    - For legacy code, OAS can be exempted if the legacy code is based out of Non-RESTful APIs. If modernization is in the roadmap, OAS standards should be given first priority.

- Consumers of the API should leverage OpenAPI generator to auto generate API Client stubs through which integration should be accomplished.

- Where ever it is possible, it is recommended to follow `Spec-First` approach, where we first write the spec (in JSON or YAML files), and then generate the server stubs from the spec.
    - It is recommended to generate abstractions (or interfaces) instead of API stubs with default implementation.
    - The generated code should serve as base abstraction for the API definition. The project should override the abstractions with real implementations.

- While using OpenAPI generator, it is **not** recommended to change/tweak the code generated.

- It is always recommended to generate the code using OpenAPI Generator in Build pipelines and create Nuget packages (both server and client stubs can be generated).
    - These Nuget packages (holding Server stubs) should be leveraged by the consumers to overwrite the abstractions with their own implementations. 
    - The Nuget packages (holding client stubs) can be directly used to invoke Server API.
    - OpenAPI Generator creates documentation as part of code generation process. This documentation can be stored in the version control repo which serves as developer guide.
    - Generating code in build pipelines helps in preventing the overhead of versioning the generated code. It also prevents unwanted modifications and changes to the generated code.
    - The configuration changes which are required for the generated `csproj` file (like changing the defaulr `RepositoryUrl`) can be handled through custom scripts.

- Project team should be cautious with the generated code and should be thoroughly evaluate for standards, compliances and unnecessary functionality. We should first build the code with the choice of build tool to validate the codes integrity.
    - At times, the generated code doesn't support the version of language and framework of the project's choice.
    - The model classes which are generated from OpenAPI generator will have default functionality which is in no interest for the project. The names of the classes generated should be checked to make sure they are in alignment with the project.
    - Sometimes the generated code will not compile because of default packages versions. In these cases, code generation can be controller by passing `additional properties` to the OpenAPI generator. For example, ASP.NET Core additional properties can be found [here](https://github.com/OpenAPITools/openapi-generator/blob/master/docs/generators/aspnetcore.md).
    - There can be instances, where code generator can misinterpret the framework provided types with custom types. In these scenario, it will end up generate model classes for the framework types. This will cause ambiguous references issues and eventually code will not compile. To avoid this situation, make sure the spec is properly documented with type information.
    - The generated code can have additional artifacts included in the project. We should evaluate the relevance and impact of these additional files.

- In case of issues with the generated code, the project should aim for customizing the OpenAPI Generator based on their needs. Open API Generator can be customized by following these [instructions](https://github.com/OpenAPITools/openapi-generator/blob/master/docs/customization.md).

- It is recommended to follow TDD for the real implementations of the API.
    - The project team should check for the possibility of creating tests for generated code especially for models.

- Versioning of API should be given clear thought before documenting the spec or code generation.
    - For minor version upgrades, the same spec document can be updated with new details.
    - For major version upgrades, a new spec document should be created.
    - The strategy and approach of generating and maintaining code to support multiple API versions should be within the discretion of the project team.


# OpenAPI Spec for {{cookiecutter.ProjectName}} Service
For {{cookiecutter.ProjectName}} service, we followed OAS standards with following approach.

1. Created OpenAPI Spec JSON document with the API definitions.
    - [{{cookiecutter.ProjectName}} Service Spec ({{cookiecutter.ProjectName}}-service-api repo)](https://github.com/cd-jump-start/{{cookiecutter.ProjectName}}-service-api/blob/master/specs/{{cookiecutter.ProjectName}}service.spec.json)
    - [Bank Service Spec (bank-service-api repo)](https://github.com/cd-jump-start/bank-service-api/blob/master/specs/bankservice.spec.json)
    - [Fraud Service Spec (fraud-service-api repo)](https://github.com/cd-jump-start/fraud-service-api/blob/master/specs/fraudservice.spec.json)
2. {{cookiecutter.ProjectName}} Service server stub (only abstractions using ASP.NET Core 3.0) is generated using {{cookiecutter.ProjectName}} Service Spec.
    - OpenAPI Generator is used at Github actions to generate code. 
    - Then the generated code is build and packaged as Nuget package, which is finally uploaded to Github Packages.
    - {{cookiecutter.ProjectName}} Service project uses this Nuget and override the base abstractions with concrete implementations.
3. The Bank and Fraud service client stubs are generated using Bank and Fraud Service specs.
    - OpenAPI Generator is used at Github actions to generate code.
    - Then the generated code is build and packaged as Nuget packages respectively, which are finally uploaded to Github Packages.
    - {{cookiecutter.ProjectName}} Service project leverages these Client Nuget packages to interact with Bank and Fraud services.
4. Integrated Swagger with {{cookiecutter.ProjectName}} Service which will generate and serve OpenAPI Spec.


# [Optional] Generate code locally using OpenAPI Generator

To better understand the OpenAPI code generation, we should generate code in our local and explore it. Install openapi-generator as shown below.
```
brew install openapi-generator
```

{{cookiecutter.ProjectName}} Service server stub is generated using OpenAPI Generator as shown below. Replace the path to {{cookiecutter.ProjectName}} service spec with the local path.

```
openapi-generator generate -i <! Path to {{cookiecutter.ProjectName}}service.spec.json !> -g aspnetcore --additional-properties=aspnetCoreVersion=3.0,isLibrary=true,packageName={{cookiecutter.ProjectName}}Service.Server,useSwashbuckle=false,operationResultTask=true,generateBody=false,classModifier=abstract
```

The generated `BankInfoDetailsApiController` is shown below. Model classes can be found in `Models` folder.

```
#region Assembly {{cookiecutter.ProjectName}}Service.Server, Version=1.0.6.0, Culture=neutral, PublicKeyToken=null
// {{cookiecutter.ProjectName}}Service.Server.dll
#endregion

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using {{cookiecutter.ProjectName}}Service.Server.Attributes;
using {{cookiecutter.ProjectName}}Service.Server.Models;

namespace {{cookiecutter.ProjectName}}Service.Server.Controllers
{
    [ApiController]
    public abstract class BankInfoDetailsApiController : ControllerBase
    {
        protected BankInfoDetailsApiController();

        //
        // Summary:
        //     Add a bankinfo
        //
        // Parameters:
        //   bankInfoRequest:
        //
        // Remarks:
        //     Adds the new bank and its url
        [HttpPost]
        [ProducesResponseType(typeof({{cookiecutter.ProjectName}}FailureResponse), 500)]
        [ProducesResponseType(typeof(BankInfoRequest), 201)]
        [Route("/bankinfo")]
        [ValidateModelState]
        public abstract Task<IActionResult> Create([FromBody] BankInfoRequest bankInfoRequest);
    }
}

```

The concrete implementation of the `BankInfoDetailsApiController` in {{cookiecutter.ProjectName}} Service can be found at [BankInfoController.cs](../{{cookiecutter.ProjectName}}Service/Controllers/BankInfoController.cs).


Bank Service Client stub (similarly we can generate for Fraud Service as well) generated using OpenAPI Generator as shown below. Replace the path to {{cookiecutter.ProjectName}} service spec with the local path.

```
openapi-generator generate -i <! Path to bankservice.spec.json !> -g csharp-netcore --additional-properties=netCoreProjectFile=true,targetFramework=netcoreapp3.1,packageName=BankService.Client
```

The Client API is integrated with {{cookiecutter.ProjectName}} Service at [BankClient.cs](../{{cookiecutter.ProjectName}}Service/ServiceClients/BankClient.cs).
