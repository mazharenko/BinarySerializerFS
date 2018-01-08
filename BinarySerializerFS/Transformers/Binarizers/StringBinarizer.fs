namespace BinarySerializerFS.Binarizers

open BinarySerializerFS.Transformers.Base
open System
open System.Text

type StringBinarizer() = 
    class
        inherit Binarizer<string>()
        
        override __.WriteInternal source stream = 
            let sourceUntilZeroBytes = 
                source
                |> Seq.takeWhile (fun c -> c <> char 0)
                |> Seq.toArray
                |> String
                |> Encoding.UTF8.GetBytes
            stream.Write(sourceUntilZeroBytes, 0, sourceUntilZeroBytes.Length)
            stream.WriteByte 0x00uy
        
        override __.ReadInternal stream = 
            Seq.initInfinite (fun _ -> __.ReadBytesInternal stream 1 |> Seq.exactlyOne)
            |> Seq.takeWhile (fun c -> c <> 0uy)
            |> Seq.toArray
            |> Encoding.UTF8.GetString
    end
