# Argument Parser

[![argument-parser MyGet Build Status](https://www.myget.org/BuildSource/Badge/argument-parser?identifier=8e48172a-4448-4b24-b56d-22680b7f86fd)](https://www.myget.org/)

A small class to help you parsing string array arguments into objects.
With this class you can save the time spent on validations created for console applications.

The ArgumentsParser just take an array of strings input and returns an instance of a class,
binding the arguments to class properties. The binding is done using attributes over the
properties, those attributes let you define required arguments, default value for properties
and regex validation for argument value.
