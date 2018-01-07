namespace BinarySerializerFS.UnitTests.ConverterTests.Cases

open NUnit.Framework
open System

type public UniversalConverterTestCase<'T, 'TSub>(object : 'T, sub : 'TSub, key : string, converterType : Type) = 
    class
        inherit TestCaseData(object, sub, converterType)
        do base.TestName <- sprintf "Test%OConverter(%O)" key object
        member public __.Object = object
        member public __.SubObject = sub
    end
