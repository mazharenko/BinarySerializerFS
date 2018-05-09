namespace BinarySerializerFS.Transformers.Base

open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions
open System.IO

type BackedBinarizer(converter : IConverter, actualBinarizer : IBinarizer) = 
    class
        
        do 
            if not (actualBinarizer.Type = converter.Type) then invalidArg "" "Converter and binarizer types must match"
        
        interface IBinarizer with
            member __.Type = actualBinarizer.Type
            
            member __.Write (source : obj) (writeAdapter : writeBytesAdapter) = 
                let writeToStream = actualBinarizer.Write >< writeAdapter
                source |> (converter.ConvertTo >> writeToStream)
            
            member __.Read(readAdapter : readBytesAdapter) = 
                match actualBinarizer.Read readAdapter with
                | Some value -> Some <| converter.ConvertFrom value
                | None -> None
    end
