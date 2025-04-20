# AggregatorService

Aggregator Service is a small examplary Api developed in .NET 8 using Clean Architecture and MediatR.

## Solution Structure

### ```AggregatorService.Api```
The Api project is the entry point of the solution and has only 2 controller (Auth, Aggration stuff). In ```Program.cs``` all depencies get injected, like Db context, background services, and http services be used later on.

### ```AggregatorService.Application```
The Application project mostly contains the command/query models and their respective handlers using MediatR, as well as the interfaces of the services that are going to be used later on in Infrastructure or in Unit tests.

### ```AggregatorService.Domain```
The Domain project contains all the entities that describe the domain this Api is all about.

### ```AggregatorService.Infrastructure```
The Infrastructure project contains all the implementations for Background services, Persistant storage services and Http services that are used by the handlers inside the Application project.

---

These 4 are the main projects for our solution. You can find them inside the ```src``` folder in the solution. On the other hand there is a ```test``` folder as well that contains all the unit tests for solutions implemented above.
### ```AggregatorService.Infrasture.Tests```
This Tests projects, developed with XUnit, contains all the tests for the services found in the Infrastructure project.

## How to Run
- Clone the project into your computer
- Make sure you have .NET 8 installed.
- In the appsettings.json file you will see the ```ApiKeys``` and ```JwtSettings``` are empty. Please use ```User-Secrets``` for the values. The structure is exactly the same but with proper values.
- You're ready to go.

## Authentication
Aggregation Service utilizes JWT tokens. If you run the Api project you will be presented with the swagger page. All endpoints except ```/Auth``` are protected. You can go and try to get a JWT with the following credentials:

```json
{
  "username": "johndoe",
  "password": "77java&&"
}
```

Should you get your JWT, you can add it in the upper right corner of the Swagger page, by clicking ```Authorize 🔒```. After that you are authorized to use the ```/api/Aggregator``` and ```/api/Aggregator/Stats``` endpoints.

Now you can call ```api/Aggregator?NewsQuery=Greece&WeatherCity=Athens&GithubUser=konkri```

## Future fixes
In case ther was extra time, I would probably use it to:
- Migration congiration from appsttings.json to Environmnet Variables for Docker Support as well.
- Add caching configuration in app settings.
- Add FluentValidation with MediatR pipelines to validate the input request from the user.
- Add Polly directly to HttpClient using Depedency Injection.
