module BinarySerializer.UnitTests.BinarizerTests.binarizerTestsUtils

open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions
open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions.StreamAdapterFunctions
open FsUnit
open ParserLibrary.Bin
open ParserLibrary.Generic
open System.IO

let TestRead (read : readBytesAdapter -> obj option) (expected : obj) (source : byte []) =
    use stream =
        (source, Array.zeroCreate 10)
        ||> Array.append
        |> MemoryStream
    readFromStream stream
    |> read
    |> should equal (Some expected)
    stream.Position |> should equal source.Length

let TestParser (parser : Parser<'a>) (expected : 'a) (source : byte []) =
    match run parser source with
    | Success(value, state) -> should equal expected value
    | Failure(_, error, _) -> failwithf "parsing failure %s" error

let TestWrite (write : writeBytesAdapter -> obj -> unit) (expected : byte []) (source : obj) =
    use stream = new MemoryStream()
    write (writeBytesToStream stream) source
    should equal expected (stream.ToArray())
