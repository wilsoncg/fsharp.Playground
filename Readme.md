
### Useful links:
* [F# Async Guide - Lev Gorodinski, Jet](https://medium.com/jettech/f-async-guide-eb3c8a2d180a)
* [Async in C# & F# Asynchronous gotchas - Tomas Petricek](http://tomasp.net/blog/csharp-async-gotchas.aspx/)
* [Asynchronous Everything - Joe Duffy, Microsoft Midori/Singularity managed OS project](http://joeduffyblog.com/2015/11/19/asynchronous-everything/)

### Other ideas:
* [IO Monad](http://theinnerlight.github.io/NovelIO/oopintro.html)

### Applicatives & Validation:
Extensions for Result type
* [FsToolkit.ErrorHandling](https://github.com/demystifyfp/FsToolkit.ErrorHandling)

Support in Computation Expressions (CE)
* [RFC-FS-1063](https://github.com/fsharp/fslang-design/blob/master/RFCs/FS-1063-support-letbang-andbang-for-applicative-functors.md) - Overview of applicatives
* [PullRequest 5696](https://github.com/Microsoft/visualfsharp/pull/5696)
* [FsLang issue 579](https://github.com/fsharp/fslang-suggestions/issues/579)

##### If Tests do not appear in VS Test Explorer
[Follow steps outlined](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-fsharp-with-nunit)  (install from nuget)
* Microsoft.NET.Test.Sdk
* NUnit
* NUnit3TestAdapter