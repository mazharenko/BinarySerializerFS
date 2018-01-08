namespace BinarySerializerFS.UnitTests.BinarizerTests.CaseSources

open BinarySerializerFS.Binarizers
open BinarySerializerFS.UnitTests.BinarizerTests.Cases
open System
open System.Collections

[<AbstractClass>]
type BaseBinarizerTestCaseSource() = 
    class
        abstract Key : string
        abstract BinarizerType : Type
        abstract GetEnumerable : unit -> IEnumerable
        interface IEnumerable with
            member __.GetEnumerator() = __.GetEnumerable().GetEnumerator()
    end