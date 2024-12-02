open System.IO

type Direction =
    | Increasing
    | Decreasing

let isReportSafeWhenNotDampened (direction: Direction) (report: int list): bool =
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

let calcNumSafeReports isReportSafe (reports: int list list): int =
    reports
    |> List.map isReportSafe
    |> List.filter id
    |> List.length

let partOne (reports: int list list): int =
    calcNumSafeReports (isReportSafeWhenNotDampened Increasing) reports
    + calcNumSafeReports (isReportSafeWhenNotDampened Decreasing) reports

let makeDampenedReports (report: int list): int list list =
    report
    |> List.indexed
    |> List.map fst
    |> List.map (fun i ->
        List.removeAt i report
    )

let isReportSafeWhenDampened (direction: Direction) (report: int list): bool =
    report
    |> makeDampenedReports
    |> List.map (isReportSafeWhenNotDampened direction)
    |> List.contains true

let partTwo (reports: int list list): int =
    calcNumSafeReports (isReportSafeWhenDampened Increasing) reports
    + calcNumSafeReports (isReportSafeWhenDampened Decreasing) reports

let reports =
    File.ReadLines "input.txt"
    |> List.ofSeq
    |> List.map (fun (line: string) ->
        line.Split ' '
        |> List.ofArray
        |> List.map int
    )

printfn "Answer for part one: %A" <| partOne reports
printfn "Answer for part two: %A" <| partTwo reports