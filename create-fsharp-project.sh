#!/bin/bash
if [[ $# -eq 0 ]] ; then
    echo 'usage: create-fsharp-project.sh <project-name>'
    exit 1
fi

dotnet new console -lang "F#" -o $1