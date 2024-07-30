-- 블럭 매치 --

240730

1 제자리 클릭했을 때 완료
- 제자리 클릭 시 스와이프 안되게 적용
- 일반 블럭의 경우 클릭중일 때 반짝이는 이미지로 변경
- 특수 블럭의 경우 제자리 클릭 시 효과 적용
- 제자리 프리즘 발동 시 가장 많은 색상 찾아서 해당 색상 블럭 제거 완료

TODO: 1. 제자리 프리즘 가장 많은 색상이 두 개 이상일 때 처리

7월 5주차

// TODO : 
1. UI & 게임 세팅
   	- 바구니 임시 세팅 적용 변경 -> 게임 시작 시 랜덤하게 기본 7가지 블럭 중에서 선택되고, 목표 개수도 랜덤으로 선택되도록
	- 스테이지 적용
	  -> 스테이지 별 맵 사이즈, 바구니 목표, 남은 시간, 배경 이미지 등
	- 스테이지 선택 씬 제작
   	    - 제작 UI 적용

2. 블럭 배치
	- 처음 보드 생성될 때 위로 안보이게 블럭 추가 생성(7x7 기준 7x14, 7,21..)
	- 미리 생성된 블럭이 내려오게 변경
	- 모든 블럭이 일정한 속도로 내려오게(현재는 같은 시간에 한번에 내려오고 있음)
	- 블럭이 제거된 후 위에서 블럭이 내려오도록 변경
	
3. 게임 동작
   	- 특수 블럭의 경우 그 자리에서 터치했을 때 터지도록 v
   	- 일반 블럭 눌렀을 때 밝은 sprite 적용 v
   	- 제거 시 제거 이펙트 적용
   	- 그냥 클릭 시 오른쪽으로 스와이프됨(그냥 클릭 시 스와이프 안되게 적용해야함) v
  
4. 동작 대기 상태
	- 일정 시간이 지나고 조작이 없는 경우 매칭되는 블럭 표시
	- 매칭되는 블럭이 없는 경우 보드 다시 셔플

5. 특수 블럭 생성
	- 족보 매칭 시 기본 블럭에서 특수 블럭으로 변환(현재는 우선순위 적용하여 위에서 특수블럭 생성중)
	- 곡괭이 계속 생성되는 중

6. 특수블럭 기능
   	- 각 특수블럭 효과 이펙트(터질 때 움직이는 효과) 적용
   	- 특수블럭 조합 효과 적용
	- 프리즘 효과 발생 시 프리즘 블럭은 터질 때 안사라짐 v

7. 게임 종료
	- 바구니 목표 블럭 채웠을 때 승리하는 조건 추가
 	- 남은 시간 10초 이하 위험 효과 UI 적용
	- 리워드 창 제작(클리어 시, 실패 시)
	- 승리 시 다음 스테이지로 이동
	- 패배 시 & 클리어 시 점수 및 재료 수급
	- 리워드 창 제작(클리어 시, 실패 시)

8. 빌드 에러
   	- CustPropertyDrawer 파일 에러남 (보드 세팅 편하게 하기 위해 설정한 스크립트로 게임 내 지장은 없어서 삭제 후 빌드중)
  
9. 리팩토링



  

7월 4주차
1. 특수 블럭 기능 제작 1차 완료
2. 바구니 임시 추가
   


7월 3주차 
24.7.15 ~ 3매치 프로토타입 제작

// TODO : 
1. UI & 게임 세팅
	- 바구니 추가 v
	- 스테이지 적용
	  -> 스테이지 별 맵 사이즈, 바구니 목표, 남은 시간, 배경 이미지 등
	- 스테이지 선택 씬 제작
   	    - 제작 UI 적용

2. 블럭 배치
	- 처음 보드 생성될 때 위로 안보이게 블럭 추가 생성(7x7 기준 7x14, 7,21..)
	- 미리 생성된 블럭이 내려오게 변경
	- 모든 블럭이 일정한 속도로 내려오게(현재는 같은 시간에 한번에 내려오고 있음)
	- 게임 도중 매칭 경우의 수가 없는 경우 다시 섞여야 함
	  -> 최초 블럭 생성 시에는 매칭 확인 후 생성됨
	- 일정 시간이 지나고 조작이 없는 경우 매칭되는 블럭 표시
	- 모서리 방향으로 스와이프 했을 경우 벽에 튕겨나오게(현재는 에러 안나게 막아놓기만 함) x 기획에서 사라짐

3. 배열 족보
	- 4배열 직선, 4배열 네모, 5배열 직선, 5배열 L자 족보 만들어야 함 v

4. 블럭 종료
	- 특수 블럭, 방해 블럭 추가
	- 배열 족보로 발생하는 특수 블럭 생성
	- 특수 블럭 발동 시 로직
                 
5. 게임 종료
	- 바구니 목표 블럭 채웠을 때 승리하는 조건 추가
	- 승리 시 다음 스테이지로 이동
	- 패배 시 & 클리어 시 점수 및 재료 수급
