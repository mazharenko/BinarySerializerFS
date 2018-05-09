namespace BinarySerializerFS.Binarizers

open BinarySerializerFS.Transformers.Base

type BooleanBinarizer() = 
    class
        inherit Binarizer<bool>()
        
        override __.WriteInternal writeAdapter source = 
            match source with
            | true -> 0xFFuy
            | false -> 0x00uy
            |> Array.singleton
            |> writeAdapter
        
        override __.ReadInternal readAdapter = 
            let onlyByte = readAdapter 1 |> Array.exactlyOne
            onlyByte > 0uy
    end
