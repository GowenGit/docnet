#!/bin/bash

dotnet pack -c Release --version-suffix $1 -o ../../nuget ../src/Docnet.Core/