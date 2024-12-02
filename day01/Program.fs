open System.IO

let partOne (sortedLeftNums: int list) (sortedRightNums: int list): int =
    let folder (distance: int) (left: int) (right: int): int =
        abs (left - right) + distance

    List.fold2 folder 0 sortedLeftNums sortedRightNums

let partTwo (sortedLeftNums: int list) (sortedRightNums: int list): int =
    let rightNumsCounts =
        sortedRightNums |> List.countBy id |> Map

    let calculateSimilarity (leftNum: int): int =
        match Map.tryFind leftNum rightNumsCounts with
        | Some rightNumCount -> leftNum * rightNumCount
        | None -> 0

    sortedLeftNums
    |> List.map calculateSimilarity
    |> List.sum

let splitInputLineNums (s: string): int * int =
    let numbers = s.Split "   " |> Array.map int
    numbers.[0], numbers.[1]

let mapTuple2 f (a, b) = (f a, f b)

let sortedLeftNums, sortedRightNums =
    File.ReadLines "input.txt"
    |> Seq.map splitInputLineNums
    |> Seq.toList
    |> List.unzip
    |> mapTuple2 List.sort

printfn "Part one answer: %A" <| partOne sortedLeftNums sortedRightNums
printfn "Part two answer: %A" <| partTwo sortedLeftNums sortedRightNums
