SonarScanner.MSBuild.exe begin /k:"CATC" /d:sonar.login="squ_599c11cf0edfcd1ab3111cced5caab1e1b9bbf79" /d:sonar.host.url="https://sonarqube.bit2bitamericas.com"

"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild" Proyecto.sln

SonarScanner.MSBuild.exe end /d:sonar.login="squ_599c11cf0edfcd1ab3111cced5caab1e1b9bbf79"

