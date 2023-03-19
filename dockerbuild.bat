echo off 
set version=%1

echo Build docker image bom.api version: %version%

docker image build -t bom.api:%version% .

echo Download docker image bom.api version: %version%

docker save bom.api -o ./bom.api-%version%.tar

echo Successfully done.

ехіt