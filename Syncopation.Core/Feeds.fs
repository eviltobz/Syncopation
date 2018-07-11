namespace Syncopation.Core

open FSharp.Data

module Feeds =

    // Let's get a cut-down sample RSS file to use & keep in source. look for differences in a few of the casts I listen to.
    //type Rss = XmlProvider<"http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master">
    type Rss = XmlProvider<"SampleFeeds/dnr.xml">

    let DoAThing () =
        //let a = Rss.Load("http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master")
        let feed = Rss.GetSample()
        printfn "Title: %s" feed.Channel.Title
        printfn "Link: %s" feed.Channel.Link
        printfn "Summary: %s" feed.Channel.Summary
        printfn "Description: %s" feed.Channel.Description
        printfn "Subtitle: %s" feed.Channel.Subtitle
        printfn "Image1: %s" feed.Channel.Image.Url
        printfn "Image2: %s" feed.Channel.Image2.Href
        printfn "Author: %s" feed.Channel.Author
        printfn "Owner: %s" feed.Channel.Owner.Name
        printfn "PubDate: %s, LastBuildDate: %s" feed.Channel.PubDate feed.Channel.LastBuildDate
        printfn "Cloud?: %s" (feed.Channel.Cloud.ToString())
        printfn "Categories: %s" (System.String.Join( ", ", feed.Channel.Categories))
        feed.Channel.Items
        |> Seq.iteri (fun index item ->
            printfn "%i - %s" index item.Title
            )

        printfn "*************************"
        printfn "hmm: %A" feed.Channel
