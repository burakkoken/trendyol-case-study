# Trendyol Case Study

Basic product list project with 1 billion records. It's working with cache. I used ASP.NET Core Web API and MVC, Azure Storage Queue, Redis Cache and PostgreSQL DBMS 

[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/?repository=https://github.com/peacecwz/trendyol-case-study)


## Getting Started

First of all, you need to clone the project to your local machine

```
git clone https://github.com/peacecwz/trendyol-case-study.git
cd trendyol-case-study
```

### Building

A step by step series of building that project

1. Restore the project :hammer:

```
dotnet restore
```

2. Update appsettings.json or appsettings.Development.json (Which you are working stage)

2. Change all connections for your development or production stage

3. If you want to use different Database Provider (MS SQL, MySQL etc...) You can change on Data layer File: DIExtensions.cs (Line: 58)

```
    //For Microsoft SQL Server
    services.AddDbContext<BestProductsDbContext>(options => options.UseSqlServer(connectionString, opt => opt.MigrationsAssembly("BestProductsApp.Data")));
```

5. Run EF Core Migrations

```
dotnet ef database update
```

## Running

### Run with Dotnet CLI

1. Run API project :bomb:

```
dotnet run -p ./BestProductsApp.API/BestProductsApp.API.csproj
```

2. Run Webjob project :boom:

```
dotnet run -p ./BestProductsApp.Function/BestProductsApp.Function.csproj
```

3. Run MVC project :bomb:

```
dotnet run -p ./BestProductsApp.MVC/BestProductsApp.MVC.csproj
```

### Run on Docker (only for API Project)

Run docker compose commands in API Project :boom:

```bash
docker-compose build
docker-compose run
```

## Built With

* [.NET Core 2.0](https://www.microsoft.com/net/) 
* [Entitiy Framework Core](https://docs.microsoft.com/en-us/ef/core/) - .NET ORM Tool
* [NpgSQL for EF Core](http://www.npgsql.org/efcore/) - PostgreSQL extension for EF 
* [Swagger](https://swagger.io/) - API developer tools for testing and documention
* [Azure Storage](https://azure.microsoft.com/en-us/services/storage/)
* [Azure Webjobs](https://github.com/Azure/azure-webjobs-sdk/wiki)
* [Redis Cache](https://github.com/StackExchange/StackExchange.Redis)

## Contributing

* If you want to contribute to codes, create pull request
* If you find any bugs or error, create an issue
