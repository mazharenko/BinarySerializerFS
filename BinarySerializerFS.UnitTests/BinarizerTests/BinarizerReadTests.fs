module BinarySerializer.UnitTests.BinarizerTests.BinarizerReadTests

open BinarySerializerFS.Transformers.Base
open BinarySerializerFS.UnitTests.BinarizerTests.CaseSources
open FsUnit
open NUnit.Framework
open System
open System.IO
open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions.StreamAdapterFunctions
open BinarySerializer.UnitTests.BinarizerTests.binarizerTestsUtils

let TestBinarizerRead (binarizer : IBinarizer) = TestRead binarizer.Read
    
[<Test>]
[<TestCaseSource(typeof<BooleanBinarizerTestCaseSource>)>]
[<TestCaseSource(typeof<StringBinarizerTestCaseSource>)>]
let TestBinarizersRead (expected : obj) (source : byte []) (``type`` : Type) = 
    let binarizer = Activator.CreateInstance ``type`` :?> IBinarizer
    TestBinarizerRead binarizer expected source
    