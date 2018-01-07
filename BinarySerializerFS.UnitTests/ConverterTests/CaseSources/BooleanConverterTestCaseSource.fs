namespace BinarySerializerFS.UnitTests.ConverterTests.CaseSources

open BinarySerializerFS.Converters
open BinarySerializerFS.UnitTests.ConverterTests.Cases
open System.Collections

type public BooleanConverterTestCaseSource() = 
    class
        let Key = "Bool"
        let ConverterType = typeof<BooleanConverter>
        
        interface IEnumerable with
            member __.GetEnumerator() = __.GetEnumerable().GetEnumerator() :> IEnumerator
        
        member private __.GetEnumerable() = 
            seq { 
                yield new UniversalConverterTestCase<bool, byte []>(true, [| 0xFFuy |], Key, ConverterType)
                yield new UniversalConverterTestCase<bool, byte []>(false, [| 0x00uy |], Key, ConverterType)
            }
    end
