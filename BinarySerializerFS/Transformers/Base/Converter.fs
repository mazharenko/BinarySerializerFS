namespace BinarySerializerFS.Transformers.Base

open System
open transformUtils

type public IConverter = 
    interface
        inherit ITransformer
        abstract TargetType : Type
        abstract ConvertTo : obj -> obj
        abstract ConvertFrom : obj -> obj
    end

[<AbstractClass>]
type public Converter<'T, 'TTarget> = 
    class
        abstract ConvertToInternal : 'T -> 'TTarget
        abstract ConvertFromInternal : 'TTarget -> 'T
        interface IConverter with
            member __.Type = typeof<'T>
            member __.TargetType = typeof<'TTarget>
            member __.ConvertTo source = source |> wrap __.ConvertToInternal
            member __.ConvertFrom source = source |> wrap __.ConvertFromInternal
    end
