# String Utility Methods

`StringExtensions` contains methods for hashing strings and removing white space from strings.

## CalculateHash (static method)

There are two versions of `CalculateHash` in the library. This first one is simply a static method that takes various arguments and a `params` collection of strings and returns an `SHA256` has of them.

|Argument|Type|Comments|
|--------|----|--------|
|caseInsensitive|`bool`|a flag indicating whether or not case matters|
|ignoreWhiteSpace|`bool`|a flag indicating whether or not to ignore white space|
|toHash|`params string[]`|the collection of strings to hash|

## CalculateHash (extension method)

This second variant is an extension method useful when you want to create multiple hashes using the same requirements as to case sensitivity and white space. It relies on a `HashContext` record:

### HashRecord definition

|Record Parameter|Type|Comments|
|--------|----|--------|
|CaseInsensitive|`bool`|a flag indicating whether or not case matters (defaults to `true`)|
|IgnoreWhiteSpace|`bool`|a flag indicating whether or not to ignore white space (defaults to `true`)|

Once you've defined an instance of `HashContext` you can call this second `CalculateHash` on it, each time supplying a different set of strings.

## RemoveWhiteSpace

This method accepts a single `string` as an extension parameter, and returns a new string with all whitespace characters from the supplied parameter.
