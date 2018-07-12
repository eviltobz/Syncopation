module Syncopation.Core.Feeds.Tests

open FsUnitTyped
open NUnit.Framework
open Syncopation.Core


[<TestCase("SampleFeeds\comcompod.xml")>]
[<TestCase("SampleFeeds\dnr.xml")>]
[<TestCase("..\..\..\..\Syncopation.Core\SampleFeeds\MasterSample.xml")>]
let ParseSampleFeeds (source: string) =
    //let feed = Feeds.Rss.Load source
    Feeds.DoAThing source
    Assert.Fail("boom")

