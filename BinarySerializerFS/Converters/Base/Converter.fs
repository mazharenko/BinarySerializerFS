namespace BinarySerializerFS.Converters.Base

open BinarySerializerFS.Exceptions
open System
open System.IO

type public IConverterBase = 
    interface
        abstract Type : Type
    end

type public IConverter = 
    interface
        inherit IConverterBase
        abstract Write : obj -> Stream -> unit
        abstract Read : Stream -> obj option
    end

[<AbstractClass>]
type public Converter<'T>() = 
    class
        abstract WriteInternal : 'T -> Stream -> unit
        abstract ReadInternal : Stream -> 'T
        
        interface IConverter with
            member __.Type = typeof<'T>
            
            member __.Write source stream = 
                match source with
                | null -> nullArg "source"
                | :? 'T as sourceTyped -> __.WriteInternal sourceTyped stream
                | _ -> invalidArg "source" (sprintf "Object of type %O was expected" typeof<'T>)
            
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
