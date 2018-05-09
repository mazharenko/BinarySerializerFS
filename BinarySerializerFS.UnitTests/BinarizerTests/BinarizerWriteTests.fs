module BinarySerializer.UnitTests.BinarizerTests.BinarizerWriteTests

open BinarySerializer.UnitTests.BinarizerTests.binarizerTestsUtils
open BinarySerializerFS.Transformers.Base
open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions.StreamAdapterFunctions
open BinarySerializerFS.UnitTests.BinarizerTests.CaseSources
open FsUnit
open NUnit.Framework
open System
open System.IO

let TestBinarizerWrite(binarizer : IBinarizer) = TestWrite binarizer.Write

[<Test>]
[<TestCaseSource(typeof<BooleanBinarizerTestCaseSource>)>]
[<TestCaseSource(typeof<StringBinarizerTestCaseSource>)>]
let TestBinarizersWrite (source : obj) (expected : byte []) (binarizerType : Type) = 
    (expected, source) ||> (Activator.CreateInstance binarizerType :?> IBinarizer |> TestBinarizerWrite)
