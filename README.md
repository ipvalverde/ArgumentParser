# Argument Parser

[![argument-parser MyGet Build Status](https://www.myget.org/BuildSource/Badge/argument-parser?identifier=8e48172a-4448-4b24-b56d-22680b7f86fd)](https://www.myget.org/)

A small class to help you parsing string array arguments into objects.
With this class you can save the time spent on validations created for console applications.

The ArgumentsParser just take an array of strings input and returns an instance of a class,
binding the arguments to class properties. The binding is done using attributes over the
properties, those attributes let you define required arguments, default value for properties
and regex validation for argument value.

# How to install

To install ArgumentsParser, run the following command in the [Package Manager Console](https://www.nuget.org/packages/IPValverde.ArgumentParser/)

```
Install-Package IPValverde.ArgumentParser
```

# How to use

ArgumentParser is responsible for binding arguments (an array of string with argument name and value) to class properties.

So first of all, you need a class with writeable properties, then you need to decorate them with an ArgumentAttribute. The ArgumentAttribute requires and argument name.

```cs
using IPValverde.ArgumentParser;

// Class deifnition
public class MyAppArguments
{
  // This will bind the argument "-name" found in arguments array
  // to this property.
  [Argument("-name")]
  public string OutputFilename { get; set; }
}

// Main method
static void Main(string[] args)
{
  // Create an instance of MyAppArguments and bind the arguments
  // to that instance.
  var argumentsObj = Parser.ParseArguments<MyAppArguments>(args);
  
  // If the "args" array is: { "-name", "John" },
  // then "John" will be printed here.
  Console.WriteLine(argumentsObj.OutputFilename);
}
```

ArgumentAttribute
--------------

The ArgumentAttribute contains some optional properties. Thos properties are shown bellow:

```cs
public class Arguments
{
  // Define the argument name, the name must be unique among
  // other names and aliases.
  [Argument("-name",
    
	// Alias is another identifier that can be used to match
	// an argument to a property, you should use it as a shortcut.
	// The alias must be unique among other names and aliases.
    Alias = "-n",
	
	// A regular expression which will be applied over the
	// argument value.
	RegexValidation = @"^((\w+ )*\w+)$",
	
	// If the argument "-name" or "-n" is not provided, this 
	// DefaultValue will be assigned to this property.
	// This property is an object, so beside strings, it also
	// accepts value types.
	DefaultValue = "Guest",
	
	// Flag this property as required, if a required property is not
	// provided an exception is thrown by "ParseArguments".
	// This property does not plays well with "DefaultValue".
	Required = true
	)]
  public string Name;
}
```

Parser.ParseArguments
--------------

The method ParseArguments is responsible for parsing the array of string arguments keys and values to a new instance of the desired class. Some behaviours of this method are noted below:

- Properties of type boolean don't expect a value in arguments, it's existence will evaluate the property to be "true".
- Some exceptions are thrown by the ParseArguments methods, here is a table explain when those exceptions are thrown.

<table>
	<tr>
		<td>Exception</td>
		<td>When it is thrown</td>
	</tr>
	<tr>
		<td>InvalidArgumentException</td>
		<td>when an invalid argument is found on "arguments" string array</td>
	</tr>
	<tr>
		<td>InvalidArgumentValueException</td>
		<td>
			<ul>
				<li>when a required argument has an null or empty string value;</li>
				<li>when the RegeValidation did not match the argument value;</li>
				<li>when the argument value cannot be converted from a string to the property type it's binded to;</li>
				<li>when a value is not provided to an argument;</li>
			</ul>
		</td>
	</tr>
	<tr>
		<td>DuplicatedArgumentException</td>
		<td>when an argument name or alias is not unique</td>
	</tr>
	<tr>
		<td>RequiredArgumentMissingException</td>
		<td>when a required argument is not found on "arguments" string array</td>
	</tr>
</table>