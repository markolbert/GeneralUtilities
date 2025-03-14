# J4JSoftware.GeneralUtilities

|Version|Description|
|:-----:|-----------|
|1.0.0|initial release|

## 1.0.0

The assembly incorporates much of what was previously in J4JSoftware.MiscellaneousUtilities. I reset the verison
number to highlight that change.

## 2.2.0

*The following API was moved to my [DependencyInjection project](https://github.com/markolbert/ProgrammingUtilities/tree/master/DependencyInjection)*.

`FileExtensions.ValidateFilePath` now accepts an enumerable of paths where the file being validated might be located. It also supports logging via my `IJ4JLogger` system. All log events are set at the Verbose level.

`ValidateFilePath` looks for the required file as follows:

- if a required extension was specified and the path to check's extension doesn't match it or is missing, the path's extension is corrected
- if write access to the file *is not* required and the path exists, the result is set to the path and true is returned
- if write access *is* required and the path exists and is writeable, the result is set to the path and true is returned
- any specified folders are searched in the following sequence
  - if the path is not rooted (i.e., it's relative), the specified folders are searched using the relative path. If a match is found with the right access rights the result is set to its path and true is returned
  - if a file was not found, or if the path is rooted (i.e., it's absolute), the specified folders are searched for the file name component of the path alone. If a match is found with the right access rights the result is set to its path and true is returned
- if no match was found, the result is set to null and false is returned.

## 2.1.0

- Added extension method for extracting PropertyInfo from an Expression `GetPropertyInfo<TContainer, TProp>()`
- Added a static method for validating configuration and output files `FileExtensions.ValidateFilePath()`
- Added static hashing methods for strings, e.g., `CalculateHash()`
