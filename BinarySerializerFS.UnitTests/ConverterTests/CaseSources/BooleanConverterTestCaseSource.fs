namespace BinarySerializerFS.UnitTests.ConverterTests.CaseSources

open BinarySerializerFS.Converters
open BinarySerializerFS.UnitTests.ConverterTests.Cases
open System.Collections

type public BooleanConverterTestCaseSource() = 
    class
        inherit BaseConverterTestCaseSource()
        override __.Key = "Bool"
        override __.ConverterType = typeof<BooleanConverter>
        
        override __.GetEnumerable() = 
            seq { 
                yield new UniversalConverterTestCase<bool, byte []>(true, [| 0xFFuy |], __.Key, __.ConverterType)
                yield new UniversalConverterTestCase<bool, byte []>(false, [| 0x00uy |], __.Key, __.ConverterType)
            } :> _
    end
