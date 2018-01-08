module BinarySerializer.UnitTests.ConverterTests.ConverterWriteTests

open BinarySerializerFS.Converters.Base
open BinarySerializerFS.UnitTests.ConverterTests.CaseSources
open FsUnit
open NUnit.Framework
open System
open System.IO

let TestConverterWrite (source : obj) (expected : byte []) (converter : IConverter) = 
    use stream = new MemoryStream()
    converter.Write source stream
    should equal expected (stream.ToArray())
    
[<Test>]
[<TestCaseSource(typeof<BooleanConverterTestCaseSource>)>]
[<TestCaseSource(typeof<StringConverterTestCaseSource>)>]
let TestConvertersWrite (source : obj) (expected : byte []) (converterType : Type)=
    Activator.CreateInstance converterType :?> IConverter
    |> TestConverterWrite source expected