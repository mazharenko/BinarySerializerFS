namespace BinarySerializerFS.UnitTests.BinarizerTests.Cases

open NUnit.Framework
open System

type public IntegerTestCase(number: uint64, negative: bool, bytes: byte[]) = 
    class
        inherit TestCaseData(number, negative, bytes)
        do base.TestName <- sprintf "TestInteger(%O)" (number,negative)
    end
