namespace BinarySerializerFS.UnitTests.BinarizerTests.Cases

open NUnit.Framework
open System

type public UniversalBinarizerTestCase<'T, 'TSub>(object : 'T, sub : 'TSub, key : string, binarizerType : Type) = 
    class
        inherit TestCaseData(object, sub, binarizerType)
        do base.TestName <- sprintf "Test%OBinarizer(%O)" key object
        member public __.Object = object
        member public __.SubObject = sub
    end
