# PromoCodeManagerApi

PromoCodeManagerApi is a .net Core RestAPI to manage Promo Codes

## Database Installation

The connection string should be specified in the efcoreSettings.json file.
(for dev environment efcoreSettings.Development.json)


```bash
{
  "ConnectionStrings": {
    "DevConn": "Data Source=database;Initial Catalog=OYAssignment;Persist Security Info=True;User ID=userId;Password=password;MultipleActiveResultSets=True"
  }
}
```

## Mock Data

The mock data for the application demo will be created by the code at first running. Promo Codes will be generated randomly 

The user list will be created as below:

```python
email: john.doe@algroup.org
password: 1234

email: hiro.hamata@algroup.org
password: 1234

email: jane.brown@algroup.org
password: 1234

```

# User Interface
The user interface has been developed with Angular

## Installation

The endpoint of the RestAPI should be specified in the web-config.json file:

```bash
{
    "apiEndpoint":"https://localhost:44355/api/"
}

```

## Npm Packages
The nmp packages should be installed before running the application.

```cmd
npm install
```


After packages installed, to run the Angular application:
```ng
ng serve

```



