# PatientsApi

PatientsApi is a take home assignment from Tensova AI Business Solutions Inc. & MedMinder

## The Design

PatientsApi was built following the Clean Architecture Design. The design consists of four layers: presentation (The API itself), infrastructure, application, and domain. PatientsApi also incorporate coding patterns such as repository, mediator, and CQRS (Command and Query Responsibility Segregation).

## Run the Project

1. Fork and clone the project repository.
2. Navigate to the project file directory and build the project by typing this command `dotnet build`.
3. Run the project by typing this command `dotnet run --project .\src\Api\`.
4. To access the SwaggerUI Documentation, open your preferred browser and go to `http://localhost:5095/swagger`.

## Test the Project

1. Fork and clone the project repository.
2. Navigate to the project file directory and build the project by typing this command `dotnet build`.
3. Test the project by typing this command `dotnet test`.
