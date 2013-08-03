echo off
cls
set path="C:\Program Files\WinRAR"

cd bin\Release

del nugardt-sc2ranks-customdivisionprofiler-*.zip
winrar a -m5 -ag"-YYYY.MM.DD" -z..\..\Comment.txt nugardt-sc2ranks-customdivisionprofiler.zip @..\..\List.txt
