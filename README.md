# Networked Math Quiz
> Providing a solution for math quizzes over a network.

Networked Math Quiz aims to provide a simple interface and workflow for
a teacher to send math questions to a student and for a student to respond
with their answer.

## Installing / Getting started

### Requirements
- .NET Core SDK (https://dotnet.microsoft.com/download)
- BinaryTree (https://github.com/Marusyk/BinaryTree)
- ENet-CSharp (https://github.com/nxrighthere/ENet-CSharp)
- Microsoft.NET.Test.Sdk (https://github.com/microsoft/vstest/)
- Microsoft.Xaml.Behaviours.Wpf (https://github.com/Microsoft/XamlBehaviorsWpf)
- Unity Container (http://unitycontainer.org/)
- xUnit (https://xunit.net/) - for testing
- xunit.runner.visualstudio (https://github.com/xunit/visualstudio.xunit) - for testing in Visual Studio
  
### 1. Download/Clone The Project
Either download the repo source code to a folder on your local machine,
or clone the repository to a folder on your local machine using:
```shell
> git clone https://github.com/Ltven0mI/TAFE-Networked-Math-Quiz.git
```

### 2. Install The .NET Core SDK
Download and install the .NET Core SDK from [here](https://dotnet.microsoft.com/download)

### 3. Restore The Project
In order to ensure all required packages are installed, run the following
command inside the folder containing the solution (.sln) file:
```shell
> dotnet restore
```

### 4. Running The Application
Run the following command from inside the solution folder:
```shell
> dotnet run
```

## Features

The Networked Math Quiz applications offer many features, some of which are:
* Creation, submission, and answering of math questions.
* Sorting of asked questions in a tabular format.
* Displaying of incorrectly answered questions in a linked list format.
* Searching for a specific asked question.
* Displaying / saving of all asked questions in different traversal orders.

## Contributing

If you wish to contribute to this project, please fork the repository and use
a feature branch. Pull requests are warmly welcome.

When contributing please ensure you are following the programming standards
outlined in [CONTRIBUTING.md](https://github.com/Ltven0mI/TAFE-Networked-Math-Quiz/blob/main/CONTRIBUTING.md).

## Links

- Project homepage: https://github.com/Ltven0mI/TAFE-Networked-Math-Quiz/
- Repository: https://github.com/Ltven0mI/TAFE-Networked-Math-Quiz/
- Issue tracker: https://github.com/Ltven0mI/TAFE-Networked-Math-Quiz/issues
  - In case of sensitive bugs like security vulnerabilities, please contact
    wade.xr@explodybeans.net directly instead of using issue tracker.
    We value your effort to improve the security and privacy of this project!


## Licensing

The code in this project is licensed under an [MIT](https://github.com/Ltven0mI/TAFE-Networked-Math-Quiz/blob/main/LICENSE) license.