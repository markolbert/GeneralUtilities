# Validate File Paths

`FileExtensions.ValidateFilePath` accepts an enumerable of paths where the file being validated might be located.

`ValidateFilePath` looks for the required file as follows:

- if a required extension was specified and the path to check's extension doesn't match it or is missing, the path's extension is corrected
- if write access to the file *is not* required and the path exists, the result is set to the path and true is returned
- if write access *is* required and the path exists and is writeable, the result is set to the path and true is returned
- any specified folders are searched in the following sequence
  - if the path is not rooted (i.e., it's relative), the specified folders are searched using the relative path. If a match is found with the right access rights the result is set to its path and true is returned
  - if a file was not found, or if the path is rooted (i.e., it's absolute), the specified folders are searched for the file name component of the path alone. If a match is found with the right access rights the result is set to its path and true is returned
- if no match was found, the result is set to null and false is returned.

[return to readme](../readme.md)
