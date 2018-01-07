namespace BinarySerializerFS.UnitTests.ConverterTests.CaseSources

open BinarySerializerFS.Converters
open BinarySerializerFS.UnitTests.ConverterTests.Cases
open System
open System.Collections

[<AbstractClass>]
type BaseConverterTestCaseSource() = 
    class
        abstract Key : string
        abstract ConverterType : Type
        abstract GetEnumerable : unit -> IEnumerable
        interface IEnumerable with
            member __.GetEnumerator() = __.GetEnumerable().GetEnumerator()
    end