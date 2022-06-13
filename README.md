# <h1 align="center">SmartUI.Forms</h1>

<h4 align="center">The light weight Form elements Components (e.g. Input Number, Input Text, DropDownList, MultiSelect and more) supports more additional features than the existing in Blazor.</h4>

<p align="center">
  <a href="#key-features">Key Features</a> •
  <a href="#key-features">Important Notice</a> •
  <a href="#how-to-use">Installation</a> •
  <a href="#how-to-use">How To Use</a> •
  <a href="#license">License</a>
  <a href="#you-may-also-like">You may also like</a> •
</p>

## Key Features

* Input Number
  - InputNumber now support following types => `(int, int?, float, float?, double, double?, decimal, decimal?, double, double?, UInt16, UInt32, UInt64)`.
  - Validation.
  - two way binding on key press.
  - `OnChange()` along with `@bind-value`
* Input Text
  - two way binding on key press.
  - Validation.
  - `OnChange()` along with `@bind-value`

## Important Notice
This project is still under active development! Currently an alpha version is available on NuGet, but keep in mind that a later version might contain breaking changes. 

```
### Planned work: 
- [] Code cleanup
- [] Better documentation
```

### Installation
1. Install the [NuGet](https://www.nuget.org/packages/SmartUI.Forms/) package:

   ```
   > dotnet add package SmartUI.Forms
   
   OR
   
   PM> Install-Package SmartUI.Forms
   ```
   Use the `--version` option to specify a specific version to install.

   Or use the build in NuGet package manager of your favorite IDEA. Simply search for `SmartUI.Forms`, select a version and hit install.

2. Import the components:

   Add the following using statement `@using SmartUI.Forms` to one of the following: 
   - For global import add it to your  `_Imports.razor` file.
   - For a scoped import add  it to your desired Blazor component.
  
## How To Use

   InputNumber 
   ```cs            
   -- Basic --

	<SmartInputNumber TValue="int?" @bind-Value="person.IntNumber" Placeholder="Enter Intger Number" />

   -- Two way binding on KeyPress --

	<SmartInputNumber TValue="float" @bind-Value="person.FloatNumber" IsChangeOnKeyPress="true" Placeholder="Enter Float Number" />

   -- OnChange() event along with @bind-value --

	<SmartInputNumber TValue="float" @bind-Value="person.FloatNumber" @OnChange="(e) => Console.WriteLine(e)"
					  IsChangeOnKeyPress="true" Placeholder="Enter Float Value" />

   ```

   InputText 
   ```cs            
   -- Basic --

	<SmartInputText @bind-Value="person.SampleText" Placeholder="Enter SampleText Here" />

   -- Two way binding on KeyPress --

	<SmartInputText @bind-Value="person.SampleText" IsChangeOnKeyPress="true" Placeholder="Enter SampleText Here" />

   -- OnChange() event along with @bind-value --

	<SmartInputText @bind-Value="person.SampleText" IsChangeOnKeyPress="true" OnChange="(e) => Console.WriteLine(e)"
					Placeholder="Enter SampleText Here" />

   ```

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
  
## You may also like...

- [SmartUI.Grid](https://github.com/MohammadFayed97/.Grid) - The light weight DataTable created with Blazor and support Filter, Sort, Pagination and more Features.
