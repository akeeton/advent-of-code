#!/bin/bash

# Fail on any error
set -e

if [[ $# -lt 2 ]] ; then
    echo "usage: $0 <year> <day>"
    exit 1
fi

YEAR=$1
DAY=$2

mkdir -p "$YEAR/$DAY"

SOLUTION_PATH=$(realpath "$YEAR/$DAY")

pushd "$SOLUTION_PATH"
    dotnet new sln

    SERVICE_NAME=Service
    mkdir "$SERVICE_NAME"
    pushd "$SERVICE_NAME"
        dotnet new classlib -lang "F#"
    popd # "$SERVICE_NAME"
    dotnet sln add "$SERVICE_NAME/$SERVICE_NAME.fsproj"

    TESTS_NAME="$SERVICE_NAME.Tests"
    mkdir Service.Tests
    pushd "$TESTS_NAME"
        dotnet new mstest -lang "F#"
        dotnet add reference "../$SERVICE_NAME/$SERVICE_NAME.fsproj"
    popd # "$TESTS_NAME"
    dotnet sln add "$TESTS_NAME/$TESTS_NAME.fsproj"

    dotnet new console -lang "F#"
    dotnet add reference "$SERVICE_NAME/$SERVICE_NAME.fsproj"
    dotnet sln add "$DAY.fsproj"

    dotnet build
    dotnet run
    dotnet test
popd # "$SOLUTION_PATH"
