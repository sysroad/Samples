### SQL Server 에 Snappy.Net 어셈블리 등록 활용해보는 예제.

#### Snappy.Net
Nuget 에서 받게 되면 서명이 되어 있지 않기 때문에 프로젝트 소스를 직접 받아서 빌드 한다.  
Snappy.Net : https://github.com/robertvazan/snappy.net.git  
프로젝트 홈페이지는 https://snappy.machinezoo.com/ 인데 여기서 빌드에 필요한 native dll 을 다운받을 수 있다.  
```
Alternatively, you can download plain 7-Zip archive of the DLLs. 
Sources are available via Bitbucket repository. 
.NET code and underlying native libraries are distributed under BSD license.
```
라고 된 부분이 보일 것이다.  
Snappy.Net 은 CRC-32C 에 의존성을 갖는데 (동일 제작자가 만든 라이브러리다) 마찬가지로 소스를 받아서 직접 빌드 한다.  
프로젝트 홈페이지는 https://crc32c.machinezoo.com/  다.  
이것도 마찬가지로 빌드에 필요한 natvie dll 을 다음 부분에서 다운받을 수 있다.  
```
Alternatively, you can download plain 7-Zip archive of the DLLs. 
Sources are available via Bitbucket repository. 
.NET code is distributed under BSD license. Underlying C++ code is distributed under zlib license.
```
