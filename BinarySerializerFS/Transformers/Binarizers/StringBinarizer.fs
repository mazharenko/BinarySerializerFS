namespace BinarySerializerFS.Binarizers

open BinarySerializerFS.Transformers.Base
open System
open System.Text

type StringBinarizer() = 
    class
        inherit Binarizer<string>()
        
        override __.WriteInternal source writeAdapter = 
            source
            |> Seq.takeWhile (fun c -> c <> char 0)
            |> Seq.toArray
            |> String
            |> Encoding.UTF8.GetBytes
            |> writeAdapter
            0x00uy
            |> Array.singleton
            |> writeAdapter
        
        override __.ReadInternal readAdapter = 
            Seq.initInfinite (fun _ -> readAdapter 1 |> Seq.exactlyOne)
            |> Seq.takeWhile (fun c -> c <> 0uy)
            |> Seq.toArray
            |> Encoding.UTF8.GetString
    end
