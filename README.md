-- 블럭 매치 --

7월 3주차 
24.7.15 ~ 3매치 프로토타입 제작

// TODO : 
1. UI & 게임 세팅
	- 바구니 추가
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
	- 모서리 방향으로 스와이프 했을 경우 벽에 튕겨나오게(현재는 에러 안나게 막아놓기만 함)

3. 배열 족보
	- 4배열 직선, 4배열 네모, 5배열 직선, 5배열 L자 족보 만들어야 함

4. 블럭 종료
	- 특수 블럭, 방해 블럭 추가
	- 배열 족보로 발생하는 특수 블럭 생성
	- 특수 블럭 발동 시 로직
                 
5. 게임 종료
	- 바구니 목표 블럭 채웠을 때 승리하는 조건 추가
	- 승리 시 다음 스테이지로 이동
	- 패배 시 & 클리어 시 점수 및 재료 수급
