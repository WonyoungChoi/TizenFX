#!/bin/bash

SCRIPT_FILE=$(readlink -f $0)
SCRIPT_DIR=$(dirname $SCRIPT_FILE)

# APITool
APITOOL_GITDIR="$SCRIPT_DIR"/.APITool
APITOOL_BINDIR="$SCRIPT_DIR"/bin/APITool

rm -fr $APITOOL_GITDIR
rm -fr $APITOOL_BINDIR
git clone https://github.com/TizenAPI/APITool $APITOOL_GITDIR
dotnet publish $APITOOL_GITDIR -o $APITOOL_BINDIR

rm -fr $APITOOL_GITDIR

# StructValidator
SV_GITDIR="$SCRIPT_DIR"/StructValidator
SV_BINDIR="$SCRIPT_DIR"/bin/StructValidator

rm -fr $SV_BINDIR
dotnet publish $SV_GITDIR -o $SV_BINDIR

