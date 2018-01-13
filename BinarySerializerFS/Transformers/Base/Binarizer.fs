namespace BinarySerializerFS.Transformers.Base

open BinarySerializerFS.Exceptions
open System
open System.IO
open transformUtils

type public IBinarizer = 
    interface
        inherit ITransformer
        abstract Write : obj -> Stream -> unit
        abstract Read : Stream -> obj option
    end

[<AbstractClass>]
type public Binarizer<'T>() = 
    class
        abstract WriteInternal : 'T -> Stream -> unit
        abstract ReadInternal : Stream -> 'T
        
        interface IBinarizer with
            member __.Type = typeof<'T>
            
            member __.Write source stream = 
                let writeToStream = __.WriteInternal >< stream
                source
                |> wrap writeToStream
                |> ignore
            
            member __.Read stream = 
                try 
                    stream
                    |> __.ReadInternal
                    |> fun o -> o :> obj
                    |> Some
                with StreamEndReachedException -> None
        
        member __.ReadBytesInternal (stream : Stream) (count : int) = 
            let buffer = Array.zeroCreate count
            let bytesRead = stream.Read(buffer, 0, count)
            __.ProcessStreamBytesReadCount bytesRead count
            buffer
        
        member private __.ProcessStreamBytesReadCount bytesRead bytesExpected = 
            match (bytesRead, bytesExpected) with
            | (0, 0) -> ()
            | (0, _) -> raise <| StreamEndReachedException
            // TODO: rec?? Реализация может вернуть меньше байтов, чем было запрошено, даже если не достигнут конец потока.
            | _ when bytesRead <> bytesExpected -> raise <| UnexpectedStreamEndException()
            | _ -> ()
    end
