namespace BinarySerializerFS.UnitTests.BinarizerTests.CaseSources

open BinarySerializerFS.Binarizers
open BinarySerializerFS.UnitTests.BinarizerTests.Cases
open System.Collections

type public BooleanBinarizerTestCaseSource() = 
    class
        inherit BaseBinarizerTestCaseSource()
        override __.Key = "Bool"
        override __.BinarizerType = typeof<BooleanBinarizer>
        
        override __.GetEnumerable() = 
            seq { 
                yield new UniversalBinarizerTestCase<bool, byte []>(true, [| 0xFFuy |], __.Key, __.BinarizerType)
                yield new UniversalBinarizerTestCase<bool, byte []>(false, [| 0x00uy |], __.Key, __.BinarizerType)
            } :> _
    end
