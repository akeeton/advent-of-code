open System.IO

type Direction =
    | Increasing
    | Decreasing

let isReportSafe (direction: Direction) (report: int list): bool =
    report
    |> List.map Some
    |> List.reduce (fun a b ->
        match a, b with
        | Some a, Some b ->
            let diff =
                match direction with
                | Increasing -> b - a
                | Decreasing -> a - b

            if diff >= 1 && diff <= 3
            then Some b
            else None
        | _, _ -> None
    )
    |> Option.isSome

let calcNumSafeReports (direction: Direction) (reports: int list list): int =
    reports
    |> List.map (isReportSafe direction)
    |> List.filter id
    |> List.length

let partOne (reports: int list list): int =
    calcNumSafeReports Increasing reports + calcNumSafeReports Decreasing reports

let reports =
    File.ReadLines "input.txt"
    |> List.ofSeq
    |> List.map (fun (line: string) ->
        line.Split ' '
        |> List.ofArray
        |> List.map int
    )

printfn "Answer for part one: %A" <| partOne reports