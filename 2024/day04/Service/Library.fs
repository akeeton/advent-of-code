namespace Service

type Matrix<'T> = 'T[,]
module Matrix =
    let fromListOfRows (rows: 'T seq seq): Matrix<'T> =
        array2D rows

    let rows (matrix: Matrix<'T>): 'T seq =
        failwith "unfinished"