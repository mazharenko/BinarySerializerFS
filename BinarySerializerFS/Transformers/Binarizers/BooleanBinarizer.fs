namespace BinarySerializerFS.Binarizers

open BinarySerializerFS.Transformers.Base

type BooleanBinarizer() = 
    class
        inherit Binarizer<bool>()
        
        override __.WriteInternal source stream = 
            stream.WriteByte(match source with
                             | true -> 0xFF
                             | false -> 0x00
                             |> byte)
        
        override __.ReadInternal stream = 
            let onlyByte = __.ReadBytesInternal stream 1 |> Array.exactlyOne
            onlyByte > 0uy
    end
    