### SQL Server 에 Snappy.Net 어셈블리 등록 활용해보는 예제.

### Snappy.Net
Nuget 에서 받는 Dll 은 서명이 되어 있지 않기 때문에 프로젝트 소스를 직접 받아서 빌드 한다.  
우선 Snappy.Net 은 CRC32C 에 의존성을 갖기 때문에 CRC32C 부터 빌드 한다.  
프로젝트 홈페이지는 https://crc32c.machinezoo.com/  다.  
이것도 마찬가지로 빌드에 필요한 natvie dll 을 다음 부분에서 다운받을 수 있다.  
```
Alternatively, you can download plain 7-Zip archive of the DLLs. 
Sources are available via Bitbucket repository. 
.NET code is distributed under BSD license. Underlying C++ code is distributed under zlib license.
```

다음 Snappy.Net 는 https://github.com/robertvazan/snappy.net.git  여기서 소스를 받으면 되고  
프로젝트 홈페이지는 https://snappy.machinezoo.com/ 인데 여기서 빌드에 필요한 native dll 을 다운받을 수 있다.  
```
Alternatively, you can download plain 7-Zip archive of the DLLs. 
Sources are available via Bitbucket repository. 
.NET code and underlying native libraries are distributed under BSD license.
```
라고 된 부분이 보일 것이다.  
프로젝트 참조에는 직접 빌드한 CRC32C.Net.dll 을 사용하도록 수정하고 빌드 한다.  

### SQL Server with Snappy.Net


### SQL Server 에 적용
빌드된 SQLSnappy.dll, Snappy.NET.dll, Crc32C.NET.dll 을 SQL Server 에 복사 ('C:\MSSQL Dll)  
  
어셈블리 생성 및 함수 생성 쿼리
```
exec sp_configure 'show advanced options', 1
go
reconfigure
go
exec sp_configure 'clr enabled',1
go
reconfigure
go

alter database [snappyTest]
set trustworthy on
go

use [snappyTest]
go

create assembly [Crc32C.NET]
from 'C:\MSSQL DLL\Crc32C.NET.dll'
with permission_set = unsafe

create assembly [Snappy.NET]
from 'C:\MSSQL DLL\Snappy.NET.dll'
with permission_set = unsafe

create assembly [Decompress]
from 'C:\MSSQL DLL\SQLSnappy.dll'
with permission_set = unsafe

create function [fnDecompress](@value varbinary(max))
returns nvarchar(max)
as
external name SQLSnappy.Snappy.Decompress
go
```
