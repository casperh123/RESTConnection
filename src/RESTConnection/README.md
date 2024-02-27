ESTConnectionToolkit
RESTConnectionToolkit is a .NET library designed to facilitate the integration with Amazon Selling Partner API and e-conomic REST APIs, providing a streamlined approach to constructing requests, handling authentication, and managing region-specific configurations. It's built with the aim of simplifying the development process for .NET applications requiring robust interaction with these APIs.

Features
Request Builders: Leverage AmazonRequestBuilder and EconomicRequestBuilder for easy construction and execution of API requests.
Region-Specific Configuration: Automatically configures API endpoints based on the selected region, supporting North America, Europe, the Far East, and Sandbox environments.
Authentication Management: Simplifies the generation and application of authentication headers, supporting both simple and OAuth-based authentication mechanisms.
Extensibility: Designed to be easily extendable for additional APIs or customized to fit specific project requirements.
Getting Started
Prerequisites
.NET 6.0 or later
An active account with Amazon Selling Partner or e-conomic, depending on which APIs you intend to interact with.
Installation
To install RESTConnectionToolkit, use the following NuGet command:

sh
Copy code
dotnet add package RESTConnectionToolkit
Or, add the package through the NuGet Package Manager in Visual Studio.

Usage
Below is a simple example demonstrating how to use the AmazonRequestBuilder to construct a request to the Amazon Selling Partner API.

csharp
Copy code
var authentication = new AmazonAuthentication("YourClientId", "YourClientSecret");
var builder = new AmazonRequestBuilder(authentication, Region.NorthAmerica);

var request = builder.BuildRequest(HttpMethod.Get, "/orders/v0/orders");
// Proceed with executing the request using HttpClient or any HTTP client of your choice.
Replace "YourClientId" and "YourClientSecret" with your actual credentials.

Contributing
Contributions are welcome! If you have improvements or bug fixes:

Fork the repository.
Create a new branch for your changes.
Submit a pull request detailing the changes made.
License
RESTConnectionToolkit is licensed under the MIT License. See the LICENSE file for more details.

Support
If you encounter any issues or have questions, please file an issue on the GitHub repository.