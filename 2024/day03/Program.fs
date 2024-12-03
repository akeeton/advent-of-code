open System.IO
open System.Text.RegularExpressions

type Token =
    | Mul of int * int
    | Do
    | Dont

let tokenizeInput (inputText: string): Token seq =
    let pattern = @"(mul|do|don't)\((?:(\d+),(\d+))?\)"
    seq {
        for m in Regex.Matches(inputText, pattern) do
            yield m
    }
    |> Seq.map (fun m ->
        match m.Groups[1].Value with
        | "mul" ->
            (m.Groups[2].Value |> int, m.Groups[3].Value |> int)
            |> Mul
        | "do" -> Do
        | "don't" -> Dont
        | _ -> failwith "unexpected group"
    )

let partOne (tokens: Token seq): int =
    tokens
    |> Seq.map (function
        | Mul (a, b) -> a * b
        | _ -> 0
    )
    |> Seq.sum

let partTwo (tokens: Token seq): int =
    tokens
    |> Seq.fold (fun (sum, doMuls) token ->
        match token with
        | Do -> (sum, true)
        | Dont -> (sum, false)
        | Mul (a, b) ->
            let newSum =
                match doMuls with
                | true -> a * b + sum
                | false -> sum
            (newSum, doMuls)
    ) (0, true)
    |> fst

let tokens =
    File.ReadAllText "input.txt"
    |> tokenizeInput

printfn "Part one: %d" <| partOne tokens
printfn "Part two: %d" <| partTwo tokens