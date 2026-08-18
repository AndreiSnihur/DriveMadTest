# Drive Mad — прототип первого уровня

Клон первого уровня Drive Mad с упором на ощущение физики машины. Unity 6000.3.22f1 (URP), Input System, uGUI.

## Запуск
1. Открыть корень проекта в Unity 6000.3.x.
2. Открыть `Assets/Scenes/Level_1.unity`, нажать Play. Для портретной раскладки поставить Game view в 9:16.

## Управление
- Клавиатура: `D` / `→` — вперёд, `A` / `←` — назад, `R` / `Space` — рестарт
- Тач: экранные кнопки `<` `>` и `RESTART` (эмулируют те же клавиши через Input System)

## Игровой цикл
Победа — доехать до финишных ворот. Поражение — приземлиться на крышу или упасть с трассы:
колёса отваливаются, через секунду уровень перезапускается. `R` перезапускает в любой момент.

## Структура
- `Assets/CodeBase/Car` — `CarMover` (мотор), `Wheel` (мотор хинджа, подвеска, отстрел), `CarDetacher` (краш),
  `RoofSensor`, `LooseWheel` (обломок колеса), `CarSettings` (весь тюнинг, ScriptableObject)
- `Assets/CodeBase/Game` — `LevelController` (победа / поражение / рестарт), `GameInput` (обёртка Input System)
- `Assets/CodeBase/Level` — `CarTriggerZone` (финиш / kill-зона)
- `Assets/CodeBase/System` — `CameraFollower`
- `Assets/CodeBase/UI` — `HudView` (статус, тач-кнопки)
- `Assets/CodeBase/Input` — `CarControls.cs`, генерируется из `Assets/Input/CarControls.inputactions`
- `Assets/Prefabs` — `Car` (кузов + `Suspension` ×2 + `WheelPairFront` ×2), `LooseWheel`, `Level_1`
  (`Track_chunk_01`, `Ramp`, `Start_Finish`, триггеры финиша / kill-зоны, стены в начале и конце), `System` (камера, свет, ввод, HUD)
- `Assets/Settings/CarSettings.asset` — настройки машины, `Assets/PhysicsMaterials` — `Wheel`, `Chassis`

## Физическая модель
2D-в-3D: машина едет вдоль **−Z**; у всех Rigidbody машины заморожены позиция по X и вращение по Y/Z.
Кузов + два хаба подвески (`ConfigurableJoint`, ход только по Y с пружинным драйвом) + два сферических колеса
на моторизованных `HingeJoint` (ось X). Реакция момента колёс естественно наклоняет кузов (вилли, стоппи, перевороты).
Коллайдер кузова — два бокса (корпус + кабина); триггер на крыше (`RoofSensor`) фиксирует приземление на крышу.

Слои: `Ground` 6 (трасса), `Car` 7 (все части машины, коллизии Car×Car выключены), `Debris` 8 (отстреленные колёса).

## Тюнинг
Все параметры лежат в `Assets/Settings/CarSettings.asset` и применяются на лету в Play Mode: гравитация, момент мотора,
максимальная скорость колёс, масса / центр масс / угловое затухание кузова, масса и инерция вращения колёс,
пружина и демпфер подвески. Задержка авторестарта — на `LevelController` в префабе `System`.
