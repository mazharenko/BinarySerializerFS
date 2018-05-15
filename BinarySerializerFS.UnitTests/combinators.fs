module combinatorsTests

open Combinators
open FsUnit
open NUnit.Framework
open ParserLibrary.Bin
open ParserLibrary.Generic

[<Test>]
[<Explicit>]
let Test() = 
    let testmodel = 
        BObject([| 1uy, bvalue 7
                   2uy, bvalue true
                   3uy, 
                   BObject([| 2uy, bvalue 4
                              1uy, bvalue false |]) |])
    
    let innerbobject = 
        bobjectsample [ bmembero 1uy bbool
                        bmembero 2uy bnumber ]
    
    bObjectRef := bobjectsample [ bmembero 2uy bbool
                                  bmembero 3uy innerbobject
                                  bmembero 1uy bnumber ]
    bValueRef := choice [ bnumber; bbool; innerbobject ]
    let g = run (bObject) [| 1uy; 1uy; 7uy; 2uy; 255uy; 0uy; 3uy; 1uy; 2uy; 4uy; 1uy; 0uy; 255uy; 0uy; 0uy |]
    match g with
    | Failure(_, _, _) -> failwith "parsing failed"
    | Success(value, _) -> should equal testmodel value
    let state = new BinaryModelInputState(testmodel)
    
    let rec tokens (state : IInputState) = 
        seq { 
            let result = state.Next()
            match result with
            | (None, _) -> ()
            | (Some token, remainingState) -> 
                yield token
                yield! tokens remainingState
        }
    
    let hh = 
        tokens state
        |> Seq.map (fun x -> (x :?> BinaryModelToken).Value)
        |> Seq.toArray
    
    let expectedTokens = 
        [| BObjectRootToken
           BMemberMarkerToken 1uy
           BValueToken(7, (7).GetType())
           BMemberMarkerToken 2uy
           BValueToken(true, (true).GetType())
           BMemberMarkerToken 3uy
           BObjectRootToken
           BMemberMarkerToken 2uy
           BValueToken(4, (4).GetType())
           BMemberMarkerToken 1uy
           BValueToken(false, (false).GetType())
           BObjectEndToken
           BObjectEndToken |]
    
    should equal expectedTokens hh
