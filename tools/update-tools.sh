#!/bin/bash

SCRIPT_FILE=$(readlink -f $0)
SCRIPT_DIR=$(dirname $SCRIPT_FILE)

# APITool
APITOOL_GITDIR="$SCRIPT_DIR"/.APITool
APITOOL_BINDIR="$SCRIPT_DIR"/bin/APITool

rm -fr $APITOOL_GITDIR
git clone https://github.com/TizenAPI/APITool $APITOOL_GITDIR
dotnet publish $APITOOL_GITDIR -o $APITOOL_BINDIR

rm -fr $APITOOL_GITDIR

# StructValidator
dotnet publish "$SCRIPT_DIR"/StructValidator -o bin/StructValidator

