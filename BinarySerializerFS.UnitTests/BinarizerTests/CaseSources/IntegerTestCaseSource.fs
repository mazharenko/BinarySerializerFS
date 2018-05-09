namespace BinarySerializerFS.UnitTests.BinarizerTests.CaseSources

open BinarySerializerFS.Binarizers
open BinarySerializerFS.UnitTests.BinarizerTests.Cases
open System
open System.Collections

type public IntegerTestCaseSource() = 
    class
        let GetEnumerable() = 
            seq { 
                yield IntegerTestCase(1UL, false, [|0b1_0000001uy|])
                yield IntegerTestCase(9UL, false, [|0b1_0001001uy|])
                yield IntegerTestCase(127UL, false, [|0b1_1111111uy|])
                yield IntegerTestCase(128UL, false, [|0b0000_0001uy; 128uy|])
                yield IntegerTestCase(1000UL, false, [|0b0000_0010uy; 0x03uy; 0xE8uy|])
                yield IntegerTestCase(UInt64.MaxValue, false, 
                    [|
                        yield 0b0000_1000uy
                        yield! Array.replicate 8 0xFFuy
                    |])
                yield IntegerTestCase(1000UL, true, [|0b0001_0010uy; 0x03uy; 0xE8uy|])
                yield IntegerTestCase(421UL, true, [|0b0001_0010uy; 0x01uy; 0xA5uy|])
                yield IntegerTestCase(65538UL, true, [|0b0001_0011uy; 0x01uy; 0x00uy; 0x02uy|])
                yield IntegerTestCase(1UL, true, [|0b0001_0001uy; 1uy|])
                yield IntegerTestCase(2UL, true, [|0b0001_0001uy; 2uy|])
                yield IntegerTestCase(0UL, false, [|0b1000_0000uy|])
                yield IntegerTestCase(0UL, true, [|0b0001_0000uy|])
            } :> IEnumerable
        interface IEnumerable with 
            member __.GetEnumerator() =
                GetEnumerable().GetEnumerator();
    end
