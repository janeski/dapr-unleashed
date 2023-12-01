# Dapr Unleashed: Accelerating Microservice Development in .NET

## Overview

Welcome to the GitHub repository for "Dapr Unleashed: Accelerating Microservice Development in .NET." This repository serves as a companion to my lecture at Sinergija23 (https://sinergija.live/en/2023) and is designed to demonstrate the practical benefits of integrating Dapr (Distributed Application Runtime) into a .NET microservices architecture. Here is the link to my lecture: https://sinergija.live/en/2023/lectures/370/rapid-development-with-dapr
The slides from my lecture are here: https://www.slideshare.net/MiroslavJaneski/dapr-unleashed-accelerating-microservice-development

## Before you run
To run any of the both solutions you need the following Azure resources:

1. Azure Service Bus Instance with Basic pricing tier
	1. In the Azure Service Bus Instance create two topics: "transform" and "extract".
2. Azure CosmosDB Instance (Serverless Capacity mode)
	1. Create a Database with DatabaseId: "dapr-unleashed-cosmosdb-dev"
	2. Create a Container with partitionKey: /partitionKey and id: id
3. Azure KeyVault Instance with Standard pricing tier
	1. In the Azure Key Vault add the following secrets:
		1. dapr-unleashed-cosmosdb-dev-key: CosmosDB master key
		2. dapr-unleashed-sb-dev: Azure Service bus connection string
		3. dapr-unleashed-cosmosdb-dev: CosmosDB connection key
4. Create application instance and grant list secrect access in the Azure KeyVault instance

## Repository Structure

The repository is organized into two main branches:

1. **`develop`**: Contains the .NET application without Dapr integration. This branch serves as a baseline to demonstrate typical microservice development scenarios.
   
2. **`dapr`**: Features the same .NET application, but enhanced with Dapr. This branch illustrates the advantages and improvements Dapr brings to microservice development, particularly in terms of reduction of source code, confiv over source code, and ease of development.

## Getting Started

To get started with this demo:

1. Clone the repository:
git clone https://github.com/janeski/dapr-unleashed.git


2. Navigate to the repository directory:

cd dapr-unleashed


### Running the Application Without Dapr

To run the application without Dapr:

1. Switch to the `develop` branch:

git checkout develop

2. Create docker-compose file and add the 3 services in the docker-compose file

3. Add environment variable to each docker container for the following values
	1. DaprUnleashedKeyVault
	2. AZURE_TENANT_ID
	3. AZURE_CLIENT_ID
	4. AZURE_CLIENT_ID

```
version: '3.4'

services:
  daprunleashed.api:
    image: ${DOCKER_REGISTRY-}daprunleashedapi
    environment:
        "DaprUnleashedKeyVault": ""
        "AZURE_TENANT_ID": ""
        "AZURE_CLIENT_ID": ""
        "AZURE_CLIENT_SECRET": ""
    build:
      context: .
      dockerfile: source/DaprUnleashed.IngestionService/Dockerfile

  daprunleashed.transformationservice:
    image: ${DOCKER_REGISTRY-}daprunleashedtransformationservice
    environment:
        "DaprUnleashedKeyVault": ""
        "AZURE_TENANT_ID": ""
        "AZURE_CLIENT_ID": ""
        "AZURE_CLIENT_SECRET": ""
    build:
      context: .
      dockerfile: source/DaprUnleashed.TransformationService/Dockerfile


  daprunleashed.extractionservice:
    image: ${DOCKER_REGISTRY-}daprunleashedextractionservice
    environment:
        "DaprUnleashedKeyVault": ""
        "AZURE_TENANT_ID": ""
        "AZURE_CLIENT_ID": ""
        "AZURE_CLIENT_SECRET": ""
    build:
      context: .
      dockerfile: source/DaprUnleashed.ExtractionService/Dockerfile

```

### Running the Application With Dapr

To experience the application with Dapr integration:

1. Switch to the `dapr` branch:

2. In the components folder add the following file: azurekeyvault.yaml based on the following link: https://docs.dapr.io/reference/components-reference/supported-secret-stores/azure-keyvault/


## Contributing

Contributions to improve the application or documentation are welcome. Please feel free to submit issues or pull requests.

## License

This project is licensed under the MIT License - see the `LICENSE` file for details.

## Contact

For any questions or further information, please contact:

- Miroslav Janeski
- Email: miroslav.janeski[at]gmail.com
- Project Link: https://github.com/janeski/dapr-unleashed
