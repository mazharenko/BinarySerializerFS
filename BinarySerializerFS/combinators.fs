module Combinators

open ParserLibrary.Bin
open ParserLibrary.Bin.Parsers
open ParserLibrary.Generic
open System
open System.Collections.Generic

type BModel = 
    | BValue of obj * Type
    | BObject of (byte * BModel) []
    | BList of BModel []

let bvalue x = BValue(x, x.GetType())
let bValue, bValueRef = createParserForwardedToRef<BModel>()

let bbool = 
    let btrue = pbyte 0xFFuy .>>. pbyte 0x00uy >>% bvalue true
    let bfalse = pbyte 0x00uy .>>. pbyte 0xFFuy >>% bvalue false
    btrue <|> bfalse <//> "bool"

let bnumber = 
    read |>> (UnpackByte
              >> int
              >> bvalue)
    <//> "number"

let bmembero marker valueparser = 
    (pbyte marker) .>>. valueparser <//> sprintf "%i: %s" marker valueparser.label
let bobjectroot = pbyte 1uy <//> "object root"
let bobjectend = pbyte 0uy <//> "object end"
let bobjectsample (memberParsers : Parser<byte * BModel> list) = 
    (List.toArray >> BObject) <!> between bobjectroot (all memberParsers) bobjectend
let bObject, bObjectRef = createParserForwardedToRef<BModel>()

type BToken = 
    | BMemberMarkerToken of marker : byte
    | BValueToken of value : obj * Type
    | BObjectRootToken
    | BObjectEndToken

type BinaryModelToken(value : BToken) = 
    struct
        interface IParserToken
        member __.Value = value
        override __.ToString() = sprintf "<%A>" value
    end

type InputStateEmpty(position : IParserPosition) = 
    class
        interface IInputState with
            member __.Position = position
            member __.Next() = (None, __ :> _)
    end

type InputStateConstant(token : IParserToken, position : IParserPosition) = 
    class
        interface IInputState with
            member __.Position = position
            member __.Next() = (Some token, InputStateEmpty position :> _)
    end

type InputStateContinuation(state : IInputState, stateContinuation : IInputState) = 
    class
        interface IInputState with
            member __.Position = state.Position
            member __.Next() = 
                let (result, newInput) = state.Next()
                match result with
                | None -> stateContinuation.Next()
                | Some token -> (Some token, InputStateContinuation(newInput, stateContinuation) :> _)
    end

type UnknownPosition = 
    struct
        interface IParserPosition
    end

// TODO: laziness!!!
// TODO: position!
type BInputBuilder() = 
    class
        member __.Zero(x) = InputStateEmpty(new UnknownPosition()) :> IInputState
        member __.Yield(btoken) = 
            new InputStateConstant(btoken |> BinaryModelToken :> IParserToken, new UnknownPosition()) :> IInputState
        member __.YieldFrom(state : IInputState) = state
        member __.Combine(state1 : IInputState, state2 : IInputState) = 
            new InputStateContinuation(state1, state2) :> IInputState
        member __.Delay(f) = f()
        member __.For(models : (byte * BModel) [], f : byte * BModel -> IInputState) = 
            models
            |> Array.map f
            |> Array.reduce (fun s1 s2 -> __.Combine(s1, s2))
    end

let binput = new BInputBuilder()

type BinaryModelInputState(model : BModel) = 
    class
        let position = new UnknownPosition()
        interface IInputState with
            member __.Position = position :> _
            member __.Next() = 
                let rec generate m = 
                    binput { 
                        match m with
                        | BValue(value, ``type``) -> yield BValueToken(value, ``type``)
                        | BObject bValArray -> 
                            yield BObjectRootToken
                            for (marker, value) in bValArray do
                                yield BMemberMarkerToken marker
                                yield! generate value
                            yield BObjectEndToken
                        | _ -> raise (NotImplementedException())
                    }
                (generate model).Next()
    end
