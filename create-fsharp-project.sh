#!/bin/bash

# Fail on any error
set -e

if [[ $# -eq 0 ]] ; then
    echo 'usage: create-fsharp-project.sh <project-name>'
    exit 1
fi

export PROJECT_NAME=$1
PROJECT_PATH=$(realpath "$PROJECT_NAME")
SUBST_VARS="\$PROJECT_NAME:" # Separate var names with colons

dotnet new console -lang "F#" -o "$PROJECT_NAME"
mkdir -p "$PROJECT_PATH/.vscode"

pushd ".vscode-templates"
for f in ./*; do
    echo "$f"
    envsubst "$SUBST_VARS" < "$f" > "$PROJECT_PATH/.vscode/$f"
done
popd