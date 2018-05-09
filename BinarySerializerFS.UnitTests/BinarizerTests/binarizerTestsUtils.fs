module BinarySerializer.UnitTests.BinarizerTests.binarizerTestsUtils

open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions
open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions.StreamAdapterFunctions
open FsUnit
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

// TODO: change the parameters' order
let TestWrite (write : writeBytesAdapter -> obj -> unit) (source : obj) (expected : byte []) = 
    use stream = new MemoryStream()
    write (writeBytesToStream stream) source
    should equal expected (stream.ToArray())

