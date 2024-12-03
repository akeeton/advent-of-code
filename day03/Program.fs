open System.IO
open System.Text.RegularExpressions

let mapTuple2 f (a, b) = (f a, f b)

let partOne (inputText: string): int =
    let pattern = @"mul\((\d+),(\d+)\)"
    seq {
        for m in Regex.Matches(inputText, pattern) do
            yield m.Groups[1].Value, m.Groups[2].Value
    }
    |> Seq.map (mapTuple2 int)
    |> Seq.map (fun (a, b) -> a * b)
    |> Seq.sum

let inputText = File.ReadAllText "input.txt"

printfn "Part one: %A" <| partOne inputText