open System.IO

type FindDirection =
    | LeftToRight
    | RightToLeft

type FoundDigit = {
    Index: int
    Value: int
}

module FoundDigit =
    let chooseOutermostIndexed
        (direction: FindDirection)
        (foundDigitA: FoundDigit)
        (foundDigitB: FoundDigit)
        : FoundDigit =
        match direction with
        | LeftToRight ->
            if foundDigitA.Index <= foundDigitB.Index
            then foundDigitA
            else foundDigitB
        | RightToLeft ->
            if foundDigitA.Index <= foundDigitB.Index
            then foundDigitB
            else foundDigitA

let findNumeralDigit (direction: FindDirection) (line: string): FoundDigit option =
    let seqTryFind =
        match direction with
        | LeftToRight -> Seq.tryFind
        | RightToLeft -> Seq.tryFindBack

    line
    |> Seq.indexed
    |> seqTryFind (fun (_, c) -> System.Char.IsAsciiDigit c)
    |> Option.map (fun (i, c) -> {
        FoundDigit.Index = i
        FoundDigit.Value = c |> string |> int
    })

let findWordDigit (direction: FindDirection) (line: string): FoundDigit option =
    let seqSortBy =
        match direction with
        | LeftToRight -> Seq.sortBy
        | RightToLeft -> Seq.sortByDescending

    let lineIndexOf (word: string): int option =
        let index =
            match direction with
            | LeftToRight -> line.IndexOf word
            | RightToLeft -> line.LastIndexOf word

        match index with
        | -1 -> None
        | index -> Some index

    let digitWordsToValues = Map([
        ("one", 1)
        ("two", 2)
        ("three", 3)
        ("four", 4)
        ("five", 5)
        ("six", 6)
        ("seven", 7)
        ("eight", 8)
        ("nine", 9)
    ])

    digitWordsToValues
    // Use just the digit words
    |> Map.keys
    // Index the digit words in the line
    |> Seq.map (fun dw -> (lineIndexOf dw, dw))
    // Only keep the digit words that were found
    |> Seq.choose (function
        | Some i, dw -> Some (i, dw)
        | _ -> None
    )
    // Sort found digit words by index (asc/desc)
    |> seqSortBy fst
    // Grab the first/last (index, digit word)
    |> Seq.tryHead
    // Convert the digit word to its value
    |> Option.bind (fun (i, dw) ->
        Map.tryFind dw digitWordsToValues
        |> Option.map (fun value -> {
            FoundDigit.Index = i
            FoundDigit.Value = value
        })
    )

let findNumeralOrWordDigit (direction: FindDirection) (line: string): FoundDigit option =
    match (findNumeralDigit direction line, findWordDigit direction line) with
    | Some foundNumeralDigit, Some foundWordDigit ->
        FoundDigit.chooseOutermostIndexed direction foundNumeralDigit foundWordDigit
        |> Some
    | Some foundNumeralDigit, None ->
        Some foundNumeralDigit
    | None, Some foundWordDigit ->
        Some foundWordDigit
    | None, None ->
        None

type FindDigit = FindDirection -> string -> FoundDigit option

let getCalibrationValueOfLine (findDigit: FindDigit) (line: string): int option =
    Option.map2
        (fun firstDigit lastDigit -> 10 * firstDigit.Value + lastDigit.Value)
        (findDigit LeftToRight line)
        (findDigit RightToLeft line)

let getCalibrationValueofLines (findDigit: FindDigit) (lines: string seq): int =
    lines
    |> Seq.map (getCalibrationValueOfLine findDigit)
    |> Seq.map Option.get // Assume all digits were found
    |> Seq.sum

let partOne (lines: string seq): int =
    lines |> getCalibrationValueofLines findNumeralDigit

let partTwo (lines: string seq): int =
    lines |> getCalibrationValueofLines findNumeralOrWordDigit

let lines = File.ReadAllLines "input.txt"
printfn "Part One: %d" <| partOne lines
printfn "Part Two: %d" <| partTwo lines