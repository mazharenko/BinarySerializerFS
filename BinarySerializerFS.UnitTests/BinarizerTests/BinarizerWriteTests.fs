module BinarySerializer.UnitTests.BinarizerTests.BinarizerWriteTests

open BinarySerializerFS.Transformers.Base
open BinarySerializerFS.UnitTests.BinarizerTests.CaseSources
open BinarySerializerFS.Transformers.Base.StreamAdapterFunctions
open FsUnit
open NUnit.Framework
open System
open System.IO

let TestBinarizerWrite (source : obj) (expected : byte []) (binarizer : IBinarizer) = 
    use stream = new MemoryStream()
    binarizer.Write source (writeBytesToStream stream)
    should equal expected (stream.ToArray())
    
[<Test>]
[<TestCaseSource(typeof<BooleanBinarizerTestCaseSource>)>]
[<TestCaseSource(typeof<StringBinarizerTestCaseSource>)>]
let TestBinarizersWrite (source : obj) (expected : byte []) (binarizerType : Type)=
    Activator.CreateInstance binarizerType :?> IBinarizer
    |> TestBinarizerWrite source expected