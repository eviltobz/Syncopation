namespace Syncopation.Core

open FSharp.Data

module Feeds =
    open System

    type Rss = XmlProvider<"..\Syncopation.Core\SampleFeeds\MasterSample.xml">


    let print title value =
        let trim (orig: string) =
            let str = orig.Trim()
            match str.Length with
            | x when x <= 150 -> str
            | x -> str.Substring(0, 150) + "..."
        try
            let v = value()
            match (v :> obj) with
            | :? System.String -> printfn "%s: %s" title (trim (v.ToString()))
            | x when x.GetType().Name.Contains("FSharpOption") ->
                let optType = x.GetType().GetGenericArguments().[0]
                let args = [| x |]
                let isSome = x.GetType().GetMethod("get_IsSome").Invoke(x, args) :?> bool
                match isSome with
                | false -> printfn "%s: (Option<%s>.None) " title (optType.Name)
                | true ->
                    let value = x.GetType().GetMethod("get_Value").Invoke(x, null)
                    printfn "%s: (Option<%s>.Some) %s" title (optType.Name) (trim (sprintf "%A" value))
            | x -> printfn "%s: (%A) %s" title (x.GetType()) (trim (sprintf "%A" x))
        with
        | ex -> printfn "%s: FAILED" title //ex

    let printFeedProperties (channel: Rss.Channel) =
        printfn "Title: %s" channel.Title
        printfn "TTL: %s" (channel.Ttl.ToString())
        printfn "Link: %s" channel.Link
        printfn "Link2: %s" (channel.Link2.ToString())
        printfn "Description: %s" channel.Description
        printfn "Image1: %s" channel.Image.Url
        printfn "PubDate: %s" channel.PubDate
        printfn "Categories: %s" (System.String.Join(", ", channel.Categories))
        // Not always present...
        print "LastBuildDate" (fun() -> channel.LastBuildDate)
        print "Cloud" (fun() -> channel.Cloud)
        print "Generator" (fun() -> channel.Generator)

        printfn "[iTunes Extensions]"
        printfn "|Subtitle: %s" channel.Subtitle
        printfn "|Summary: %s" channel.Summary
        printfn "|Image2: %s" channel.Image2.Href
        printfn "|Author: %s" channel.Author
        printfn "|Owner: %s" channel.Owner.Name

    type ItunesOwner = {Name: string; Email: string}
    type AtomLink = {Href: string; Rel: string; Type: string;}
    type Image = {url: string; title: string; link: string; width:int; height:int; description: string}

    type Enclosure = { Url: string; Type: string; Length: int }
    type Source = { Url: string; Value: string }
    type EpisodeGuid = { Value: string; IsPermaLink: bool }
    type Duration = { DateTime: DateTime; String: string }
    type Episode = {
        Title: string
        DerivedEpisodeNumber: int
        Enclosure: Enclosure
        Guid: EpisodeGuid
        Link: string
        Comments: Option<string>
        Categories: string[]
        PubDate: DateTime
        Source: Source
        Description: string
        Content_Encoded: Option<string>
        Itunes_Author: string
        Itunes_Subtitle: string
        Itunes_Summary:string
        Itunes_Duration: Duration
        Itunes_Keywords: int
        Itunes_Explicit: bool
        Itunes_Block: Option<bool>
        Itunes_Episode: Option<int>
        Itunes_EpisodeType: Option<string>
        Itunes_Image: string
        }
    type Podcast = {
        Title: string
        Link: string
        Atom_Link: AtomLink
        Description: string
        Image: Image
        PubDate: DateTime
        Categories: string[]
        Generator: string
        Itunes_Subtitle: string
        Itunes_Summary: string
        Itunes_Author: string
        Itunes_Image: string
        Itunes_Owner: ItunesOwner
        Episodes: Episode[]
    }


    let printItems (items: Rss.Item[]) =
        let printi title value =
            print ("-- " + title) value

        items
        |> Seq.iteri (fun index item ->
            printfn "Episode %i" (items.Length - index)
            printi "Title" (fun() -> item.Title)
            printi "Enclosure" (fun() -> item.Enclosure) // link to the file
            printi "Guid" (fun() -> item.Guid)
            printi "Link" (fun() -> item.Link) // show page, not the content
            printi "Comments" (fun() -> item.Comments)
            printi "Categories" (fun() -> System.String.Join(", ", item.Categories))
            printi "PubDate" (fun() -> item.PubDate)
            printi "Source" (fun() -> item.Source)
            printi "Description" (fun() -> item.Description) // longer ?
            printfn "[content Extensions]"
            printi "|Encoded" (fun() -> item.Encoded)

            printfn "[iTunes Extensions]"
            printi "|Author" (fun() -> item.Author)
            printi "|Subtitle" (fun() -> item.Subtitle) // short ?
            printi "|Summary" (fun() -> item.Summary) // longest?
            printi "|Duration/DateTime" (fun() -> item.Duration.DateTime)
            printi "|Duration/String" (fun() -> item.Duration.String)
            printi "|Keywords" (fun() -> item.Keywords)
            printi "|Explicit" (fun() -> item.Explicit)
            printi "|Block" (fun() -> item.Block)
            printi "|Episode" (fun() -> item.Episode)
            printi "|EpisodeType" (fun() -> item.EpisodeType)
            printi "|Image" (fun() -> item.Image.Value.Href)
            printfn ""
            )

    let DoAThing (source: string) =
        let feed = Rss.Load source
        printFeedProperties feed.Channel
        printItems feed.Channel.Items

        printfn "*************************"
        printfn "hmm: %A" feed.Channel
