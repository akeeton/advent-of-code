open System.IO

module Tuple2 =
    let map f (a, b) = (f a, f b)

let splitIntoNumPair (s: string) =
    let numbers = s.Split "   " |> Array.map int
    numbers.[0], numbers.[1]

File.ReadLines "input.txt"
|> Seq.map splitIntoNumPair
|> Seq.toList
|> List.unzip
|> Tuple2.map List.sort
|> (fun (left, right) -> List.zip left right)
|> List.fold (fun state (a, b) -> state + abs (a - b)) 0
|> printfn "answer: %d"
