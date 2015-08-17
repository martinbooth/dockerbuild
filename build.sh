#!/bin/bash
cd $(dirname `which $0`)

if [ ! -f "./packages/FAKE/tools/FAKE.exe" ]; then
  mono ".nuget/NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
fi

mono ./packages/FAKE/tools/FAKE.exe "$@"