namespace BinarySerializerFS.Transformers.Converters

open BinarySerializerFS.Transformers.Base
open System

type TimeSpanConverter() = 
    class
        inherit ConverterBase<TimeSpan, int64>()
        override __.ConvertToInternal(source : TimeSpan) = source.Ticks
        override __.ConvertFromInternal(source : int64) = TimeSpan source
    end
