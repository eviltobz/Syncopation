module Syncopation.Core.Feeds.Tests

open FsUnitTyped
open NUnit.Framework
open Syncopation.Core

(*
module Say =
    let hello name =
        printfn "Hello %s" name
*)

[<Test>]
let GoBoom () =
    Feeds.DoAThing()
    Assert.Fail("boom")
