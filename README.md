# ArgumentParser
A small class to help you parsing string array arguments into objects.
With this class you can save the time spent on validations created for console applications.

The ArgumentsParser just take an array of strings input and returned an instance of a class,
binding the arguments to class properties. The binding is done using attributes over the
properties, those attribute let you define required arguments, default value for properties
and regex validation for argument value.
