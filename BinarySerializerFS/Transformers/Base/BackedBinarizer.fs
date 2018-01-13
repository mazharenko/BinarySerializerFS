namespace BinarySerializerFS.Transformers.Base

open System.IO

type BackedBinarizer(converter : IConverter, actualBinarizer : IBinarizer) = 
    class
        
        do 
            if not (actualBinarizer.Type = converter.Type) then invalidArg "" "Converter and binarizer types must match"
        
        interface IBinarizer with
            member __.Type = actualBinarizer.Type
            member __.Write (source : obj) (stream : Stream) = 
                actualBinarizer.Write (converter.ConvertTo source) stream
            member __.Read(stream : Stream) = 
                match actualBinarizer.Read stream with
                | Some value -> Some <| converter.ConvertFrom value
                | None -> None
    end
