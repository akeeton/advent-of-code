namespace Service

type Matrix<'a> = private Matrix of 'a[,]

module Matrix =
    let private numRows' = Array2D.length1
    let private numCols' = Array2D.length2

    // #seq<'a> is a flexible type
    // See https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/flexible-types
    let createFromRows (rows: seq<#seq<'a>>): Matrix<'a> =
        rows |> array2D |> Matrix

    let numRows (Matrix m: Matrix<'a>): int =
        m |> numRows'

    let numCols (Matrix m: Matrix<'a>): int =
        m |> numCols'

    let rows (Matrix m: Matrix<'a>): 'a seq seq =
        Seq.init (numRows' m) (fun i -> m[i, *])

    let cols (Matrix m: Matrix<'a>): 'a seq seq =
        Seq.init (numCols' m) (fun j -> m[*, j])

    type DiagonalDirection =
        | TopLeftToBottomRight
        | TopRightToBottomLeft

    let diagonals (direction: DiagonalDirection) (Matrix m: Matrix<'a>): 'a seq seq =
        let outerColI =
            match direction with
            | TopLeftToBottomRight -> 0
            | TopRightToBottomLeft -> numCols' m - 1

        // Includes the top left/right corner
        let topRowCoordinates =
            Seq.init (numCols' m) (fun colI -> (0, colI))

        // Excludes the top left/right corner
        let outerColCoordinates =
            Seq.init (numRows' m - 1) (fun rowI -> (rowI + 1, outerColI))

        Seq.append topRowCoordinates outerColCoordinates
        |> Seq.map (fun (startRowI, startColI) ->
            // Collect the items along each diagonal, starting at the
            // coordinates on the top and left/right edges

            let numRowsInDiagonal = numRows' m - startRowI
            let numColsInDiagonal =
                match direction with
                | TopLeftToBottomRight -> numCols' m - startColI
                | TopRightToBottomLeft -> startColI + 1

            let numItemsInDiagonal = min numRowsInDiagonal numColsInDiagonal

            Seq.init numItemsInDiagonal (fun diagItemOffset ->
                let rowI = startRowI + diagItemOffset
                let colI =
                    match direction with
                    | TopLeftToBottomRight -> startColI + diagItemOffset
                    | TopRightToBottomLeft -> startColI - diagItemOffset

                m[rowI, colI]
            )
        )
