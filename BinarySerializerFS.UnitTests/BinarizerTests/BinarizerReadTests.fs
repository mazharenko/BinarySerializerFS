module BinarySerializer.UnitTests.BinarizerTests.BinarizerReadTests

open BinarySerializerFS.Transformers.Base
open BinarySerializerFS.UnitTests.BinarizerTests.CaseSources
open FsUnit
open NUnit.Framework
open System
open System.IO
open BinarySerializerFS.Transformers.Base.StreamAdapterFunctions

let TestBinarizerRead (expected : obj) (source : byte []) (binarizer : IBinarizer) = 
    use stream = 
        (source, Array.zeroCreate 10)
        ||> Array.append
        |> MemoryStream
    readFromStream stream
        |> binarizer.Read
        |> should equal (Some expected)
    stream.Position
        |> should equal source.Length

[<Test>]
[<TestCaseSource(typeof<BooleanBinarizerTestCaseSource>)>]
[<TestCaseSource(typeof<StringBinarizerTestCaseSource>)>]
let TestBinarizersRead (expected : obj) (source : byte []) (``type`` : Type) = 
    let binarizer = Activator.CreateInstance ``type`` :?> IBinarizer
    TestBinarizerRead expected source binarizer
    