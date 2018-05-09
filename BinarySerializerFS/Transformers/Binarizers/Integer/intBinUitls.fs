module internal BinarySerializerFS.Binarizers.Integer.intBinUitls

open BinarySerializerFS.Transformers.Base.BytesAdapterFunctions
open System.IO

let rec private numberToBytes number = 
    seq { 
        let smaller = number &&& 0xFFUL |> byte
        let shifted = number >>> 8
        match shifted with
        | 0UL -> ()
        | _ -> yield! numberToBytes shifted
        yield smaller
    }

let private bytesToNumber = 
    let accumulate acc byte = acc <<< 8 ||| (uint64 byte)
    Array.fold accumulate 0UL

// if non-negative and 7 bits are enough
// 1
// 7 bits - value
//
// ELSE
//
// 0001 if negative, 0000 if non-negative
// 4 bits - size, how many bytes are required to store the value
// [size] bytes - value, exact if non-negative, |x| if negative
type private ServiceStructureBlock = 
    | SimpleComplete of number : uint64
    | ComplexAnnotation of negative : bool * length : int

type private StructureBlock = 
    | Service of ServiceStructureBlock
    | ComplexValue of bytes : byte []

let private GetStructure (source : uint64) negative = 
    seq { 
        if not negative && source >>> 7 = 0UL then yield Service(SimpleComplete source)
        else 
            let numberOwnBytes = 
                source
                |> numberToBytes
                |> Seq.toArray
            
            let length = numberOwnBytes.Length
            yield Service(ComplexAnnotation(negative, length))
            yield ComplexValue numberOwnBytes
    }

let private simpleBinMarker = 0b1000_0000uy
let private simpleBinValueMask = ~~~simpleBinMarker
let private annotationNegativeBinMarker = 0b0001_0000uy
let private annotationLengthMask = 0b0000_1111uy

let private bin block = 
    match block with
    | Service(SimpleComplete number) -> [| simpleBinMarker + (byte number &&& simpleBinValueMask) |]
    | Service(ComplexAnnotation(negative, length)) -> 
        [| (if negative then annotationNegativeBinMarker
            else 0uy) + (byte length &&& annotationLengthMask) |]
    | ComplexValue bytes -> bytes

let private (|ServiceStructureBlock|) firstByte = 
    match firstByte &&& simpleBinMarker with
    | 0uy -> 
        firstByte &&& simpleBinValueMask
        |> uint64
        |> SimpleComplete
    | _ -> 
        ComplexAnnotation(firstByte &&& annotationNegativeBinMarker <> 0uy, firstByte &&& annotationLengthMask |> int)

let GetBytesForInteger = GetStructure >>+ ((Seq.collect bin) >> Seq.toArray)

let WriteInteger (writeAdapter : writeBytesAdapter) (source : uint64) negative = 
    let bytes = GetBytesForInteger source negative
    writeAdapter bytes

let ReadInteger(readAdapter : readBytesAdapter) = 
    let firstByte = readAdapter 1 |> Array.exactlyOne
    match firstByte with
    | ServiceStructureBlock block -> 
        match block with
        | SimpleComplete number -> (number, false)
        | ComplexAnnotation(negative, length) -> 
            (length
             |> readAdapter
             |> bytesToNumber, negative)
