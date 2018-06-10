namespace BinarySerializer.UnitTests.BinarizerTests.IntUtilsTests

open BinarySerializer.UnitTests.BinarizerTests.binarizerTestsUtils
open BinarySerializerFS.Binarizers.Integer.intBinUitls
open BinarySerializerFS.Transformers.Base
open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions.StreamAdapterFunctions
open BinarySerializerFS.UnitTests.BinarizerTests.CaseSources
open FsUnit
open NUnit.Framework
open System
open System.IO
open ParserLibrary.Bin

module IntegerWriteTests = 
    let TestIntWrite number negative expectedBytes = 
        let unifiedWrite adapter (source : obj) = source :?> uint64 * bool ||> WriteInteger adapter
        TestWrite unifiedWrite expectedBytes (number, negative)
    
    [<Test>]
    [<TestCaseSource(typeof<IntegerTestCaseSource>)>]
    let TestWrite (number : uint64) (negative : bool) (expected : byte []) = TestIntWrite number negative expected

module IntegerReadTests = 
    let TestIntRead number negative bytes = 
        TestParser ReadParser (negative, number) bytes
    
    [<Test>]
    [<TestCaseSource(typeof<IntegerTestCaseSource>)>]
    let TestRead (number : uint64) (negative : bool) (source : byte []) = TestIntRead number negative source
