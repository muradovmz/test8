 # Exception Handling Practices

Following are some of the best practices which needs to be followed to have a reliable exception handling mechanism.

- Bring consistency on how exceptions are handled and how they translate to responses sent back to service consumers. For this a best practice is to  have a common/global handler (or handlers for specific types like system, business etc.) to handle all the exceptions.
    - The global handler should be capable of handling **all** exceptions from **different** layers of the application like filters, middleware, controllers, actions etc.
- Avoid using `TRY...CATCH` in the code to the maximum extent.
    - If it is inevitable to avoid (for example, when we want to retry an operation) the try/catch block, make sure to bubble up the exception in imminent erroneous scenarios by throwing it from the catch block.
    - Always use `throw` instead of `throw ex;` to preserve exception context.
- Error response to the consumer can include detailed response only in cases where the errors are related to how the call is made by the consumer or what data is passed, since in such cases consumer can fix it. In case of errors related to the implementation of the service itself - e.g. database not accessible - do not send such details to consumer. Following information is preferred in error response.
    - Proper HTTP Status Code 
    - Error Code
    - Message (description of Error code)
    - Additional error reasons (potentially the business error message included while throwing the exception)
- Entire stacktrace should be logged to preserve the exception context for further analysis.
    - Centralized logging is recommended (refer to [logging best practices](logging.md) for more details).
    - Logs can consist of additional contextual information like request-id, correlation-id, dependencies information etc.
- [Handle errors in ASP.NET Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-3.1)

## Exception handling implementation details of {{cookiecutter.ProjectName}} Service

- Global Exception are handled through the Exception filter [{{cookiecutter.ProjectName}}ExceptionFilterAttribute](../src/{{cookiecutter.ProjectName}}.Framework/Filter/{{cookiecutter.ProjectName}}ExceptionFilterAttribute.cs)
  Here we map the exceptions with the error response 
- The filter is configured in the AddMvcServices extension method 
- Different exception types can be found in [Exceptions Folder](../src/{{cookiecutter.ProjectName}}.Framework/Shared/Exception/).

# Model Validation Practices

Following are some of the best practices which needs to be followed when handling model validation errors in the application.

- Most of the modern web frameworks like Spring Boot, ASP.NET Core etc, supports model validation out of the box, which means the input model will be validated by the frameworks automatically. 
    - The .Net framework has a very popular validation library called FluentValidation.It is used for building strongly-typed validation rules.
    - FluentValidation is a replacement for the existing validation attributes (Data Annotations). It can turn up the validation game to a new level, gives total control. It separates the validation rules and/or logic from the Model/DTO classes.
      It is a open-source library that helps you make validations clean, easy to create, and maintain. It also works on external models that you don’t have access. It makes the model classes clean and readable .
    - **IMPORTANT**: The default HTTP Response (which is sent on error scenarios) and error messages are not quite user friendly. We can overwrite them through centralized error handling code (mentioned in next points). 
- Have the model validation verification (pass/fail scenarios of validation) code centralized in a class.
    - In ASP.NET Core, we can use a `Filter` to check for `ModelState.IsValid`. Register the filter in the `Startup` class.
    - Create a custom exception `BadRequestException` (or use an existing one from framework) which can be raised on model validation failures.
    - This exception can be handled at global exception handler where it can be transformed to `400 BAD REQUEST` custom HTTP response (in some cases `422 Unprocessable Entity` and other HTTP Status codes based on the type of validation failure). 
- Model validation errors are different from JSON Input errors. All errors should be handled in the `Filter` and `BadRequestException` should be raised with proper error message based on the type of the failure.
    - JSON Input errors occur when JSON Serializer is not able to deserialize the input JSON to a strongly typed model. For example, we are passing a string value in a property which is mapped to an integer type on the server side model.
    - Model validation errors are typically business and data validation errors which result from checks for length, format, min/max values etc.
    - For JSON Input errors, a generic message like `Input field is not in correct format` can be returned to the consumer. 
    - For model validation errors, specific messages based on the types of validation errors should be returned to the consumer. For example, `Name is exceeding the allowed 30 characters.` message can be returned to the consumer, when the length requirements are not met for name property.
- Custom validation errors should raise custom exceptions and should be handled in global exception filter which will transform to respective HTTP responses.
    - For example, situations like {{cookiecutter.ProjectName}} cannot be processed because of less funds in account can be handled through `InsufficientFundsException`.


## Model Validation implementation details of {{cookiecutter.ProjectName}} Service

- Validator class can be created for objects to define custom rules
- FluentValidation is configured at MVC level in the extension method AddMvcServices to scan all validators in the assembly
  `.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<Startup>(); });`

- Custom business validation exception is defined as [BadRequestException](../src/{{cookiecutter.ProjectName}}.Framework/Shared/Exception/)
- Model Validation verification is done using [ModelValidationFilter](../src/{{cookiecutter.ProjectName}}.Framework/Filter/ModelValidationFilter.cs).
- Global Exception Handler can be found at [{{cookiecutter.ProjectName}}ExceptionFilterAttribute](../src/{{cookiecutter.ProjectName}}.Framework/Filter/{{cookiecutter.ProjectName}}ExceptionFilterAttribute.cs).

ASP.NET Core by default handles model validation and JSON Input errors, and sends `400 Bad Request` response to the consumer. The format of the response message is not user friendly. Hence we turned off the ASP.NET Core's default model validation error handling by setting `SuppressModelStateInvalidFilter` to `true` at `Startup` class. ASP.NET Core's default JSON Input formatter errors are disabled by setting `AllowInputFormatterExceptionMessages` to `false` in `Startup` class. 

> **NOTE:** In {{cookiecutter.ProjectName}} Service, both types of errors are handled through custom `Filter` and `Exceptions` as mentioned at the beginning of this section.

# Circuit Breaker Practices

Circuit breaker pattern is one of the key pattern which is widely used in achieving the overall resilience objective of a distributed system. The primary goal of the circuit breaker pattern is to open the faulty subsystem of the overall distributed system for sometime by not allowing the traffic. By this way the impact can be contained to that specific subsystem without affecting the health of the overall system. Once the fault is rectified and the health of the subsystem is restored, the circuit breaker will close the circuit and allows the regular traffic.

> Usually in distributed system, the most typical system outages occur due to the cascading impact of multiple subsystem failures. For example, consider a timeout exception at database layer which usually takes few seconds to notify the source system which initiated the connection. During these few seconds, multiple requests can pile up on the source system requesting the same database access and the overall load can bring down the database and web servers. This catastrophic affect can cascade and propagate to the remaining subsystems either because of infrastructure/network/business dependencies, eventually bringing down the entire system.

Following are few of the best practices to be considered while implementing a circuit breaker policy.

- Circuit breaker policies should be implemented while:
    - Interacting with External Dependencies
    - Having Infrastructure Dependencies like Databases, Cache etc.
    - High performance computing
- Circuit breaker policies should be wrapped with the below order of precedence.
    - Fallback policy (outermost policy)
    - Circuit breaker policy
    - Wait and Retry policy (innermost policy)
- Different circuit breaker instances should be created to handle different scenarios.
    - For example, the same circuit breaker instance should not be used to handle faults for HTTP and Database interaction.
- It is always recommended to configure circuit breaker policies in the HTTP pipeline like `Configure` method in `Startup` class.
- Where external HTTP calls are involved, it is always advised to handle circuit breaking by configuring `HttpClientFactory` instead of handling individual methods. Similar approach should be followed for other dependencies like database etc.
- Circuit breaking activity should be logged to gather analytics and alert the teams for issues.

## Circuit Breaker implementation details of {{cookiecutter.ProjectName}} Service

Circuit breaker pattern for {{cookiecutter.ProjectName}} Service is implemented using `Polly` package.
Web client is configured with default settings for circuit breaker, timeout and other settings
Call HttpRetryPolicySettings.Builder class to configure with desired values
```
<PackageReference Include="Polly" Version="7.2.2" />