open System.IO

let debugPrint message x =
    printfn "%s: %A" message x
    x

let getFirstDigitFromLeft (line: char seq): int =
    line
    |> Seq.find System.Char.IsAsciiDigit
    |> string
    |> int

let getCalibrationValue (line: string): int =
    let leftDigit = line |> getFirstDigitFromLeft
    let rightDigit = line |> Seq.rev |> getFirstDigitFromLeft
    10 * leftDigit + rightDigit

let partOne (lines: string seq): int =
    lines
    |> Seq.map getCalibrationValue
    |> Seq.sum

let partTwo (lines: string seq): int =
    failwith "unfinished"

let lines = File.ReadAllLines "input.txt"

printfn "Part One: %d" <| partOne lines
printfn "Part Two: %d" <| partTwo lines