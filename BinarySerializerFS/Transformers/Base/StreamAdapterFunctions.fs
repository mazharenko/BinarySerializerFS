namespace BinarySerializerFS.Transformers.Base.BytesAdapterFunctions

type writeBytesAdapter = byte [] -> unit

type readBytesAdapter = int -> byte []

module StreamAdapterFunctions = 
    open BinarySerializerFS.Exceptions
    open System.IO
    
    let writeBytesToStream (stream : Stream) bytes = stream.Write(bytes, 0, bytes.Length)
    
    // TODO: railway oriented ?
    let private processStreamBytesReadCount bytesRead bytesExpected = 
        match (bytesRead, bytesExpected) with
        | (0, 0) -> ()
        | (0, _) -> raise <| StreamEndReachedException
        // TODO: rec?? Реализация может вернуть меньше байтов, чем было запрошено, даже если не достигнут конец потока.
        | _ when bytesRead <> bytesExpected -> raise <| UnexpectedStreamEndException()
        | _ -> ()
    
    let readFromStream (stream : Stream) (count : int) = 
        let buffer = Array.zeroCreate count
        let bytesRead = stream.Read(buffer, 0, count)
        processStreamBytesReadCount bytesRead count
        buffer
