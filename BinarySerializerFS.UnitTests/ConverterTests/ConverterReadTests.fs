module BinarySerializer.UnitTests.ConverterTests.ConverterReadTests

open BinarySerializerFS.Converters.Base
open BinarySerializerFS.UnitTests.ConverterTests.CaseSources
open FsUnit
open NUnit.Framework
open System
open System.IO

let TestConverterRead (expected : obj) (source : byte []) (converter : IConverter) = 
    use stream = 
        (source, Array.zeroCreate 10)
        ||> Array.append
        |> MemoryStream
    stream
        |> converter.Read
        |> should equal (Some expected)
    stream.Position
        |> should equal source.Length

[<Test>]
[<TestCaseSource(typeof<BooleanConverterTestCaseSource>)>]
[<TestCaseSource(typeof<StringConverterTestCaseSource>)>]
let TestConvertersRead (expected : obj) (source : byte []) (``type`` : Type) = 
    let converter = Activator.CreateInstance ``type`` :?> IConverter
    TestConverterRead expected source converter
    