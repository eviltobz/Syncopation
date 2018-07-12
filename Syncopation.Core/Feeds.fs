namespace Syncopation.Core

open FSharp.Data

module Feeds =

    // Let's get a cut-down sample RSS file to use & keep in source. look for differences in a few of the casts I listen to.
    //type Rss = XmlProvider<"http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master">
    type Rss = XmlProvider<"..\Syncopation.Core\SampleFeeds\MasterSample.xml">

    let print title value =
        try
            printfn "%s: %s" title (value().ToString())
        with
        | _ -> printfn "%s: FAILED" title

    let printFeedProperties (channel: Rss.Channel) =
        printfn "Title: %s" channel.Title
        printfn "Link: %s" channel.Link
        printfn "Description: %s" channel.Description
        printfn "Image1: %s" channel.Image.Url
        printfn "PubDate: %s" channel.PubDate
        printfn "Categories: %s" (System.String.Join( ", ", channel.Categories))
        // Not always present...
        print "LastBuildDate" (fun() -> channel.LastBuildDate)
        print "Cloud" (fun() -> channel.Cloud.ToString())

        printfn "[iTunes Extensions]"
        printfn "|Subtitle: %s" channel.Subtitle
        printfn "|Summary: %s" channel.Summary
        printfn "|Image2: %s" channel.Image2.Href
        printfn "|Author: %s" channel.Author
        printfn "|Owner: %s" channel.Owner.Name


    let printItems (items: Rss.Item[]) =
        let printi title value =
            print ("-- " + title) value

        items
        |> Seq.iteri (fun index item ->
            printfn "Episode %i" (items.Length - index)
            printi "Title" (fun() -> item.Title)
            printi "Author" (fun() -> item.Author)
            printi "Duration" (fun() -> item.Duration)
            printi "Enclosure" (fun() -> item.Enclosure) // link to the file
            printi "Guid" (fun() -> item.Guid)
            printi "Link" (fun() -> item.Link) // show page, not the content
            printi "PubDate" (fun() -> item.PubDate)
            printi "Source" (fun() -> item.Source)
            printi "Keywords" (fun() -> item.Keywords)
            printi "Subtitle" (fun() -> item.Subtitle) // short ?
            printi "Description" (fun() -> item.Description) // longer ?
            printi "Summary" (fun() -> item.Summary) // longest?
            printfn ""
            )

    let DoAThing (source: string) =
        let feed = Rss.Load source
        printFeedProperties feed.Channel
        printItems feed.Channel.Items
        //|> Seq.iteri (fun index item ->
        //    printfn "%i - %s" index item.Title
        //    )

        printfn "*************************"
        printfn "hmm: %A" feed.Channel
