namespace Service.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting

open Service

[<TestClass>]
type TestClass () =
    let testRows = [
        [ 0; 1;  2;  3 ]
        [ 4; 5;  6;  7 ]
        [ 8; 9; 10; 11 ]
    ]

    let testMatrix =
        testRows |> Matrix.createFromRows

    let seqSeqToListList (xs: seq<#seq<'a>>) =
        xs |> Seq.map Seq.toList |> Seq.toList

    let testMatrixDiagonals direction expected =
        let actual =
            testMatrix
            |> Matrix.diagonals direction
            |> seqSeqToListList
            |> List.sort

        let expected = List.sort expected

        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.``Matrix.numRows is correct`` () =
        let expected = 3
        let actual = testMatrix |> Matrix.numRows
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.``Matrix.numColumns is correct`` () =
        let expected = 4
        let actual = testMatrix |> Matrix.numCols
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.``Matrix.rows is the same as the rows passed to Matrix.createFromRows`` () =
        let expected = testRows
        let actual = testMatrix |> Matrix.rows |> seqSeqToListList
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.``Matrix.columns is correct`` () =
        let expected = [
            [0; 4; 8]
            [1; 5; 9]
            [2; 6; 10]
            [3; 7; 11]
        ]

        let actual = testMatrix |> Matrix.cols |> seqSeqToListList

        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.``Matrix.diagonals TopLeftToBottomRight is correct`` () =
        let expected = [
            [ 0; 5; 10 ]
            [ 4; 9 ]
            [ 8 ]
            [ 1; 6; 11 ]
            [ 2; 7 ]
            [ 3 ]
        ]

        testMatrixDiagonals Matrix.TopLeftToBottomRight expected

    [<TestMethod>]
    member this.``Matrix.diagonals TopRightToBottomLeft is correct`` () =
        let expected = List.sort [
            [ 3; 6; 9 ]
            [ 7; 10 ]
            [ 11 ]
            [ 2; 5; 8]
            [ 1; 4]
            [ 0 ]
        ]

        testMatrixDiagonals Matrix.TopRightToBottomLeft expected
