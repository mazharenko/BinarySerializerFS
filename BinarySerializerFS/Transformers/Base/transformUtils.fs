module internal transformUtils

let inline wrap<'from, '``to``> (convertF : 'from -> '``to``) (source : obj) = 
    match source with
    | null -> nullArg "source"
    | :? 'from as sourceTyped -> convertF sourceTyped :> obj
    | _ -> invalidArg "source" (sprintf "Object of type %O was expected" typeof<'from>)
