# ePay REST Server

## Overview

This project consists of two separate .NET Core projects:

1. **epay_rest_server**: Contains the REST server built using ASP.NET Core framework. This project includes a controller and service for managing customer data.

2. **epay_rest_server_simulator**: Contains a standalone simulator application that sends POST and GET requests to the server to simulate interactions.

## epay_rest_server

### Controller

The `CustomerController` is responsible for handling HTTP requests related to customer data. It includes two endpoints:

1. **GET /api/customers**: Retrieves a list of customers stored in the system.
2. **POST /api/customers**: Adds new customers to the system.

### Service

The `CustomerService` provides functionalities for managing customer data. It includes methods for loading, adding, and saving customer information. It ensures thread safety for data operations by using locks.

## epay_rest_server_simulator

The simulator is a standalone application that sends simulated HTTP POST and GET requests to the server. It generates random customer data and sends it to the server using POST requests. After posting the data, it sends a single GET request to retrieve the list of customers from the server.

## How to Run

1. Start the ePay REST Server ("epay_rest_server") by running the .NET project.
2. Ensure that the server is running on `https://localhost:7172`.
3. Run the simulator application ("epay_rest_server_simulator"). It will send multiple POST requests to add customer data and then a single GET request to retrieve the data from the server.

## Dependencies

- .NET Core Runtime
- ASP.NET Core framework

## Configuration

- The server is configured to run on port 7172 by default. You can modify the port in the server's configuration if needed.
- The simulator is configured to send requests to `https://localhost:7172/api/customers`. Ensure that the server is running on this URL.