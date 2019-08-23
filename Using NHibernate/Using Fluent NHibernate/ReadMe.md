### https://github.com/FluentNHibernate/fluent-nhibernate/wiki/Getting-started

- FluentNH 는 NH 의 표준 매핑 파일 (.hbm.xml) 을 C# 코드로 대체한다.  
- C# 코드 사용의 장점  
  - XML 의 복잡스러움에서 탈출  
  - 리팩토링이 용이해짐  
  - 매핑정보 요류를 컴파일 타임에 감지  
- Mapping 클래스들은 MappingSet partial class 를 하나 만들어 한데 묶었다.  
  .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MappingSet>()) 로 한번에 호출하기 위함
