[<AutoOpen>]
module internal utils

let (><) f a b = f b a 

let (>>+) f g x y = g ( f x y )