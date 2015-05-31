using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IPValverde.ArgumentParser.Exceptions;

namespace IPValverde.ArgumentParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        #region Test duplicate name and alias
        public class DuplicateArgumentName
        {
            [Argument("-property")]
            public string Property1 { get; set; }

            [Argument("-property")]
            public string Property2 { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateArgumentException),
            "A duplicated attribute name was incorrectly accepted.")]
        public void ParseArguments_DuplicateArgumentName()
        {
            try
            {
                var parsedObject = Parser.ParseArguments<DuplicateArgumentName>();
            }
            catch(DuplicateArgumentException ex)
            {
                Assert.AreEqual("-property", ex.ArgumentIdentifier);
                throw ex;
            }
        }
        

        public class DuplicateArgumentAlias
        {
            [Argument("-property1", Alias="-p")]
            public string Property1 { get; set; }

            [Argument("-property2", Alias = "-p")]
            public string Property2 { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateArgumentException),
            "A duplicated attribute alias was incorrectly accepted.")]
        public void ParseArguments_DuplicateArgumentAlias()
        {
            try
            {
                var parsedObject = Parser.ParseArguments<DuplicateArgumentAlias>();
            }
            catch(DuplicateArgumentException ex)
            {
                Assert.AreEqual("-p", ex.ArgumentIdentifier);
                throw ex;
            }
        }

        public class DuplicatedArgumentIdentifier
        {
            [Argument("-prop", Alias="-p")]
            public string Property1 { get; set; }

            [Argument("-property2", Alias="-prop")]
            public string Property2 { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateArgumentException),
            "A duplicated attribute alias was incorrectly accepted.")]
        public void ParseArguments_DuplicateArgumentIdentifier()
        {
            try
            {
                var parsedObject = Parser.ParseArguments<DuplicatedArgumentIdentifier>();
            }
            catch(DuplicateArgumentException ex)
            {
                Assert.AreEqual("-prop", ex.ArgumentIdentifier);
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Class used for invalid argument value and invalid argument tests.
        /// </summary>
        public class InvalidArgumentTestClass
        {
            [Argument("-name", Alias="-n",
                Required = true)]
            public string Name { get; set; }

            [Argument("-age", Alias="-a")]
            public int Age { get; set; }

            [Argument("-dayOfBirth", Alias = "-d",
                RegexValidation = @"^((3[01])|([1-2][0-9])|([1-9]))$")]
            public int DayOfBirth { get; set; }
        }

        #region Test invalid argument value

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentValueException))]
        public void ParseArguments_NoArgumentValueGiven()
        {
            try
            {
                var paramObj = Parser.ParseArguments<InvalidArgumentTestClass>(
                    "-d", "28",
                    "-name");
            }
            catch(InvalidArgumentValueException ex)
            {
                Assert.IsNull(ex.ArgumentValue);
                Assert.AreEqual("-name", ex.ArgumentName);

                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentValueException))]
        public void ParseArguments_EmptyValueForRequiredArgument()
        {
            try
            {
                var paramObj = Parser.ParseArguments<InvalidArgumentTestClass>(
                    "-d", "28",
                    "-name", string.Empty);
            }
            catch (InvalidArgumentValueException ex)
            {
                Assert.AreEqual("-name", ex.ArgumentName);
                Assert.AreEqual(string.Empty, ex.ArgumentValue);

                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentValueException))]
        public void ParseArguments_RegexInvalidArgumentValue()
        {
            try
            {
                var paramObj = Parser.ParseArguments<InvalidArgumentTestClass>(
                    "-d", "-28",
                    "-name", "John");
            }
            catch (InvalidArgumentValueException ex)
            {
                Assert.AreEqual("-d", ex.ArgumentName);
                Assert.AreEqual("-28", ex.ArgumentValue);

                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentValueException))]
        public void ParseArguments_InvalidArgumentValueType()
        {
            try
            {
                var paramObj = Parser.ParseArguments<InvalidArgumentTestClass>(
                    "-age", "NaN",
                    "-name", "John");
            }
            catch (InvalidArgumentValueException ex)
            {
                Assert.AreEqual("-age", ex.ArgumentName);
                Assert.AreEqual("NaN", ex.ArgumentValue);

                throw ex;
            }
        }

        #endregion

        #region Test invalid argument

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ParseArguments_InvalidArgumentIdentifier()
        {
            try
            {
                var paramObj = Parser.ParseArguments<InvalidArgumentTestClass>(
                    "-d", "28",
                    "-name", "John",
                    "-lastName", "Locke");
            }
            catch (InvalidArgumentException ex)
            {
                Assert.AreEqual("-lastName", ex.ArgumentName);

                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredArgumentMissingException))]
        public void ParseArguments_RequiredArgumentMissing()
        {
            try
            {
                var paramObj = Parser.ParseArguments<InvalidArgumentTestClass>(
                    "-d", "11",
                    "-age", "25");
            }
            catch(RequiredArgumentMissingException ex)
            {
                Assert.AreEqual("-name", ex.ArgumentName);

                throw ex;
            }
        }

        #endregion


        #region Test valid parameters and arguments

        public class Parameters
        {
            [Argument("-silent", Alias="-s")]
            public bool SilentMode { get; set; }

            [Argument("-minified", Alias = "-m")]
            public bool Minified { get; set; }

            [Argument("-precision", Alias = "-p")]
            public double Precision { get; set; }

            [Argument("-output-name", Alias = "-o")]
            public string OutputName { get; set; }

            [Argument("-format", Alias = "-f")]
            public string Format { get; set; }

            [Argument("-date", Alias = "-d",
                RegexValidation = @"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}(:\d{2})?$")]
            public DateTime BaseDateTime { get; set; }

            [Argument("-emailFrom", Alias = "-e",
                DefaultValue = "contact@mydomain.com",
                RegexValidation = @"^\w([\w.]*\w)?@\w([\w.]+\w)?")]
            public string EmailFrom { get; set; }

            [Argument("-timeout", Alias = "-t")]
            public int Timeout { get; set; }

            public string NotBindedProperty { get; set; }
        }

        [TestMethod]
        public void ParseArguments_ValidArguments()
        {
            var parametersObject = Parser.ParseArguments<Parameters>(new []
                                    {
                                        // This argument is binded to a boolean property, so
                                        // there is no value associated to it, it's existence
                                        // is enough to set a true value to the property.
                                        "-silent",
                                        "-precision", "192",
                                        "-o", "output filename.txt",
                                        "-f", "hh:mm:ss dd/MM/yyyy",
                                        "-timeout", "120",
                                        "-d", "11/04/2015 20:50"
                                    });

            // This property should be null, as it's not binded
            Assert.IsNull(parametersObject.NotBindedProperty);

            // This property should be false, ad it was not given on arguments array
            Assert.AreEqual(false, parametersObject.Minified);

            Assert.AreEqual(true, parametersObject.SilentMode);
            Assert.AreEqual(192, parametersObject.Precision);
            Assert.AreEqual("output filename.txt", parametersObject.OutputName);
            Assert.AreEqual("hh:mm:ss dd/MM/yyyy", parametersObject.Format);
            Assert.AreEqual(120, parametersObject.Timeout);
            Assert.AreEqual(DateTime.Parse("11/04/2015 20:50"), parametersObject.BaseDateTime);

            // Assert devault value is set
            Assert.AreEqual("contact@mydomain.com", parametersObject.EmailFrom);
        }

        #endregion
    }
}
