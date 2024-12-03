#!/bin/bash

# Fail on any error
set -e

if [[ $# -lt 2 ]] ; then
    echo 'usage: create-fsharp-project.sh <year> <project-name>'
    exit 1
fi

YEAR=$1
export PROJECT_NAME=$2

mkdir -p "$YEAR"

PROJECT_PATH=$(realpath "$YEAR/$PROJECT_NAME")
SUBST_VARS="\$PROJECT_NAME:" # Separate var names with colons


pushd "$YEAR"
    dotnet new console -lang "F#" -o "$PROJECT_NAME"
popd # "$YEAR"

mkdir -p "$PROJECT_PATH/.vscode"
pushd ".vscode-templates"
    for f in ./*; do
        echo "Substituting environment variables in '$f'"
        envsubst "$SUBST_VARS" < "$f" > "$PROJECT_PATH/.vscode/$f"
    done
popd # ".vscode-templates"

pushd "$PROJECT_PATH"
    for f in ./*; do
        if [ -f "$f" ]; then
            echo "Removing BOM from '$f'"
            dos2unix -r "$f"
        fi
    done
popd # "$PROJECT_PATH"