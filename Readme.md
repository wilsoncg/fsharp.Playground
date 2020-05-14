
### Useful links:
* [F# Async Guide - Lev Gorodinski, Jet](https://medium.com/jettech/f-async-guide-eb3c8a2d180a)
* [Async in C# & F# Asynchronous gotchas - Tomas Petricek](http://tomasp.net/blog/csharp-async-gotchas.aspx/)
* [Asynchronous Everything - Joe Duffy, Microsoft Midori/Singularity managed OS project](http://joeduffyblog.com/2015/11/19/asynchronous-everything/)
* [Functors, Applicatives, And Monads In Pictures](http://adit.io/posts/2013-04-17-functors,_applicatives,_and_monads_in_pictures.html)

### Reader Monad:
* [Advanced functional programming in F# using F#+](https://www.youtube.com/watch?v=pxJCHJgG8ws) - [slides](https://starychfojtu.github.io/#/)
* [Reinventing the Reader monad](https://fsharpforfunandprofit.com/posts/elevated-world-6/)

### Writer Monad:
* [Writer monad with pictures - Aditya Bhargava](http://adit.io/posts/2013-06-10-three-useful-monads.html#the-writer-monad)
* [An example implementation of a writer monad - Matthew Podwysocki](http://codebetter.com/matthewpodwysocki/2010/02/02/a-kick-in-the-monads-writer-edition/)
* [Stateful computations in F# with update monads - Thomas Petricek](http://tomasp.net/blog/2014/update-monads/)
* [A monad writer for F# - Nicolas Perez](https://hackernoon.com/a-monad-writer-for-f-26aa987e4a3a)
* [Event sourcing using writer monad - Nicolas Perez](https://hackernoon.com/event-sourcing-using-writer-monad-b26a390285a)
* [F#+ API reference Writer<'monoid,'t>](https://fsprojects.github.io/FSharpPlus/reference/fsharpplus-data-writer-2.html)

### Other ideas:
* [IO Monad](http://theinnerlight.github.io/NovelIO/oopintro.html)
* [Combining free Monads in F# - Mark Seemann](https://blog.ploeh.dk/2017/07/31/combining-free-monads-in-f/)
* [F# free monad recipe - Mark Seemann](https://blog.ploeh.dk/2017/08/07/f-free-monad-recipe/)

### Applicatives & Validation:
Extensions for Result type
* [FsToolkit.ErrorHandling](https://github.com/demystifyfp/FsToolkit.ErrorHandling)

Support in Computation Expressions (CE)
* [RFC-FS-1063](https://github.com/fsharp/fslang-design/blob/master/preview/FS-1063-support-letbang-andbang-for-applicative-functors.md) - let! ... and! support for applicative functors
* [PullRequest 5696](https://github.com/Microsoft/visualfsharp/pull/5696)
* [FsLang issue 579](https://github.com/fsharp/fslang-suggestions/issues/579)

##### If Tests do not appear in VS Test Explorer
[Follow steps outlined](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-fsharp-with-nunit)  (install from nuget)
* Microsoft.NET.Test.Sdk
* NUnit
* NUnit3TestAdapter