# Kodlama.io-devs

.Net training using CQRS architecture in accordance with SOLID principles.

## :open_file_folder: View

<p align="center">
  <img src="https://user-images.githubusercontent.com/74189776/192102845-7cafd28f-6f05-4fe7-af98-2a2ef5395d99.png" alt="view"/>
</p>

## :newspaper: Postman Documentation

### [Go To The Postman Documentation](https://documenter.getpostman.com/view/22932272/2s83KQFSSk)

## :star2: Startup

### :arrow_double_down: Configure connection string in appsettings.json :arrow_double_down:

``` ruby
ConnectionStrings": {
    "PostgreSql": "User ID=postgres;Password="ExamplePassword";Host=localhost;Port=5432;Database=KodlamaIoDevs;"
  }
```

---

### :arrow_double_down: Create migration and database in package manager console :arrow_double_down:

<p align="left">
  <img src="https://user-images.githubusercontent.com/74189776/192103970-063f6de0-4f7e-4e07-bf85-89b2c3c73a61.png" alt="migration"/>
</p>

---

### :arrow_double_down: Configure token options in appsettings.json :arrow_double_down:

``` ruby
"TokenOptions": {
    "Audience": "kodlama.io.devs",
    "Issuer": "Gresta",
    "AccessTokenExpiration": 30,
    "SecurityKey": "jKr1u.JDX5E14ZDs.K5ph8j7zEB.Vz82Mk",
    "RefreshTokenTTL": 2
  }
```

## :floppy_disk: ERD Diagram

<p align="center">
  <img src="https://user-images.githubusercontent.com/74189776/192105010-3a950b77-f11c-439d-b7e4-7206ca9c2d60.png" alt="erd"/>
</p>
