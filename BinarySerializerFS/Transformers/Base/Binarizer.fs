namespace BinarySerializerFS.Transformers.Base

open BinarySerializerFS.Exceptions
open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions
open System
open System.IO
open transformUtils

type public IBinarizer = 
    interface
        inherit ITransformer
        abstract Write : writeBytesAdapter ->  obj -> unit
        abstract Read : readBytesAdapter -> obj option
    end

[<AbstractClass>]
type public Binarizer<'T>() = 
    class
        abstract WriteInternal : writeBytesAdapter -> 'T -> unit
        abstract ReadInternal : readBytesAdapter -> 'T
        interface IBinarizer with
            member __.Type = typeof<'T>
            
            member __.Write writeBytesAdapter source = 
                let writeToStream = __.WriteInternal writeBytesAdapter
                source
                |> wrap writeToStream
                |> ignore
            
            member __.Read readBytesAdapter = 
                try 
                    readBytesAdapter
                    |> __.ReadInternal
                    |> fun o -> o :> obj
                    |> Some
                with StreamEndReachedException -> None
    end
