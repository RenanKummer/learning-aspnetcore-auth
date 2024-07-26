# Learning Series: ASP.NET Core Authentication & Authorization

This application explores how to configure authentication and authorization in ASP.NET Core.

## Local Setup

### Pre-requirements

- Git
- [.NET SDK 8](https://dotnet.microsoft.com/download)
- IDE compatible with C#/.NET
    - [Visual Studio Code](https://code.visualstudio.com)
    - [JetBrains Rider](https://jetbrains.com/rider)

### How to run?

1. Clone this repository to your computer.

```shell
# Option 1: SSH
$ git clone git@github.com:RenanKummer/learning-aspnetcore-auth.git

# Option 2: HTTPS
$ git clone https://github.com/RenanKummer/learning-aspnetcore-auth.git
```

2. Open the solution in your preferred IDE.


3. Configure `dev` profile at `src/Learning.AspNetCore.Auth.WebApi/Properties/launchSettings.json`.

```json5
// launchSettings.json
// This is an example file - feel free to adjust settings as needed.
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "dev": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "applicationUrl": "http://localhost:8080",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

4. Run the application with `dev` profile.
   - Option 1: On your preferred IDE, select the profile and press the `Run` button.
   - Option 2: On the terminal, execute the following command in the solution folder:

```shell
$ dotnet run --project ./src/Learning.AspNetCore.Auth.WebApi --launch-profile dev
```
