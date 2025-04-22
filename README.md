# Project C# Azure Functions V4
#### Author: Daniel Juarez

## Project Overview
This project demonstrates a scalable `Azure Functions V4 worker (.NET 9.0 Isolated)` implementation, utilizing the **Http trigger** template to **POST** tickets to the [RESTful Web API](https://github.com/danljuarez/cSharp-RestAPI-NetCore-TicketList). Developed in `Visual Studio 2022 Community Edition (v17.12.3)`, this project shows key features of Azure Functions, including secure `authorization level` to `Function` and integration with the external APIs.

Key Features:
- Implements Azure Functions V4 following clean architecture principles.
- Utilizes Dependency Injection using Microsoft.Extensions.DependencyInjection.
- Employs Logging using Microsoft.Extensions.Logging.
- Handles errors using exceptions types.
- Includes OpenAPI extension (Swagger documentation) using Microsoft.Azure.WebJobs.Extensions.OpenApi.
- Supports Unit testing using MS Unit Test and Moq.

## Running the Project Locally

> **Note**: Project [Web API RESTfulNetCoreWebAPI-TicketList](https://github.com/danljuarez/cSharp-RestAPI-NetCore-TicketList) should be running locally before you run this Azure Function. To do so, follow [these instructions](https://github.com/danljuarez/cSharp-RestAPI-NetCore-TicketList/blob/master/docs/running-locally.md).

### Step 1: Run the Function API Locally
1. Press `F5` to start the project. When Functions runtime starts locally, a set of OpenAPI and Swagger endpoints are shown in the output, along with the function endpoint.

2. Open the **RenderSwaggerUI** endpoint, which should look like: `http://localhost:7086/api/swagger/ui`. A page will be rendered, based on the OpenAPI definitions.

3. Select **POST** > **Try it out**, enter in the JSON request body values indicated (red square), and select **Execute**.
![](./screenshots/screenshot-01.jpg)


### Step 2: Verify the Response
1. You'll get a response that looks like the following example:
![](./screenshots/screenshot-02.jpg)

2. You can verify your tickets have been successfully added by looking into the running `Web API RESTfulNetCoreWebAPI-TicketList` Swagger UI and selecting **GET** /api/Tickets/getAll > and then **Execute**.

## To Publish this Project to Azure
Now that you have verified the Azure function is running:

- [Publish the Azure Functions project and API definitions to Azure](https://learn.microsoft.com/en-us/azure/azure-functions/openapi-apim-integrate-visual-studio?tabs=isolated-process#publish-the-project-to-azure) and use the name `AddTicket` instead of 'TurbineRepair' as named in this linked document steps.

- [Import an Azure Function App as an API in Azure API Management](https://learn.microsoft.com/en-us/azure/api-management/import-function-app-as-api).

## Additional resources
- [Azure Functions documentation](https://learn.microsoft.com/en-us/azure/azure-functions/).
- [Serverless REST APIs using Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-proxies?source=recommendations).

<br/>
<br/>
Thank You.