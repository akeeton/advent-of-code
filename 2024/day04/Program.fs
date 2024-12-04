open System
open System.IO

open Service

module Program =
    let substringCount (str: string) (substr: string): int =
        let strMinusSubstrs = str.Replace(substr, "")
        (str.Length - strMinusSubstrs.Length) / substr.Length

    let parseInput filename =
        filename
        |> File.ReadAllLines

    let partOne filename =
        let matrix =
            filename
            |> parseInput
            |> Matrix.createFromRows

        let rows = matrix |> Matrix.rows
        let columns = matrix |> Matrix.cols
        let topLeftToBottomRightDiags =
            matrix |> Matrix.diagonals Matrix.TopLeftToBottomRight
        let topRightToBottomLeftDiags =
            matrix |> Matrix.diagonals Matrix.TopRightToBottomLeft

        let count (cs: char seq seq) =
            cs
            |> Seq.map String.Concat
            |> Seq.map (fun s ->
                substringCount s "XMAS"
                + substringCount s "SAMX"
            )
            |> Seq.sum

        let allDirectionsStrings =
            rows
            |> Seq.append columns
            |> Seq.append topLeftToBottomRightDiags
            |> Seq.append topRightToBottomLeftDiags
            |> Seq.map String.Concat

        let rowsCount = rows |> count
        let columnsCount = columns |> count
        let topLeftToBottomRightDiagsCount =
            topLeftToBottomRightDiags
            |> count
        let topRightToBottomLeftDiagsCount =
            topRightToBottomLeftDiags
            |> count

        rowsCount
        + columnsCount
        + topLeftToBottomRightDiagsCount
        + topRightToBottomLeftDiagsCount

    let [<EntryPoint>] main _ =
        printfn "Part One (small): %d" <| partOne "input-small-part1.txt"
        printfn "Part One: %d" <| partOne "input.txt"
        0